using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotChocolate.AzureFunctions;

/// <summary>
/// Represents a GraphQL Over HTTP request executor.
/// </summary>
public interface IMultiSchemaRequestExecutor : IGraphQLRequestExecutor
{
    /// <summary>
    /// Executes a GraphQL over HTTP request.
    /// </summary>
    /// <param name="request">
    /// The HTTP request.
    /// </param>
    /// <param name="schemaName">
    /// The schema name, the one used with AddGraphQLServer.
    /// </param>
    /// <returns>
    /// returns the GraphQL HTTP response.
    /// </returns>
    Task<IActionResult> ExecuteAsync(HttpRequest request, string schemaName) => ExecuteAsync(request.HttpContext, schemaName);

    /// <summary>
    /// Executes a GraphQL over HTTP request.
    /// </summary>
    /// <param name="context">
    /// The HTTP context.
    /// </param>
    /// <param name="schemaName">
    /// The schema name, the one used with AddGraphQLServer.
    /// </param>
    /// <returns>
    /// returns the GraphQL HTTP response.
    /// </returns>
    Task<IActionResult> ExecuteAsync(HttpContext context, string schemaName);
}

