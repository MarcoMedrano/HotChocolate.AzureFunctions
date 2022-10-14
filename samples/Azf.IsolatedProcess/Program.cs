using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>{
        s.AddGraphQLServer("persons")
        .AddQueryType<Query>();

        s.AddGraphQLServer("persons2")
        .AddQueryType<Query2>();
    })
    .AddGraphQLFunctions()
    .Build();

host.Run();
