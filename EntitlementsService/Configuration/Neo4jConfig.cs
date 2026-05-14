namespace AsbBianTechnicalTest.Configuration;

/// <summary>
/// Configuration class for Neo4j database connection settings.
/// </summary>
public class Neo4jConfig
{
    /// <summary>
    /// Gets or sets the Neo4j database connection URI.
    /// </summary>
    public string Uri { get; set; } = "neo4j://localhost:7687";
    /// <summary>
    /// Gets or sets the username for the Neo4j database connection.
    /// </summary>
    public string Username { get; set; } = "neo4j";
    /// <summary>
    /// Gets or sets the password for the Neo4j database connection.
    /// </summary>
    public string Password { get; set; } = "password";
    /// <summary>
    /// Gets or sets the Neo4j database name.
    /// </summary>
    public string Database { get; set; } = "neo4j";
}
