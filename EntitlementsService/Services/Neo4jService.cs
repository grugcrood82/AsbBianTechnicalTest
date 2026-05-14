using Neo4j.Driver;
using AsbBianTechnicalTest.Configuration;

namespace AsbBianTechnicalTest.Services;

/// <summary>
/// Provides services for executing queries against a Neo4j graph database.
/// </summary>
public class Neo4jService : INeo4jService
{
    private readonly IDriver _driver;
    private readonly string _database;

    /// <summary>
    /// Initializes a new instance of the Neo4jService class with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration settings for connecting to the Neo4j database.</param>
    public Neo4jService(Neo4jConfig config)
    {
        _driver = GraphDatabase.Driver(
            config.Uri,
            AuthTokens.Basic(config.Username, config.Password)
        );
        _database = config.Database;
    }

    /// <inheritdoc/>
    public async Task ExecuteQueryAsync(string cypher, Dictionary<string, object> parameters)
    {
        await using var session = _driver.AsyncSession(options => options.WithDatabase(_database));
        await session.RunAsync(cypher, parameters);
    }

    /// <inheritdoc/>
    public async Task<List<T>> ExecuteQueryAsync<T>(
        string cypher,
        Dictionary<string, object> parameters,
        Func<IRecord, T> mapRecord)
    {
        await using var session = _driver.AsyncSession(options => options.WithDatabase(_database));
        var result = await session.RunAsync(cypher, parameters);
        var records = new List<T>();

        await foreach (var record in result)
        {
            records.Add(mapRecord(record));
        }

        return records;
    }
    /// <summary>
    /// Performs asynchronous cleanup of resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_driver != null)
        {
            await _driver.DisposeAsync();
        }
    }
}

