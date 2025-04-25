using API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices();

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddHttpLoggingServices();

builder.Services.AddCorsServices();

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
