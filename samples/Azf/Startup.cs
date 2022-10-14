[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddGraphQLServer("persons")
                        .AddQueryType<Query>();

        builder.Services.AddGraphQLServer("persons2")
                        .AddQueryType<Query2>();

        builder.AddGraphQLFunctions();
        // builder
        //     .AddGraphQLFunction()
        //     .AddQueryType<Query>();
    }
}
