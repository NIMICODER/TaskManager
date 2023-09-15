using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskManager_Data.Implementations;
using TaskManager_Data.Interfaces;
using TaskManager_Logging;
using TaskManager_Models.Context;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Services.Domains.Auth;
using TaskManager_Services.Domains.Email;
using TaskManager_Services.Domains.ServiceFactory;
using TaskManager_Services.Domains.Tasks;
using TaskManager_Services.Utility;

namespace TaskManager_API.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void RegisterAppServices(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork<TaskManagerDbContext>>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITaskService, TaskService>();
        }

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration? connectionString)
        {
            services.AddDbContext<TaskManagerDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(connectionString.GetConnectionString("sqlConnection"), s =>
                {
                    s.MigrationsAssembly("TaskManager_Migrations");
                    s.EnableRetryOnFailure(3);
                });
            });
        }

       


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TASKMANAGERAPI", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    },
                });
            });
        }

        public static void ConfigureCors(this IServiceCollection services) =>
             services.AddCors(options =>
             {
                 options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
             });

        public static void RegisterAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<TaskManagerDbContext>()
             .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = serverSecret,
                    ValidIssuer = jwtConfig.JwtIssuer,
                    ValidAudience = jwtConfig.JwtAudience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization();
        }
    }
}
