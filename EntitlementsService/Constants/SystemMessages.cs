namespace AsbBianTechnicalTest.Constants;

/// <summary>
/// Contains user-facing messages and descriptions for the system.
/// These strings are visible to API clients and end users.
/// </summary>
public static class SystemMessages
{
    /// <summary>
    /// Messages related to entitlement checks.
    /// </summary>
    public static class CheckEntitlement
    {
        /// <summary>
        /// Description of the CheckEntitlement operation for OpenAPI documentation.
        /// </summary>
        public const string OperationDescription = "Check if a subject has the specified permission on a resource";

        /// <summary>
        /// Description for the request body in OpenAPI documentation.
        /// </summary>
        public const string RequestBodyDescription = "Entitlement check request payload";

        /// <summary>
        /// Description for successful response in OpenAPI documentation.
        /// </summary>
        public const string SuccessResponseDescription = "Entitlement check completed successfully";

        /// <summary>
        /// Error message when required fields are missing.
        /// </summary>
        public const string MissingFieldsError = "Missing required fields: subjectId, permission, resourceId";

        /// <summary>
        /// Error message for internal server errors.
        /// </summary>
        public const string InternalServerError = "Internal server error";
    }

    /// <summary>
    /// Messages related to seed data operations.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Description for the SeedData operation for OpenAPI documentation.
        /// </summary>
        public const string OperationDescription = "Seed the graph database with demo data";

        /// <summary>
        /// Success message returned when seeding completes successfully.
        /// </summary>
        public const string SuccessMessage = "Demo graph data seeded successfully";

        /// <summary>
        /// Error message for failed seed operations.
        /// </summary>
        public const string ErrorMessage = "Failed to seed data";

        /// <summary>
        /// Error message template for seed operations with exception details.
        /// </summary>
        public const string ErrorMessageTemplate = "Failed to seed data: {0}";

        /// <summary>
        /// Description for internal server error response in OpenAPI documentation.
        /// </summary>
        public const string ErrorResponseDescription = "Failed to seed data - internal server error";
    }

    /// <summary>
    /// Messages related to entitlement service operations.
    /// </summary>
    public static class EntitlementService
    {
        /// <summary>
        /// Error message for invalid input parameters.
        /// </summary>
        public const string InvalidInputError = "Invalid input: subjectId, permission, and resourceId are required";

        /// <summary>
        /// Reason message template when no entitlement is found.
        /// </summary>
        public const string NoEntitlementFoundTemplate = "No entitlement found granting {0} on {1} for subject {2}";

        /// <summary>
        /// Reason message template when entitlement is granted.
        /// </summary>
        public const string EntitlementGrantedTemplate = "Permission {0} on resource {1} granted via role {2} and entitlement {3}";

        /// <summary>
        /// Error message template for entitlement check exceptions.
        /// </summary>
        public const string ErrorCheckingEntitlementTemplate = "Error checking entitlement: {0}";
    }

    /// <summary>
    /// API configuration messages.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// Title of the API for OpenAPI documentation.
        /// </summary>
        public const string Title = "ASB BIAN Technical Test API";

        /// <summary>
        /// Version of the API.
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// Description of the API for OpenAPI documentation.
        /// </summary>
        public const string Description = "API for checking entitlements based on a Neo4j graph database";
    }
}
