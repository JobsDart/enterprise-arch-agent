<#
.SYNOPSIS
    Provisions Azure OpenAI and deploys a gpt-4o model for the Enterprise Architecture Agent.
.PREREQUISITES
    Azure CLI (https://learn.microsoft.com/cli/azure/install-azure-cli); run `az login` first.
.EXAMPLE
    ./provision-azure.ps1 -ResourceGroup rg-jobsdart-ai -Location swedencentral -OpenAiName jobsdart-openai
#>
param(
    [Parameter(Mandatory = $true)] [string] $ResourceGroup,
    [string] $Location  = "swedencentral",
    [Parameter(Mandatory = $true)] [string] $OpenAiName,
    [string] $ChatModel = "gpt-4o"
)
$ErrorActionPreference = "Stop"

Write-Host "==> Resource group '$ResourceGroup'" -ForegroundColor Cyan
az group create -n $ResourceGroup -l $Location | Out-Null

Write-Host "==> Azure OpenAI '$OpenAiName'" -ForegroundColor Cyan
az cognitiveservices account create -n $OpenAiName -g $ResourceGroup -l $Location `
    --kind OpenAI --sku S0 --custom-domain $OpenAiName --yes | Out-Null

Write-Host "==> Deploying chat model '$ChatModel'" -ForegroundColor Cyan
az cognitiveservices account deployment create -g $ResourceGroup -n $OpenAiName `
    --deployment-name $ChatModel --model-name $ChatModel --model-format OpenAI `
    --sku-name Standard --sku-capacity 10 | Out-Null

$endpoint = az cognitiveservices account show -g $ResourceGroup -n $OpenAiName --query properties.endpoint -o tsv
$key      = az cognitiveservices account keys list -g $ResourceGroup -n $OpenAiName --query key1 -o tsv

Write-Host ""
Write-Host "================ DONE ================" -ForegroundColor Green
Write-Host "From src/EnterpriseArchAgent.Api run:" -ForegroundColor Green
Write-Host "  dotnet user-secrets set `"Ai:AzureOpenAI:Endpoint`" `"$endpoint`""
Write-Host "  dotnet user-secrets set `"Ai:AzureOpenAI:ApiKey`"   `"$key`""
