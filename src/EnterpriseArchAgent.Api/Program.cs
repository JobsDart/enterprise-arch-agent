using System.Text.Json.Serialization;
using EnterpriseArchAgent.Api.Endpoints;
using EnterpriseArchAgent.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddArchitectureAgent(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Serialize enums (AgentRole) as readable strings in JSON responses.
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.MapOpenApi();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapArchitectureEndpoints();

app.Run();
