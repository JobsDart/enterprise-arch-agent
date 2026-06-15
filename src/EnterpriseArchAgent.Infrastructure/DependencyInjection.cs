using Azure;
using Azure.AI.OpenAI;
using EnterpriseArchAgent.Core.Abstractions;
using EnterpriseArchAgent.Core.Application;
using EnterpriseArchAgent.Infrastructure.Agents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;

namespace EnterpriseArchAgent.Infrastructure;

/// <summary>
/// Composition root. Wires the Azure OpenAI chat client, the Agent Framework runner,
/// and the orchestrator from configuration.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddArchitectureAgent(
        this IServiceCollection services, IConfiguration config)
    {
        var endpoint = NormalizeEndpoint(Required(config, "Ai:AzureOpenAI:Endpoint"));
        var apiKey = Required(config, "Ai:AzureOpenAI:ApiKey");
        var deployment = config["Ai:AzureOpenAI:ChatDeployment"] ?? "gpt-4o";

        var azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient(deployment);

        services.AddSingleton(chatClient);
        services.AddSingleton<IAgentRunner, MafAgentRunner>();
        services.AddSingleton<ArchitectureOrchestrator>();

        return services;
    }

    /// <summary>Reduce any Azure OpenAI / Foundry endpoint to its base "scheme://host/" form.</summary>
    private static string NormalizeEndpoint(string raw)
    {
        var uri = new Uri(raw.Trim(), UriKind.Absolute);
        return $"{uri.Scheme}://{uri.Authority}/";
    }

    private static string Required(IConfiguration config, string key)
        => config[key] is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException(
                $"Missing required configuration '{key}'. Set it via user-secrets or environment variables.");
}
