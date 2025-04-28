using System.Text;
using Infrastructure.Repository;
using Infrastructure.Service;
using Domain.Interfaces;
using Application.CommandHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Application.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System.Reflection.PortableExecutable;
using System.Configuration;

namespace API
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure controllers + Swagger/OpenAPI.
        /// </summary>
        public static IServiceCollection AddPresentationServices(
            this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TeachingPlatforme API",
                    Version = "v1"
                });

                opts.AddSecurityDefinition("Auth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Enter your JWT token her"
                });
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Auth"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Configure MediatR (application layer).
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));
            return services;
        }

        /// <summary>
        /// Configure EF Core with Cosmos DB + repositories + domain services.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // EF Core / Cosmos
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseCosmos(
                    configuration.GetConnectionString("CosmosDb")!,
                    databaseName: "TeachingPlatforme"));

            // Repositories & services
            services.AddHttpContextAccessor();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPdfToMarkdownService, PdfToMarkdownService>();

            return services;
        }

        /// <summary>
        /// Configure JWT authentication.
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            return services;
        }

        /// <summary>
        /// Configure HTTP logging.
        /// </summary>
        public static IServiceCollection AddHttpLoggingServices(
            this IServiceCollection services)
        {
            services.AddHttpLogging(opts =>
            {
                opts.LoggingFields = HttpLoggingFields.None;
                opts.RequestBodyLogLimit = 4096;
                opts.ResponseBodyLogLimit = 4096;
                opts.CombineLogs = true;
            });

            return services;
        }

        /// <summary>
        /// Configure CORS policy.
        /// </summary>
        public static IServiceCollection AddCorsServices(
            this IServiceCollection services)
        {
            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowOrigin", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }

        // public static IServiceCollection AddLearningWorkflowOrchestrator(
        //     this IServiceCollection services)
        // {
        //     services.AddSingleton<LearningWorkflowOrchestrator>();
        //     return services;
        // }

        public static IServiceCollection AddSemanticKernel(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<Kernel>(sp =>
            {
                Kernel kernel;

                var builder = Kernel.CreateBuilder();
                builder.AddAzureOpenAIChatCompletion("o3-mini", configuration["Model:Endpoint"]!, configuration["Model:ApiKey"]!);
                builder.Plugins.AddFromType<LearningWorkflowOrchestrator>();

                kernel = builder.Build();

                return kernel;
            });

            return services;
        }
    }
}
