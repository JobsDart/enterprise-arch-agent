namespace EnterpriseArchAgent.Core.Domain;

/// <summary>
/// The input to the pipeline: a plain-language description of the system to design,
/// plus any optional constraints (cloud, compliance, budget, team size…).
/// </summary>
/// <param name="Brief">e.g. "Design a real-time order management system for a B2B retailer."</param>
/// <param name="Constraints">Optional. e.g. "Must run on Azure, GDPR-compliant, team of 6."</param>
public sealed record ArchitectureRequest(string Brief, string? Constraints = null);
