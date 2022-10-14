using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Azf.IsolatedProcess;

public class GraphQLFunction2
{
    private readonly IMultiSchemaRequestExecutor _executor;

    public GraphQLFunction2(IMultiSchemaRequestExecutor executor)
    {
        _executor = executor;
    }

    [Function(nameof(GraphQLFunction2))]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "persons2/{**slug}")]
        HttpRequestData request)
        => _executor.ExecuteAsync(request, "persons2");
}
