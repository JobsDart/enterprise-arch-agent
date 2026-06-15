using EnterpriseArchAgent.Core.Domain;

namespace EnterpriseArchAgent.Core.Abstractions;

/// <summary>
/// Executes a single agent: given its spec (instructions) and an input message,
/// return the agent's text response.
///
/// This is the seam between the domain (which knows *what* the agents are) and the
/// runtime (which knows *how* to run them). The shipped implementation uses the
/// Microsoft Agent Framework; it could be swapped for Semantic Kernel agents or a
/// raw model call without touching the orchestration logic.
/// </summary>
public interface IAgentRunner
{
    Task<string> RunAsync(AgentSpec spec, string input, CancellationToken ct = default);
}
