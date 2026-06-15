# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY EnterpriseArchAgent.sln .
COPY src/EnterpriseArchAgent.Core/EnterpriseArchAgent.Core.csproj src/EnterpriseArchAgent.Core/
COPY src/EnterpriseArchAgent.Infrastructure/EnterpriseArchAgent.Infrastructure.csproj src/EnterpriseArchAgent.Infrastructure/
COPY src/EnterpriseArchAgent.Api/EnterpriseArchAgent.Api.csproj src/EnterpriseArchAgent.Api/
RUN dotnet restore src/EnterpriseArchAgent.Api/EnterpriseArchAgent.Api.csproj
COPY . .
RUN dotnet publish src/EnterpriseArchAgent.Api/EnterpriseArchAgent.Api.csproj -c Release -o /app /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EnterpriseArchAgent.Api.dll"]
