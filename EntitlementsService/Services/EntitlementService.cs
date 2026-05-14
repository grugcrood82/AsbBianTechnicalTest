using AsbBianTechnicalTest.Constants;
using AsbBianTechnicalTest.Models;
using Neo4j.Driver;

namespace AsbBianTechnicalTest.Services;

/// <summary>
/// Provides services for checking entitlements based on a Neo4j graph database. This service evaluates whether a given subject (e.g., user) has the necessary permissions to access a specified resource by traversing the relationships in the graph.
/// </summary>
public class EntitlementService : IEntitlementService
{
    private readonly INeo4jService _neo4jService;

    /// <summary>
    /// Initializes a new instance of the EntitlementService class with the specified Neo4j service.
    /// </summary>
    /// <param name="neo4jService">The Neo4j service used for executing queries.</param>
    public EntitlementService(INeo4jService neo4jService)
    {
        _neo4jService = neo4jService;
    }

    /// <inheritdoc/>
    public async Task<EntitlementCheckResult> CheckEntitlementAsync(string subjectId, string permission, string resourceId)
    {
        if (string.IsNullOrWhiteSpace(subjectId) || string.IsNullOrWhiteSpace(permission) || string.IsNullOrWhiteSpace(resourceId))
        {
            return new EntitlementCheckResult
            {
                Allowed = false,
                Reason = SystemMessages.EntitlementService.InvalidInputError,
                GrantedEntitlement = null,
                Path = []
            };
        }

                var cypher = @$"
        MATCH (i:{SystemInternalConstants.Graph.IdentityLabel} {{{SystemInternalConstants.Graph.IdProperty}: ${SystemInternalConstants.Cypher.SubjectIdParameter}}})-[:{SystemInternalConstants.Graph.HasRoleRelationship}]->(pr:{SystemInternalConstants.Graph.PartyRoleLabel})-[:{SystemInternalConstants.Graph.AssignsRelationship}]->(e:{SystemInternalConstants.Graph.EntitlementLabel})-[:{SystemInternalConstants.Graph.GrantsAccessRelationship}]->(r:{SystemInternalConstants.Graph.ResourceLabel} {{{SystemInternalConstants.Graph.ResourceIdProperty}: ${SystemInternalConstants.Cypher.ResourceIdParameter}}})
        WHERE e.{SystemInternalConstants.Graph.PermissionProperty} = ${SystemInternalConstants.Cypher.PermissionParameter}
        RETURN i.{SystemInternalConstants.Graph.IdProperty} as {SystemInternalConstants.Cypher.IdentityIdField}, pr.{SystemInternalConstants.Graph.RoleTypeProperty} as {SystemInternalConstants.Cypher.RoleTypeField}, e.{SystemInternalConstants.Graph.EntitlementNameProperty} as {SystemInternalConstants.Cypher.EntitlementNameField}, e.{SystemInternalConstants.Graph.PermissionProperty} as {SystemInternalConstants.Graph.PermissionProperty}, r.{SystemInternalConstants.Graph.ResourceIdProperty} as {SystemInternalConstants.Graph.ResourceIdProperty}
        LIMIT {SystemInternalConstants.Cypher.QueryLimit}
        ";

        var parameters = new Dictionary<string, object>
        {
            { SystemInternalConstants.Cypher.SubjectIdParameter, subjectId },
            { SystemInternalConstants.Cypher.PermissionParameter, permission },
            { SystemInternalConstants.Cypher.ResourceIdParameter, resourceId }
        };

        try
        {
            var results = await _neo4jService.ExecuteQueryAsync<EntitlementCheckResult>(
                cypher,
                parameters,
                record =>
                {
                    return new EntitlementCheckResult
                    {
                        Allowed = true,
                        Reason = string.Format(SystemMessages.EntitlementService.EntitlementGrantedTemplate,
                            record[SystemInternalConstants.Graph.PermissionProperty].As<string>(),
                            record[SystemInternalConstants.Graph.ResourceIdProperty].As<string>(),
                            record[SystemInternalConstants.Cypher.RoleTypeField].As<string>(),
                            record[SystemInternalConstants.Cypher.EntitlementNameField].As<string>()),
                        GrantedEntitlement = record[SystemInternalConstants.Cypher.EntitlementNameField].As<string>(),
                        Path = [
                            record[SystemInternalConstants.Cypher.IdentityIdField].As<string>(),
                            record[SystemInternalConstants.Cypher.RoleTypeField].As<string>(),
                            record[SystemInternalConstants.Cypher.EntitlementNameField].As<string>(),
                            record[SystemInternalConstants.Graph.ResourceIdProperty].As<string>()
                        ]
                    };
                });

            if (results.Count > 0)
            {
                return results[0];
            }

            return new EntitlementCheckResult
            {
                Allowed = false,
                Reason = string.Format(SystemMessages.EntitlementService.NoEntitlementFoundTemplate, permission, resourceId, subjectId),
                GrantedEntitlement = null,
                Path = []
            };
        }
        catch (Exception ex)
        {
            return new EntitlementCheckResult
            {
                Allowed = false,
                Reason = string.Format(SystemMessages.EntitlementService.ErrorCheckingEntitlementTemplate, ex.Message),
                GrantedEntitlement = null,
                Path = []
            };
        }
    }
}
