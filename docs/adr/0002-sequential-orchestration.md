# ADR-0002: Sequential handoff orchestration (not parallel, not a single prompt)

- **Status:** Accepted
- **Date:** 2026-06-14

## Context
Five agents must produce one coherent architecture package. Options:
1. **One big prompt** asking a single model call for everything.
2. **Parallel agents**, each given only the brief.
3. **Sequential handoff**, each agent given the brief + all prior outputs.

## Decision
Use **sequential handoff**: the orchestrator accumulates a running context and passes it to each agent
in turn (Requirements → Architecture → Security → ADRs → Cost).

## Rationale
- The later agents depend on earlier output: Security must assess the *proposed* architecture, ADRs
  document *those* decisions, Cost prices *those* services. Parallel agents can't do this.
- A single prompt produces shallow, unstructured output and is hard to evolve; discrete agents give
  focused, individually-tunable sections and a clean section model.

## Consequences
- ✅ Coherent, cross-referential output.
- ✅ Each agent is independently editable (one `AgentSpec`).
- ⚠️ Latency is the sum of five calls (~20–40s). Independent agents (e.g. Security + Cost) could be
  parallelised later without changing the public contract.
