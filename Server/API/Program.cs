using API;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Ai;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// 1. Présentation, Auth, CORS, Swagger
// ---------------------------------------------------------------------------
builder.Services.AddPresentationServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsServices();
builder.Services.AddHttpLoggingServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------------
// 2. Application & Infrastructure
// ---------------------------------------------------------------------------
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Services AI « simples »
builder.Services.AddScoped<IQuizGenerationService,     QuizGenerationService>();
builder.Services.AddScoped<IWeakTopicDetectionService, WeakTopicDetectionService>();
builder.Services.AddScoped<IRemediationService,        RemediationService>();

// Services orchestrateurs
builder.Services.AddScoped<QuizApplicationService>();
builder.Services.AddScoped<RemediationLoopService>();

// ---------------------------------------------------------------------------
// 3. Kernel + Logging
// ---------------------------------------------------------------------------
// LoggerFactory que l’on réutilise pour SK et ASP.NET :
var loggerFactory = LoggerFactory.Create(b =>
{
    b.AddConsole();
    b.AddDebug();
    b.SetMinimumLevel(LogLevel.Information);
});
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new LoggerFactoryAdapter(loggerFactory));

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Information);
});

;
// Ajoutez le Kernel au conteneur de services
builder.Services.AddSingleton<Kernel>(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

    var kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.Services.AddSingleton(loggerFactory);
    kernelBuilder.AddAzureOpenAIChatCompletion(
            deploymentName: builder.Configuration["DeploymentName"],
            endpoint: builder.Configuration["Endpoint"],
            apiKey: builder.Configuration["ApiKey"]
    );

    return kernelBuilder.Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// ---------------------------------------------------------------------------
// 4. MagenticOrchestration (multi-agents)
// ---------------------------------------------------------------------------
#pragma warning disable SKEXP0110
builder.Services.AddSingleton<MagenticOrchestration>(sp =>
#pragma warning restore SKEXP0110
{
    var kernel = sp.GetRequiredService<Kernel>();
    return AgentOrchestration.Build(kernel);       // crée IssueDetector, IssueTutor, Coach, Cuisine
});

// ---------------------------------------------------------------------------
// 5. Pipeline HTTP
// ---------------------------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHsts();

app.Run();

// ---------------------------------------------------------------------------
// 6. Implémentation d’adaptateur Logger pour ASP.NET (optionnel)
// ---------------------------------------------------------------------------
public sealed class LoggerFactoryAdapter : ILoggerProvider
{
    private readonly ILoggerFactory _factory;
    public LoggerFactoryAdapter(ILoggerFactory factory) => _factory = factory;
    public ILogger CreateLogger(string categoryName) => _factory.CreateLogger(categoryName);
    public void Dispose() { }
}
