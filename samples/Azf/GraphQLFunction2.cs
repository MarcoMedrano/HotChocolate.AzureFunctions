using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HotChocolate.AzureFunctions;
namespace Azf;

public class GraphQLFunction2
{
    [FunctionName(nameof(GraphQLFunction2))]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "persons2/{**slug}")]
        HttpRequest request,
        [GraphQL]
        IMultiSchemaRequestExecutor executor)
        => executor.ExecuteAsync(request, "persons2");
}