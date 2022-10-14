using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    // .AddGraphQLFunction(b => b.AddQueryType<Query>())
    .ConfigureServices(s =>{
        s.AddGraphQLServer("persons")
        .AddQueryType<Query>();
    })
    .AddGraphQLFunctions()
    .Build();

host.Run();
