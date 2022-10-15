[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddGraphQLServer("persons") // schema 1
                        .AddQueryType<Query>();

        builder.Services.AddGraphQLServer("persons2") // schema 2
                        .AddQueryType<Query2>();

        builder.AddGraphQLFunctions(); // Add support for Azure FunctionS
    }
}
