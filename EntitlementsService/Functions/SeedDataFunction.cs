using AsbBianTechnicalTest.Constants;
using AsbBianTechnicalTest.Data;
using AsbBianTechnicalTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace AsbBianTechnicalTest.Functions;

/// <summary>
/// Azure Function that handles seeding graph data for demo purposes.
/// </summary>
public class SeedDataFunction
{
    private readonly INeo4jService _neo4jService;
    private readonly ILogger<SeedDataFunction> _logger;

    /// <summary>
    /// Initializes a new instance of the SeedDataFunction class.
    /// </summary>
    /// <param name="neo4jService">The Neo4j service for database operations.</param>
    /// <param name="logger">The logger for tracing operations.</param>
    public SeedDataFunction(INeo4jService neo4jService, ILogger<SeedDataFunction> logger)
    {
        _neo4jService = neo4jService;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the graph database with demo data.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// - 200 OK with success message if seeding succeeds
    /// - 500 Internal Server Error if an exception occurs
    /// </returns>
    /// <remarks>
    /// POST /api/entitlement/seed
    /// Clears existing graph data and populates it with demo data.
    /// </remarks>
    [Function(SystemInternalConstants.SeedData.FunctionName)]
    [OpenApiOperation(SystemInternalConstants.SeedData.OperationId)]
    [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.OK, contentType: SystemInternalConstants.Http.JsonContentType, bodyType: typeof(object))]
    [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.InternalServerError, SystemInternalConstants.Http.JsonContentType, typeof(object), Description = SystemMessages.SeedData.ErrorResponseDescription)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, SystemInternalConstants.SeedData.HttpMethod, Route = SystemInternalConstants.SeedData.Route)] HttpRequest req)
    {
        _logger.LogInformation(SystemInternalConstants.SeedData.RequestReceivedLog);

        try
        {
            // Clear existing data
            await SeedData.ClearGraphAsync(_neo4jService);
            _logger.LogInformation(SystemInternalConstants.SeedData.DataClearedLog);

            // Populate with demo data
            await SeedData.PopulateGraphAsync(_neo4jService);
            _logger.LogInformation(SystemInternalConstants.SeedData.DataPopulatedLog);

            return new OkObjectResult(new
            {
                success = true,
                message = SystemMessages.SeedData.SuccessMessage,
                nodesCreated = SystemInternalConstants.SeedData.NodesCreated
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(SystemInternalConstants.SeedData.ErrorLogTemplate, ex.Message);
            return new ObjectResult(new { error = string.Format(SystemMessages.SeedData.ErrorMessageTemplate, ex.Message) }) { StatusCode = 500 };
        }
    }
}