using AsbBianTechnicalTest.Models;

namespace AsbBianTechnicalTest.Services;

/// <summary>
/// Interface for the entitlement service that checks if a subject has a specific permission on a resource. This service interacts with the Neo4j graph database to evaluate entitlements based on the relationships between identities, roles, entitlements, and resources.    
/// </summary>
public interface IEntitlementService
{
    /// <summary>
    /// Checks if the subject has the specified permission on the resource.
    /// </summary>
    /// <param name="subjectId">The ID of the subject (user) to check.</param>
    /// <param name="permission">The permission to check.</param>
    /// <param name="resourceId">The ID of the resource to check against.</param>
    /// <returns>An <see cref="EntitlementCheckResult"/> indicating whether the permission is granted.</returns>
    Task<EntitlementCheckResult> CheckEntitlementAsync(string subjectId, string permission, string resourceId);
}
