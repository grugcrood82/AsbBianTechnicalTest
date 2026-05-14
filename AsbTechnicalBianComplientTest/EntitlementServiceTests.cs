using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.Driver;
using AsbBianTechnicalTest.Configuration;
using AsbBianTechnicalTest.Services;
using AsbBianTechnicalTest.Data;
using AsbTechnicalBianComplientTest.Fixtures;

namespace AsbTechnicalBianComplientTest;

[TestClass]
public class EntitlementServiceTests
{
    private static IDriver? _driver;
    private static INeo4jService? _neo4jService;
    private static IEntitlementService? _entitlementService;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        var connectionString = Neo4jFixture.GetConnectionString();
        var uri = connectionString.Replace("bolt://", "neo4j://");

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", "password"));
        _neo4jService = new Neo4jService(new Neo4jConfig { Uri = uri, Username = "neo4j", Password = "password", Database = "neo4j" });
        _entitlementService = new EntitlementService(_neo4jService);

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
    [TestCategory("Entitlement")]
    public async Task CheckEntitlement_Allowed_AccountHolderViewsBalance()
    {
        // Arrange
        var subjectId = "CUST-001";
        var permission = "VIEW_BALANCE";
        var resourceId = "ACC-12345";

        // Act
        var result = await _entitlementService!.CheckEntitlementAsync(subjectId, permission, resourceId);

        // Assert
        Assert.IsTrue(result.Allowed, "Expected entitlement check to allow VIEW_BALANCE for AccountHolder");
        Assert.AreEqual("ViewAccountBalance", result.GrantedEntitlement);
        Assert.IsTrue(result.Path.Count > 0, "Expected non-empty path");
        Assert.IsTrue(result.Reason.Contains("granted"), "Expected reason to indicate grant");
    }

    [TestMethod]
    [TestCategory("Entitlement")]
    public async Task CheckEntitlement_Denied_NoEntitlementFound()
    {
        // Arrange
        var subjectId = "CUST-001";
        var permission = "DELETE_ACCOUNT"; // Non-existent permission
        var resourceId = "ACC-12345";

        // Act
        var result = await _entitlementService!.CheckEntitlementAsync(subjectId, permission, resourceId);

        // Assert
        Assert.IsFalse(result.Allowed, "Expected entitlement check to deny non-existent permission");
        Assert.IsNull(result.GrantedEntitlement);
        Assert.AreEqual(0, result.Path.Count, "Expected empty path for denied entitlement");
    }

    [TestMethod]
    [TestCategory("Entitlement")]
    public async Task CheckEntitlement_Denied_WrongResourceId()
    {
        // Arrange
        var subjectId = "CUST-001";
        var permission = "VIEW_BALANCE";
        var resourceId = "ACC-99999"; // Non-existent resource

        // Act
        var result = await _entitlementService!.CheckEntitlementAsync(subjectId, permission, resourceId);

        // Assert
        Assert.IsFalse(result.Allowed, "Expected entitlement check to deny when resource doesn't match");
        Assert.IsNull(result.GrantedEntitlement);
        Assert.AreEqual(0, result.Path.Count, "Expected empty path");
    }

    [TestMethod]
    [TestCategory("Entitlement")]
    public async Task CheckEntitlement_Allowed_MultipleRoles_SelectsCorrectPath()
    {
        // Arrange - CUST-001 has both AccountHolder and BusinessOwner roles
        var subjectId = "CUST-001";
        var permission = "VIEW_DASHBOARD";
        var resourceId = "DASHB-001";

        // Act
        var result = await _entitlementService!.CheckEntitlementAsync(subjectId, permission, resourceId);

        // Assert
        Assert.IsTrue(result.Allowed, "Expected entitlement check to allow VIEW_DASHBOARD for BusinessOwner");
        Assert.AreEqual("ViewBusinessDashboard", result.GrantedEntitlement);
        Assert.IsTrue(result.Path.Contains("BusinessOwner"), "Expected path to include BusinessOwner role");
    }

    [TestMethod]
    [TestCategory("Entitlement")]
    public async Task CheckEntitlement_Denied_InvalidSubjectId()
    {
        // Arrange
        var subjectId = "CUST-999"; // Non-existent customer
        var permission = "VIEW_BALANCE";
        var resourceId = "ACC-12345";

        // Act
        var result = await _entitlementService!.CheckEntitlementAsync(subjectId, permission, resourceId);

        // Assert
        Assert.IsFalse(result.Allowed, "Expected entitlement check to deny for non-existent subject");
        Assert.IsNull(result.GrantedEntitlement);
        Assert.AreEqual(0, result.Path.Count, "Expected empty path");
    }
}
