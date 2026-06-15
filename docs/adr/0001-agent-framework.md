# ADR-0001: Use the Microsoft Agent Framework, behind an abstraction

- **Status:** Accepted
- **Date:** 2026-06-14

## Context
The system is multi-agent. We needed an agent runtime. Options: the **Microsoft Agent Framework
(MAF)** (GA, unifies Semantic Kernel + AutoGen), **Semantic Kernel Agents**, or hand-rolled calls to
the model API.

## Decision
Use the **Microsoft Agent Framework** (`Microsoft.Agents.AI` + `Microsoft.Agents.AI.OpenAI`) to create
and run agents, but **only inside Infrastructure**, behind the Core `IAgentRunner` interface. Each
agent is `chatClient.AsAIAgent(instructions, name)` and runs via `agent.RunAsync(...)`.

## Rationale
- MAF is Microsoft's strategic, GA agent runtime — the right thing to demonstrate and the right
  long-term bet (handoffs, workflows, tools).
- Hiding it behind `IAgentRunner` means the orchestration logic and agent definitions (in Core) have
  zero dependency on MAF; we could switch runtimes by writing one class.

## Consequences
- ✅ Uses the current, flagship Microsoft agent stack.
- ✅ Core stays pure and unit-testable with a fake `IAgentRunner`.
- ⚠️ MAF is young; its API surface evolves. The abstraction limits the blast radius of changes to a
  single adapter class.
