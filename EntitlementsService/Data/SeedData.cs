using AsbBianTechnicalTest.Services;

namespace AsbBianTechnicalTest.Data;

public static class SeedData
{
    public static async Task PopulateGraphAsync(INeo4jService neo4jService)
    {
        // Create Identity nodes
        var createIdentities = @"
            CREATE (c1:Identity {id: 'CUST-001', customerNumber: 'C001', name: 'John Doe'})
            CREATE (c2:Identity {id: 'CUST-002', customerNumber: 'C002', name: 'Jane Smith'})
            ";
        await neo4jService.ExecuteQueryAsync(createIdentities, []);

        // Create PartyRole nodes
        var createRoles = @"
            CREATE (r1:PartyRole {roleId: 'ROLE-001', roleType: 'AccountHolder', name: 'Account Holder'})
            CREATE (r2:PartyRole {roleId: 'ROLE-002', roleType: 'BusinessOwner', name: 'Business Owner'})
            CREATE (r3:PartyRole {roleId: 'ROLE-003', roleType: 'ThirdParty', name: 'Third Party'})
            ";
        await neo4jService.ExecuteQueryAsync(createRoles, []);

        // Create Entitlement nodes
        var createEntitlements = @"
            CREATE (e1:Entitlement {entitlementId: 'ENT-001', permission: 'VIEW_BALANCE', name: 'ViewAccountBalance'})
            CREATE (e2:Entitlement {entitlementId: 'ENT-002', permission: 'TRANSFER_FUNDS', name: 'TransferFunds'})
            CREATE (e3:Entitlement {entitlementId: 'ENT-003', permission: 'VIEW_DASHBOARD', name: 'ViewBusinessDashboard'})
            CREATE (e4:Entitlement {entitlementId: 'ENT-004', permission: 'VIEW_HISTORY', name: 'ViewTransactionHistory'})
            ";
        await neo4jService.ExecuteQueryAsync(createEntitlements, []);

        // Create Resource nodes
        var createResources = @"
            CREATE (res1:Resource {resourceId: 'ACC-12345', resourceType: 'Account', name: 'Checking Account'})
            CREATE (res2:Resource {resourceId: 'DASHB-001', resourceType: 'Dashboard', name: 'Business Dashboard'})
            ";
        await neo4jService.ExecuteQueryAsync(createResources, []);

        // Create relationships: Identity HAS_ROLE PartyRole
        var createHasRoleRelationships = @"
            MATCH (c:Identity {id: 'CUST-001'}), (r1:PartyRole {roleType: 'AccountHolder'})
            CREATE (c)-[:HAS_ROLE]->(r1)
            ";
        await neo4jService.ExecuteQueryAsync(createHasRoleRelationships, []);

        var createHasRoleRelationships2 = @"
            MATCH (c:Identity {id: 'CUST-001'}), (r2:PartyRole {roleType: 'BusinessOwner'})
            CREATE (c)-[:HAS_ROLE]->(r2)
            ";
        await neo4jService.ExecuteQueryAsync(createHasRoleRelationships2, []);

        var createHasRoleRelationships3 = @"
            MATCH (c:Identity {id: 'CUST-002'}), (r1:PartyRole {roleType: 'AccountHolder'})
            CREATE (c)-[:HAS_ROLE]->(r1)
            ";
        await neo4jService.ExecuteQueryAsync(createHasRoleRelationships3, []);

        var createHasRoleRelationships4 = @"
            MATCH (c:Identity {id: 'CUST-002'}), (r3:PartyRole {roleType: 'ThirdParty'})
            CREATE (c)-[:HAS_ROLE]->(r3)
            ";
        await neo4jService.ExecuteQueryAsync(createHasRoleRelationships4, []);

        // Create relationships: PartyRole ASSIGNS Entitlement
        var createAssignsRelationships = @"
            MATCH (r:PartyRole {roleType: 'AccountHolder'}), (e1:Entitlement {permission: 'VIEW_BALANCE'})
            CREATE (r)-[:ASSIGNS]->(e1)
            ";
        await neo4jService.ExecuteQueryAsync(createAssignsRelationships, []);

        var createAssignsRelationships2 = @"
            MATCH (r:PartyRole {roleType: 'AccountHolder'}), (e2:Entitlement {permission: 'TRANSFER_FUNDS'})
            CREATE (r)-[:ASSIGNS]->(e2)
            ";
        await neo4jService.ExecuteQueryAsync(createAssignsRelationships2, []);

        var createAssignsRelationships3 = @"
            MATCH (r:PartyRole {roleType: 'BusinessOwner'}), (e3:Entitlement {permission: 'VIEW_DASHBOARD'})
            CREATE (r)-[:ASSIGNS]->(e3)
            ";
        await neo4jService.ExecuteQueryAsync(createAssignsRelationships3, []);

        var createAssignsRelationships4 = @"
            MATCH (r:PartyRole {roleType: 'ThirdParty'}), (e4:Entitlement {permission: 'VIEW_HISTORY'})
            CREATE (r)-[:ASSIGNS]->(e4)
            ";
        await neo4jService.ExecuteQueryAsync(createAssignsRelationships4, []);

        // Create relationships: Entitlement GRANTS_ACCESS_TO Resource
        var createGrantsAccessRelationships = @"
            MATCH (e1:Entitlement {permission: 'VIEW_BALANCE'}), (res1:Resource {resourceId: 'ACC-12345'})
            CREATE (e1)-[:GRANTS_ACCESS_TO]->(res1)
            ";
        await neo4jService.ExecuteQueryAsync(createGrantsAccessRelationships, []);

        var createGrantsAccessRelationships2 = @"
            MATCH (e2:Entitlement {permission: 'TRANSFER_FUNDS'}), (res1:Resource {resourceId: 'ACC-12345'})
            CREATE (e2)-[:GRANTS_ACCESS_TO]->(res1)
            ";
        await neo4jService.ExecuteQueryAsync(createGrantsAccessRelationships2, []);

        var createGrantsAccessRelationships3 = @"
            MATCH (e3:Entitlement {permission: 'VIEW_DASHBOARD'}), (res2:Resource {resourceId: 'DASHB-001'})
            CREATE (e3)-[:GRANTS_ACCESS_TO]->(res2)
            ";
        await neo4jService.ExecuteQueryAsync(createGrantsAccessRelationships3, []);

        var createGrantsAccessRelationships4 = @"
            MATCH (e4:Entitlement {permission: 'VIEW_HISTORY'}), (res1:Resource {resourceId: 'ACC-12345'})
            CREATE (e4)-[:GRANTS_ACCESS_TO]->(res1)
            ";
        await neo4jService.ExecuteQueryAsync(createGrantsAccessRelationships4, []);
    }

    public static async Task ClearGraphAsync(INeo4jService neo4jService)
    {
        var deleteAll = "MATCH (n) DETACH DELETE n";
        await neo4jService.ExecuteQueryAsync(deleteAll, []);
    }
}
