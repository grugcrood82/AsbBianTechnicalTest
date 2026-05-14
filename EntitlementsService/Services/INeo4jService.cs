using Neo4j.Driver;

namespace AsbBianTechnicalTest.Services;
/// <summary>
/// Provides methods for executing Cypher queries against a Neo4j database.
/// </summary>
public interface INeo4jService : IAsyncDisposable
{
    /// <summary>
    /// Executes a Cypher query with the provided parameters. This method is intended for queries that do not return results (e.g., CREATE, DELETE).    
    /// </summary>
    /// <param name="cypher">The Cypher query to execute.</param>
    /// <param name="parameters">The parameters for the Cypher query.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteQueryAsync(string cypher, Dictionary<string, object> parameters);
    /// <summary>
    /// Executes a Cypher query with the provided parameters and maps the results to a list of type <typeparamref name="T"/>.
    /// This method is intended for queries that return results (e.g., MATCH, RETURN).
    /// </summary>
    /// <typeparam name="T">The type to map the query results to.</typeparam>
    /// <param name="cypher">The Cypher query to execute.</param>
    /// <param name="parameters">The parameters for the Cypher query.</param>
    /// <param name="mapRecord">A function to map each record to an instance of <typeparamref name="T"/>.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of mapped results.</returns>
    Task<List<T>> ExecuteQueryAsync<T>(string cypher, Dictionary<string, object> parameters, Func<IRecord, T> mapRecord);
}
