using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using AsbBianTechnicalTest.Configuration;
using AsbBianTechnicalTest.Constants;
using AsbBianTechnicalTest.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Add Swagger/OpenAPI services
        services.AddEndpointsApiExplorer();
        services.AddScoped<IOpenApiConfigurationOptions>(_ => new OpenApiConfigurationOptions
        {
            Info = new OpenApiInfo
            {
                Title = SystemMessages.Api.Title,
                Version = SystemMessages.Api.Version,
                Description = SystemMessages.Api.Description
            }
        });


        // Neo4j configuration from environment variables
        var neo4jConfig = new Neo4jConfig
        {
            Uri = Environment.GetEnvironmentVariable(SystemInternalConstants.Neo4jConfig.UriVariable) ?? SystemInternalConstants.Neo4jConfig.DefaultUri,
            Username = Environment.GetEnvironmentVariable(SystemInternalConstants.Neo4jConfig.UsernameVariable) ?? SystemInternalConstants.Neo4jConfig.DefaultUsername,
            Password = Environment.GetEnvironmentVariable(SystemInternalConstants.Neo4jConfig.PasswordVariable) ?? SystemInternalConstants.Neo4jConfig.DefaultPassword,
            Database = Environment.GetEnvironmentVariable(SystemInternalConstants.Neo4jConfig.DatabaseVariable) ?? SystemInternalConstants.Neo4jConfig.DefaultDatabase
        };

        services.AddSingleton(neo4jConfig);
        services.AddSingleton<INeo4jService, Neo4jService>();
        services.AddScoped<IEntitlementService, EntitlementService>();
    })
    .Build();

host.Run();