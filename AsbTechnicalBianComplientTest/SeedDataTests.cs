using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.Driver;
using AsbBianTechnicalTest.Configuration;
using AsbBianTechnicalTest.Services;
using AsbBianTechnicalTest.Data;
using AsbTechnicalBianComplientTest.Fixtures;

namespace AsbTechnicalBianComplientTest;

[TestClass]
public class SeedDataTests
{
    private static IDriver? _driver;
    private static INeo4jService? _neo4jService;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        var connectionString = Neo4jFixture.GetConnectionString();
        var uri = connectionString.Replace("bolt://", "neo4j://");

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", "password"));
        _neo4jService = new Neo4jService(new Neo4jConfig { Uri = uri, Username = "neo4j", Password = "password", Database = "neo4j" });

        // Data is seeded once in Neo4jFixture.AssemblyInit for all tests
    }

    [ClassCleanup]
    public static async Task ClassCleanup()
    {
        if (_neo4jService is not null)
        {
            await _neo4jService.DisposeAsync();
        }

        if (_driver is not null)
        {
            await _driver.DisposeAsync();
        }
    }

    [TestMethod]
    [TestCategory("SeedData")]
    public async Task SeedData_CreatesIdentities()
    {
        // Arrange
        var cypher = "MATCH (i:Identity) RETURN COUNT(i) as count";

        // Act
        var results = await _neo4jService!.ExecuteQueryAsync<int>(
            cypher,
            [],
            record => record["count"].As<int>()
        );

        // Assert
        Assert.IsTrue(results.Count > 0, "Expected at least one identity count result");
        Assert.AreEqual(2, results[0], "Expected 2 identity nodes (CUST-001, CUST-002)");
    }

    [TestMethod]
    [TestCategory("SeedData")]
    public async Task SeedData_CreatesEntitlements()
    {
        // Arrange
        var cypher = "MATCH (e:Entitlement) RETURN COUNT(e) as count";

        // Act
        var results = await _neo4jService!.ExecuteQueryAsync<int>(
            cypher,
            [],
            record => record["count"].As<int>()
        );

        // Assert
        Assert.IsTrue(results.Count > 0, "Expected at least one entitlement count result");
        Assert.AreEqual(4, results[0], "Expected 4 entitlement nodes (VIEW_BALANCE, TRANSFER_FUNDS, VIEW_DASHBOARD, VIEW_HISTORY)");
    }

    [TestMethod]
    [TestCategory("SeedData")]
    public async Task SeedData_CreatesResourcesAndRelationships()
    {
        // Arrange - verify graph structure
        var cypher = @"
            MATCH (i:Identity)-[:HAS_ROLE]->(r:PartyRole)-[:ASSIGNS]->(e:Entitlement)-[:GRANTS_ACCESS_TO]->(res:Resource)
            RETURN COUNT(*) as pathCount
            ";

        // Act
        var results = await _neo4jService!.ExecuteQueryAsync<int>(
            cypher,
            [],
            record => record["pathCount"].As<int>()
        );

        // Assert
        Assert.IsTrue(results.Count > 0, "Expected path count result");
        Assert.IsTrue(results[0] > 0, "Expected at least one complete traversal path");
        Assert.AreEqual(6, results[0], "Expected 6 complete paths (Identity -> Role -> Entitlement -> Resource)");
    }
}
