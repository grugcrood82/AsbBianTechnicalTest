using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testcontainers.Neo4j;
using Neo4j.Driver;
using AsbBianTechnicalTest.Configuration;
using AsbBianTechnicalTest.Services;
using AsbBianTechnicalTest.Data;

namespace AsbTechnicalBianComplientTest.Fixtures;

[TestClass]
public sealed class Neo4jFixture
{
    private static Neo4jContainer? _container;
    private static string? _connectionString;
    private static IDriver? _driver;
    private static INeo4jService? _neo4jService;

    [AssemblyInitialize]
    public static async Task AssemblyInit(TestContext context)
    {
        _container = new Neo4jBuilder()
            .Build();

        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();

        // Initialize Neo4j service and seed data once for all tests
        var uri = _connectionString.Replace("bolt://", "neo4j://");
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", "password"));
        _neo4jService = new Neo4jService(new Neo4jConfig { Uri = uri, Username = "neo4j", Password = "password", Database = "neo4j" });

        // Seed demo data once for all tests
        await _neo4jService.ExecuteQueryAsync("MATCH (n) DETACH DELETE n", []);
        await SeedData.PopulateGraphAsync(_neo4jService);
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        if (_neo4jService != null)
        {
            try
            {
                await _neo4jService.DisposeAsync();
            }
            catch
            {
                // Ignore errors during dispose
            }
        }

        if (_driver != null)
        {
            try
            {
                await _driver.DisposeAsync();
            }
            catch
            {
                // Ignore errors during dispose
            }
        }

        if (_container != null)
        {
            try
            {
                await _container.StopAsync();
            }
            catch
            {
                // Ignore errors during stop if container is already stopping
            }

            try
            {
                await _container.DisposeAsync();
            }
            catch
            {
                // Ignore errors during dispose
            }
        }
    }

    public static string GetConnectionString()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Neo4j container not initialized");
        }
        return _connectionString;
    }

    public static Neo4jContainer GetContainer()
    {
        if (_container == null)
        {
            throw new InvalidOperationException("Neo4j container not initialized");
        }
        return _container;
    }
}
