using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Azf.IsolatedProcess;

public class GraphQLFunction
{
    private readonly IMultiSchemaRequestExecutor _executor;

    public GraphQLFunction(IMultiSchemaRequestExecutor executor)
    {
        _executor = executor;
    }

    [Function("GraphQLHttpFunction")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "graphql/{**slug}")]
        HttpRequestData request)
        => _executor.ExecuteAsync(request, "persons");
}
