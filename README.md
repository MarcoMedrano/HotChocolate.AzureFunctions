# Multi Schema / Endpoint for HotChocolate Azure Functions.
Azure supports multiple endpoints using multiple functions, official implementation of HotChocolate (as 13.0.0-preview.66) does not yet. 

> I let it be as mush as possible *aspnet* like implementation, so we do not need to change a lot of code when coming from server project.


## Azure Functions **in process**
### Install
```shell
dotnet add package (wip Markind?).HotChocolate.AzureFunctions
```
### Startup
```csharp
[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddGraphQLServer("persons") // schema 1
                        .AddQueryType<Query>();

        builder.Services.AddGraphQLServer("persons2") // schema 2, etc..
                        .AddQueryType<Query2>();

        builder.AddGraphQLFunctions(); // Add support for Azure FunctionS
    }
}
```

### Function
Make sure to use **IMultiSchemaRequestExecutor**.
```csharp
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
```
Create the other functions and make sure to set the schema name `executor.ExecuteAsync(request, "persons2");`

Full sample at [samples/Azf](https://github.com/MarcoMedrano/HotChocolate.AzureFunctions/tree/main/samples/Azf)


---

## Azure Functions **Isolated Process**

### Install
```shell
dotnet add package (wip Markind?).HotChocolate.AzureFunctions.IsolatedProcess
```
### Program
```csharp
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>{
        s.AddGraphQLServer("persons") // schema 1
        .AddQueryType<Query>();

        s.AddGraphQLServer("persons2") // schema 2 , etc.
        .AddQueryType<Query2>();
    })
    .AddGraphQLFunctions()// Add support for Azure FunctionS
    .Build();

host.Run();
```
Similarly if using Startup.cs

### Function
Again make sure to use **IMultiSchemaRequestExecutor**.
```csharp
public class GraphQLFunction
{
    private readonly IMultiSchemaRequestExecutor _executor;

    public GraphQLFunction(IMultiSchemaRequestExecutor executor)
    {
        _executor = executor;
    }

    [Function("GraphQLHttpFunction")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "persons/{**slug}")]
        HttpRequestData request)
        => _executor.ExecuteAsync(request, "persons");
}
```
Create the other functions and make sure to set the schema name `executor.ExecuteAsync(request, "persons2");`

Full sample at [samples/Azf.IsolatedProcess](https://github.com/MarcoMedrano/HotChocolate.AzureFunctions/tree/main/samples/Azf.IsolatedProcess)

## [Buy me a Coffee ☕](https://www.buymeacoffee.com/marcomedrano)
*Glad to be of help, it took me more than a week cracking my head :') to understand how it should work and give the easiest and smoothly solution possible. I am willing to improve it or fix issues if needed. So if you wish*

<a href="https://www.buymeacoffee.com/marcomedrano" target="_blank">
<img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174">
</a> 

## Mentions
Base code is using official implementation of [HotChocolate.AzureFunctions](https://github.com/ChilliCream/hotchocolate/tree/main/src/HotChocolate/AzureFunctions) and I reviewed a lot [GraphQL.AzureFunctionsProxy](https://github.com/cajuncoding/GraphQL.AzureFunctionsProxy)

## Notice 
This implementation has original implementation from HotChocolate.AzureFunctions as well, I made this way with the hope of integrate it in the official HotChocolate repo. Be sure to use `.AddGraphQLFunctions()` with **s** at the end, using without it you will be using original implementation that has no multi schema support.