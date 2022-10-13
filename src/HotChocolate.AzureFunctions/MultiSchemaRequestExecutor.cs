using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using static HotChocolate.AspNetCore.MiddlewareRoutingType;
namespace HotChocolate.AzureFunctions;

internal sealed class MultiSchemaRequestExecutor : IGraphQLRequestExecutor
{
    private readonly EmptyResult _result = new();

    private readonly GraphQLServerOptions _options;
    private readonly Dictionary<string, (RequestDelegate, GraphQLServerOptions)> endpoints = new();
    private readonly IServiceProvider sp;

    public MultiSchemaRequestExecutor(IServiceProvider sp, GraphQLServerOptions options)
    {
        this.sp = sp;
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void BuildPipeline(string schemaNameOrDefault, PathString path, GraphQLServerOptions options)
    {
        if(endpoints.ContainsKey(schemaNameOrDefault)) return;

        var fileProvider = CreateFileProvider();

        var appBuilder = new ApplicationBuilder(sp)
                    // .UseCancellation()
                    .UseMiddleware<WebSocketSubscriptionMiddleware>(schemaNameOrDefault)
                    .UseMiddleware<HttpPostMiddleware>(schemaNameOrDefault)
                    .UseMiddleware<HttpMultipartMiddleware>(schemaNameOrDefault)
                    .UseMiddleware<HttpGetSchemaMiddleware>(schemaNameOrDefault, Integrated)
                    .UseMiddleware<ToolDefaultFileMiddleware>(fileProvider, path)
                    .UseMiddleware<ToolOptionsFileMiddleware>(path)
                    .UseMiddleware<ToolStaticFileMiddleware>(fileProvider, path)
                    .UseMiddleware<HttpGetMiddleware>(schemaNameOrDefault)
                    .Use(_ => context =>
                    {
                        context.Response.StatusCode = 404;
                        return Task.CompletedTask;
                    });

        endpoints.Add(schemaNameOrDefault, (appBuilder.Build(), options));
    }

    public async Task<IActionResult> ExecuteAsync(HttpContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        PathString path = context.Request.Path.ToString().TrimEnd('/');
        string schemaNameOrDefault;

        if (path.ToString().Contains('/'))
            schemaNameOrDefault = path.ToString().Split('/').Last();
        else
            schemaNameOrDefault = Schema.DefaultName;

        BuildPipeline(schemaNameOrDefault, path, _options);

         (var pipeline, var options)= endpoints[schemaNameOrDefault];
        // TODO: check by server?
        // First we need to populate the HttpContext with the current GraphQL server options ...
        context.Items.Add(nameof(GraphQLServerOptions), _options);

        // after that we can execute the pipeline ...
        await pipeline.Invoke(context).ConfigureAwait(false);

        // last we return out empty result that we have cached in this class.
        // the pipeline actually takes care of writing the result to the response stream.
        return _result;
    }

    private static IFileProvider CreateFileProvider()
    {
        var type = typeof(HttpMultipartMiddleware);
        var resourceNamespace = typeof(MiddlewareBase).Namespace + ".Resources";
        return new EmbeddedFileProvider(type.Assembly, resourceNamespace);
    }
}
