namespace EnterpriseArchAgent.Core.Domain;

/// <summary>One agent's contribution to the final architecture package.</summary>
/// <param name="Role">Which agent produced it.</param>
/// <param name="Title">Section title.</param>
/// <param name="Content">Markdown content (may contain Mermaid diagrams, tables, ADRs…).</param>
public sealed record AgentOutput(AgentRole Role, string Title, string Content);

/// <summary>
/// The complete result: every agent's section, plus a single assembled Markdown
/// document ready to drop into a repo / wiki / design review.
/// </summary>
/// <param name="Brief">The original brief.</param>
/// <param name="Sections">Per-agent outputs in pipeline order.</param>
/// <param name="CombinedMarkdown">All sections assembled into one document.</param>
/// <param name="GeneratedAtUtc">When it was produced.</param>
public sealed record ArchitecturePackage(
    string Brief,
    IReadOnlyList<AgentOutput> Sections,
    string CombinedMarkdown,
    DateTimeOffset GeneratedAtUtc);
