# Live Demo — get a public URL recruiters can click

## Option 1 (recommended): Azure Container Apps — uses your free $200 credit
Scales to zero when idle (≈no cost when not in use).

```powershell
az login
# from the repo root:
./scripts/deploy-containerapp.ps1 `
  -OpenAiEndpoint "https://<your-resource>.openai.azure.com/" `
  -OpenAiKey "<your-key>"
```
Prints `https://arch-agent.<region>.azurecontainerapps.io`. Share that URL.
Tear down: `az group delete -n rg-jobsdart-ai --yes --no-wait`.

## Option 2: GitHub Codespaces (free)
Repo → **Code ▸ Codespaces**. Then:
```bash
cd src/EnterpriseArchAgent.Api
dotnet user-secrets set "Ai:AzureOpenAI:Endpoint" "https://<res>.openai.azure.com/"
dotnet user-secrets set "Ai:AzureOpenAI:ApiKey" "<key>"
dotnet run
```
Make the forwarded port (5090) **Public** → shareable URL.

## Option 3: Railway / Render (free tier)
Connect the repo (builds the Dockerfile). Set env vars `Ai__AzureOpenAI__Endpoint`,
`Ai__AzureOpenAI__ApiKey`. Deploy → public URL.

> Note: each pipeline run makes 5 sequential GPT-4o calls (~20–40s) and uses tokens — keep the demo
> input short to stay well within the free credit.
