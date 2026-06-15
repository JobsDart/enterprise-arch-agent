# Deployment Guide

## A. Local

```powershell
cd src/EnterpriseArchAgent.Api
dotnet user-secrets set "Ai:AzureOpenAI:Endpoint" "https://<resource>.openai.azure.com/"
dotnet user-secrets set "Ai:AzureOpenAI:ApiKey"   "<key>"
dotnet run
```
→ http://localhost:5090 (only a **gpt-4o** deployment is required — no embeddings, no database).

## B. Provision Azure OpenAI

Use [`scripts/provision-azure.ps1`](../scripts/provision-azure.ps1) (requires Azure CLI + `az login`):
```powershell
./scripts/provision-azure.ps1 -ResourceGroup rg-jobsdart-ai -Location swedencentral -OpenAiName jobsdart-openai
```
Or by hand:
```powershell
az group create -n rg-jobsdart-ai -l swedencentral
az cognitiveservices account create -n jobsdart-openai -g rg-jobsdart-ai -l swedencentral --kind OpenAI --sku S0 --custom-domain jobsdart-openai
az cognitiveservices account deployment create -g rg-jobsdart-ai -n jobsdart-openai `
  --deployment-name gpt-4o --model-name gpt-4o --model-format OpenAI --sku-name Standard --sku-capacity 10
az cognitiveservices account show -g rg-jobsdart-ai -n jobsdart-openai --query properties.endpoint -o tsv
az cognitiveservices account keys list -g rg-jobsdart-ai -n jobsdart-openai --query key1 -o tsv
```

## C. Docker

```powershell
docker build -t enterprise-arch-agent:latest .
docker run -p 8080:8080 `
  -e Ai__AzureOpenAI__Endpoint="https://<resource>.openai.azure.com/" `
  -e Ai__AzureOpenAI__ApiKey="<key>" `
  enterprise-arch-agent:latest
```
→ http://localhost:8080

| Target | Command |
|--------|---------|
| Azure Container Apps | `az containerapp up --source .` |
| AKS | push image to ACR + Helm (see the Kubernetes platform project) |

## Production checklist
- [ ] Secrets in **Azure Key Vault**
- [ ] **Application Insights** to trace each agent's latency/tokens (the App Insights resource created with the Foundry project already supports this)
- [ ] Rate limiting via **API Management** (agent runs are token-expensive)
- [ ] Persist generated packages (Azure Blob / Cosmos DB)
