namespace EnterpriseArchAgent.Core.Domain;

/// <summary>
/// The definition of an agent: its role, display name, the section title it produces,
/// and the system instructions that give it its expertise and output format.
///
/// Keeping the instructions here (in Core) — rather than in the Infrastructure that
/// runs them — means the *behaviour* of the system is part of the domain, not an
/// implementation detail of whichever agent runtime is plugged in.
/// </summary>
/// <param name="Role">Pipeline role.</param>
/// <param name="Name">Human-readable agent name, e.g. "Requirements Analyst".</param>
/// <param name="Title">Title of the Markdown section this agent contributes.</param>
/// <param name="Instructions">System prompt defining the agent's expertise and output.</param>
public sealed record AgentSpec(
    AgentRole Role,
    string Name,
    string Title,
    string Instructions);
