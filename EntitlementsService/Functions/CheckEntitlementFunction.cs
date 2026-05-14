using AsbBianTechnicalTest.Constants;
using AsbBianTechnicalTest.Models;
using AsbBianTechnicalTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AsbBianTechnicalTest.Functions;

/// <summary>
/// Azure Function that handles entitlement checks against a Neo4j graph database.
/// </summary>
public class CheckEntitlementFunction
{
    private readonly IEntitlementService _entitlementService;
    private readonly ILogger<CheckEntitlementFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the CheckEntitlementFunction class.
    /// </summary>
    /// <param name="entitlementService">The entitlement service for checking permissions.</param>
    /// <param name="logger">The logger for tracing operations.</param>
    public CheckEntitlementFunction(IEntitlementService entitlementService, ILogger<CheckEntitlementFunction> logger)
    {
        _entitlementService = entitlementService;
        _logger = logger;
    }

    /// <summary>
    /// Checks if a subject has the specified permission on a resource.
    /// </summary>
    /// <param name="req">The HTTP request containing the entitlement check request.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// - 200 OK with <see cref="EntitlementCheckResult"/> if the request is valid
    /// - 400 Bad Request if required fields are missing
    /// - 500 Internal Server Error if an exception occurs
    /// </returns>
    /// <remarks>
    /// POST /api/entitlement/check
    /// Request body must contain:
    /// - subjectId (string): The identity/user to check
    /// - permission (string): The permission to verify (e.g., "read", "write")
    /// - resourceId (string): The resource being accessed
    /// </remarks>
    [Function(SystemInternalConstants.CheckEntitlement.FunctionName)]
    [OpenApiOperation(SystemInternalConstants.CheckEntitlement.OperationId)]
    [OpenApiRequestBody(contentType: SystemInternalConstants.Http.JsonContentType, bodyType: typeof(EntitlementCheckRequest), Description = SystemMessages.CheckEntitlement.RequestBodyDescription, Required = true)]
    [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.OK, contentType: SystemInternalConstants.Http.JsonContentType, bodyType: typeof(EntitlementCheckResult), Description = SystemMessages.CheckEntitlement.SuccessResponseDescription)]
    [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.BadRequest, contentType: SystemInternalConstants.Http.JsonContentType, bodyType: typeof(object), Description = SystemMessages.CheckEntitlement.MissingFieldsError)]
    [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.InternalServerError, contentType: SystemInternalConstants.Http.JsonContentType, bodyType: typeof(object), Description = SystemMessages.CheckEntitlement.InternalServerError)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, SystemInternalConstants.CheckEntitlement.HttpMethod, Route = SystemInternalConstants.CheckEntitlement.Route)] HttpRequest req)
    {
        _logger.LogInformation(SystemInternalConstants.CheckEntitlement.RequestReceivedLog);

        try
        {
            var requestBody = await JsonSerializer.DeserializeAsync<EntitlementCheckRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (requestBody == null || string.IsNullOrWhiteSpace(requestBody.SubjectId) ||
                string.IsNullOrWhiteSpace(requestBody.Permission) || string.IsNullOrWhiteSpace(requestBody.ResourceId))
            {
                return new BadRequestObjectResult(new { error = SystemMessages.CheckEntitlement.MissingFieldsError });
            }

            var result = await _entitlementService.CheckEntitlementAsync(
                requestBody.SubjectId,
                requestBody.Permission,
                requestBody.ResourceId);

            _logger.LogInformation(SystemInternalConstants.CheckEntitlement.ResultLogTemplate, result.Allowed);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(SystemInternalConstants.CheckEntitlement.ErrorLogTemplate, ex.Message);
            return new ObjectResult(new { error = SystemMessages.CheckEntitlement.InternalServerError }) { StatusCode = 500 };
        }
    }
}