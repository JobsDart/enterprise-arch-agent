# Debugging Guide

### `InvalidOperationException: Missing required configuration 'Ai:AzureOpenAI:Endpoint'`
Set the endpoint + key:
```powershell
cd src/EnterpriseArchAgent.Api
dotnet user-secrets set "Ai:AzureOpenAI:Endpoint" "https://<resource>.openai.azure.com/"
dotnet user-secrets set "Ai:AzureOpenAI:ApiKey"   "<key>"
```

### HTTP 400 `api-version query parameter is not allowed when using /v1 path`
Your endpoint includes the Foundry `/openai/v1` path. The app strips paths automatically, but if you
hand-edit it, use only the base host: `https://<resource>.openai.azure.com/`.

### `DeploymentNotFound` (404)
`Ai:AzureOpenAI:ChatDeployment` (default `gpt-4o`) must match a real deployment:
```powershell
az cognitiveservices account deployment list -g <rg> -n <account> -o table
```

### `429 Too Many Requests`
The pipeline makes **5 sequential model calls** per run. On a low free-trial quota this can throttle.
Raise the deployment's tokens-per-minute in Azure AI Foundry, or wait and retry.

### A run takes 20–40 seconds
Expected — five GPT-4o calls run in sequence, each building on the last. The UI shows progress. To
speed it up you could run independent agents (e.g. Security + Cost) concurrently in the orchestrator.

### The Mermaid diagram doesn't render in the UI
The browser UI shows raw Markdown/Mermaid as text by design (no external JS). Paste the downloaded
`.md` into GitHub or any Mermaid-aware viewer and the C4 diagram renders.

### A section is empty or malformed
Individual agents occasionally return less than asked. Re-run; or set logging to `Debug` in
`appsettings.Development.json` to inspect each agent's raw output.
