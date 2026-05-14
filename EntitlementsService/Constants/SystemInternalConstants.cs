namespace AsbBianTechnicalTest.Constants;

/// <summary>
/// Contains internal constants and identifiers used throughout the system.
/// These strings are not typically visible to end users but are used internally
/// for operation IDs, routes, property names, and logging.
/// </summary>
public static class SystemInternalConstants
{
    /// <summary>
    /// Constants related to the CheckEntitlement operation.
    /// </summary>
    public static class CheckEntitlement
    {
        /// <summary>
        /// Operation ID for the CheckEntitlement function (used in OpenAPI/Swagger).
        /// </summary>
        public const string OperationId = "checkEntitlement";

        /// <summary>
        /// HTTP route for the CheckEntitlement endpoint.
        /// </summary>
        public const string Route = "entitlement/check";

        /// <summary>
        /// Azure Function name for the CheckEntitlement function.
        /// </summary>
        public const string FunctionName = "CheckEntitlement";

        /// <summary>
        /// HTTP method for the CheckEntitlement endpoint.
        /// </summary>
        public const string HttpMethod = "post";

        /// <summary>
        /// Log message for incoming requests.
        /// </summary>
        public const string RequestReceivedLog = "Entitlement check request received";

        /// <summary>
        /// Log message template for entitlement check results.
        /// </summary>
        public const string ResultLogTemplate = "Entitlement check result: {0}";

        /// <summary>
        /// Log message template for errors during entitlement checks.
        /// </summary>
        public const string ErrorLogTemplate = "Error during entitlement check: {0}";
    }

    /// <summary>
    /// Constants related to the SeedData operation.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Operation ID for the SeedData function (used in OpenAPI/Swagger).
        /// </summary>
        public const string OperationId = "seedData";

        /// <summary>
        /// HTTP route for the SeedData endpoint.
        /// </summary>
        public const string Route = "entitlement/seed";

        /// <summary>
        /// Azure Function name for the SeedData function.
        /// </summary>
        public const string FunctionName = "SeedData";

        /// <summary>
        /// HTTP method for the SeedData endpoint.
        /// </summary>
        public const string HttpMethod = "post";

        /// <summary>
        /// Log message for incoming seed requests.
        /// </summary>
        public const string RequestReceivedLog = "Seed data request received";

        /// <summary>
        /// Log message for successful graph clearing.
        /// </summary>
        public const string DataClearedLog = "Cleared existing graph data";

        /// <summary>
        /// Log message for successful graph population.
        /// </summary>
        public const string DataPopulatedLog = "Graph seeded successfully";

        /// <summary>
        /// Log message template for errors during seeding.
        /// </summary>
        public const string ErrorLogTemplate = "Error seeding data: {0}";

        /// <summary>
        /// Number of nodes created during seeding.
        /// </summary>
        public const int NodesCreated = 13;
    }

    /// <summary>
    /// Constants for Neo4j graph structure and property names.
    /// </summary>
    public static class Graph
    {
        /// <summary>
        /// Neo4j node label for Identity.
        /// </summary>
        public const string IdentityLabel = "Identity";

        /// <summary>
        /// Neo4j node label for PartyRole.
        /// </summary>
        public const string PartyRoleLabel = "PartyRole";

        /// <summary>
        /// Neo4j node label for Entitlement.
        /// </summary>
        public const string EntitlementLabel = "Entitlement";

        /// <summary>
        /// Neo4j node label for Resource.
        /// </summary>
        public const string ResourceLabel = "Resource";

        /// <summary>
        /// Neo4j relationship type: Identity has role.
        /// </summary>
        public const string HasRoleRelationship = "HAS_ROLE";

        /// <summary>
        /// Neo4j relationship type: Role assigns entitlement.
        /// </summary>
        public const string AssignsRelationship = "ASSIGNS";

        /// <summary>
        /// Neo4j relationship type: Entitlement grants access.
        /// </summary>
        public const string GrantsAccessRelationship = "GRANTS_ACCESS_TO";

        /// <summary>
        /// Neo4j property name for identity ID.
        /// </summary>
        public const string IdProperty = "id";

        /// <summary>
        /// Neo4j property name for resource ID.
        /// </summary>
        public const string ResourceIdProperty = "resourceId";

        /// <summary>
        /// Neo4j property name for permission.
        /// </summary>
        public const string PermissionProperty = "permission";

        /// <summary>
        /// Neo4j property name for role type.
        /// </summary>
        public const string RoleTypeProperty = "roleType";

        /// <summary>
        /// Neo4j property name for entitlement name.
        /// </summary>
        public const string EntitlementNameProperty = "name";
    }

    /// <summary>
    /// Constants for Cypher queries.
    /// </summary>
    public static class Cypher
    {
        /// <summary>
        /// Parameter name for subject ID in Cypher queries.
        /// </summary>
        public const string SubjectIdParameter = "subjectId";

        /// <summary>
        /// Parameter name for permission in Cypher queries.
        /// </summary>
        public const string PermissionParameter = "permission";

        /// <summary>
        /// Parameter name for resource ID in Cypher queries.
        /// </summary>
        public const string ResourceIdParameter = "resourceId";

        /// <summary>
        /// Query result field name for identity ID.
        /// </summary>
        public const string IdentityIdField = "identityId";

        /// <summary>
        /// Query result field name for role type.
        /// </summary>
        public const string RoleTypeField = "roleType";

        /// <summary>
        /// Query result field name for entitlement name.
        /// </summary>
        public const string EntitlementNameField = "entitlementName";

        /// <summary>
        /// Query result limit for entitlement checks (ensures single result).
        /// </summary>
        public const int QueryLimit = 1;
    }

    /// <summary>
    /// Constants for JSON serialization and HTTP content types.
    /// </summary>
    public static class Http
    {
        /// <summary>
        /// Content type for JSON responses.
        /// </summary>
        public const string JsonContentType = "application/json";

        /// <summary>
        /// Property name for error objects in responses.
        /// </summary>
        public const string ErrorProperty = "error";

        /// <summary>
        /// Property name for success indicator in responses.
        /// </summary>
        public const string SuccessProperty = "success";

        /// <summary>
        /// Property name for message in responses.
        /// </summary>
        public const string MessageProperty = "message";

        /// <summary>
        /// Property name for nodes count in seed responses.
        /// </summary>
        public const string NodesCreatedProperty = "nodesCreated";
    }

    /// <summary>
    /// Constants for Neo4j configuration.
    /// </summary>
    public static class Neo4jConfig
    {
        /// <summary>
        /// Environment variable name for Neo4j connection URI.
        /// </summary>
        public const string UriVariable = "NEO4J_URI";

        /// <summary>
        /// Default Neo4j connection URI.
        /// </summary>
        public const string DefaultUri = "neo4j://localhost:7687";

        /// <summary>
        /// Environment variable name for Neo4j username.
        /// </summary>
        public const string UsernameVariable = "NEO4J_USERNAME";

        /// <summary>
        /// Default Neo4j username.
        /// </summary>
        public const string DefaultUsername = "neo4j";

        /// <summary>
        /// Environment variable name for Neo4j password.
        /// </summary>
        public const string PasswordVariable = "NEO4J_PASSWORD";

        /// <summary>
        /// Default Neo4j password.
        /// </summary>
        public const string DefaultPassword = "password";

        /// <summary>
        /// Environment variable name for Neo4j database name.
        /// </summary>
        public const string DatabaseVariable = "NEO4J_DATABASE";

        /// <summary>
        /// Default Neo4j database name.
        /// </summary>
        public const string DefaultDatabase = "neo4j";
    }
}
