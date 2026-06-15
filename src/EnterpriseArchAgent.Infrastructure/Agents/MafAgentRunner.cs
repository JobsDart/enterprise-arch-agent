using EnterpriseArchAgent.Core.Abstractions;
using EnterpriseArchAgent.Core.Domain;
using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace EnterpriseArchAgent.Infrastructure.Agents;

/// <summary>
/// Runs an agent using the <b>Microsoft Agent Framework</b>. For each spec it builds a
/// <see cref="AIAgent"/> from the shared Azure OpenAI chat client, configured with that
/// agent's instructions and name, then runs it against the supplied input.
/// </summary>
public sealed class MafAgentRunner : IAgentRunner
{
    private readonly ChatClient _chatClient;

    public MafAgentRunner(ChatClient chatClient) => _chatClient = chatClient;

    public async Task<string> RunAsync(AgentSpec spec, string input, CancellationToken ct = default)
    {
        // AsAIAgent comes from Microsoft.Agents.AI.OpenAI — it turns the Azure OpenAI
        // chat client into a first-class MAF agent with its own system instructions.
        AIAgent agent = _chatClient.AsAIAgent(instructions: spec.Instructions, name: spec.Name);

        // session/options are optional — each call runs the agent statelessly.
        AgentResponse response = await agent.RunAsync(input, cancellationToken: ct);
        return response.Text ?? string.Empty;
    }
}
