using HotChocolate.AzureFunctions.IsolatedProcess;
using Microsoft.Azure.Functions.Worker.Http;

namespace HotChocolate.AzureFunctions;

public static class MultiSchemaRequestExecutorExtensions
{
    public static Task<HttpResponseData> ExecuteAsync(
        this IMultiSchemaRequestExecutor executor,
        HttpRequestData requestData, string schemaName)
    {
        if (executor is null)
        {
            throw new ArgumentNullException(nameof(executor));
        }

        if (requestData is null)
        {
            throw new ArgumentNullException(nameof(requestData));
        }

        return ExecuteGraphQLRequestInternalAsync(executor, requestData, schemaName);
    }

    private static async Task<HttpResponseData> ExecuteGraphQLRequestInternalAsync(
        IMultiSchemaRequestExecutor executor,
        HttpRequestData requestData, string schemaName)
    {
        var context = new AzureHttpContext(requestData);
        await executor.ExecuteAsync(context, schemaName).ConfigureAwait(false);
        return context.CreateResponseData();
    }
}
