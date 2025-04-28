using API;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Ai;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices();

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddHttpLoggingServices();

builder.Services.AddCorsServices();

builder.Services.AddScoped<IQuizGenerationService, QuizGenerationService>();
builder.Services.AddScoped<IWeakTopicDetectionService, WeakTopicDetectionService>();
builder.Services.AddScoped<IRemediationService, RemediationService>();

builder.Services.AddScoped<QuizApplicationService>();
builder.Services.AddScoped<RemediationApplicationService>();

builder.Services.AddSingleton(sp =>
{
    var kb = Kernel.CreateBuilder();                                   // create the builder  :contentReference[oaicite:0]{index=0}
    kb.AddAzureOpenAIChatCompletion(
        deploymentName: "o3-mini",          // required
        endpoint:       "https://aigenstudio6832366256.openai.azure.com/",            // required
        apiKey:         "0d87619f35064fd9a6f6125d6c1bff57");                         // user-secrets / KeyVault
    return kb.Build();                                                 // singleton Kernel   :contentReference[oaicite:1]{index=1}
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHsts();

app.Run();
