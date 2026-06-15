namespace EnterpriseArchAgent.Core.Domain;

/// <summary>
/// The specialised roles in the architecture pipeline. The agents run in this order;
/// each one builds on the output of the previous (a sequential handoff pattern).
/// </summary>
public enum AgentRole
{
    /// <summary>Turns the brief into functional + non-functional requirements.</summary>
    Requirements = 0,
    /// <summary>Proposes bounded contexts, components and a C4 container diagram.</summary>
    Architecture = 1,
    /// <summary>Produces a security + GDPR threat assessment and checklist.</summary>
    Security = 2,
    /// <summary>Writes Architecture Decision Records for the key choices.</summary>
    Adr = 3,
    /// <summary>Estimates the monthly Azure running cost.</summary>
    Cost = 4
}
