using ChatBot.Data.AppDbContext;
using ChatBot.Data.Mapping;
using ChatBot.Services.IServices.IChatBotServices;
using ChatBot.Services.IServices.IModel;
using ChatBot.Services.IServices.IUserServices;
using ChatBot.Services.Services.ChatBotServices;
using ChatBot.Services.Services.Model;
using ChatBot.Services.Services.UserServices;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ChatBot.APIs.ExtensionsServices
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your services here

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));


            //Services

           
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IModelProvider, HuggingFaceProvider>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular app origin
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();

                });
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatBot API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please provide a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            services.AddAutoMapper(config =>
            {
                // Add your profiles here
                config.AddProfile<MappingProfile>();



            });
            services.AddHttpContextAccessor();

            services.AddSwaggerGen();
            return services;
        }
    }
}
