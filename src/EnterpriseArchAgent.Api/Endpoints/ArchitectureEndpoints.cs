using EnterpriseArchAgent.Core.Application;
using EnterpriseArchAgent.Core.Domain;

namespace EnterpriseArchAgent.Api.Endpoints;

/// <summary>Request body for generating an architecture package.</summary>
/// <param name="Brief">The system to design.</param>
/// <param name="Constraints">Optional constraints (cloud, compliance, budget…).</param>
public sealed record GenerateRequest(string Brief, string? Constraints);

public static class ArchitectureEndpoints
{
    public static void MapArchitectureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/architectures", async (
            GenerateRequest body, ArchitectureOrchestrator orchestrator, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(body.Brief))
                return Results.BadRequest("Brief is required.");

            var package = await orchestrator.GenerateAsync(
                new ArchitectureRequest(body.Brief, body.Constraints), ct);
            return Results.Ok(package);
        })
        .WithTags("Architecture")
        .WithName("GenerateArchitecture")
        .WithSummary("Run the multi-agent pipeline and return a full architecture package.");
    }
}
