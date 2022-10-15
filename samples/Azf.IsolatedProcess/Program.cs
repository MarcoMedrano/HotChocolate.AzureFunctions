using Microsoft.Extensions.Hosting;

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
