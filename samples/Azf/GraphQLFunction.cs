using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HotChocolate.AzureFunctions;
namespace Azf;

public class GraphQLFunction
{
    [FunctionName("GraphQLHttpFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "persons/{**slug}")] 
        HttpRequest request,
        [GraphQL] 
        IMultiSchemaRequestExecutor executor)
        => executor.ExecuteAsync(request, "persons");
}