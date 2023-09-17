using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using TaskManager_API.Extensions;
using TaskManager_API.Middlewares;
using TaskManager_Cache.Extensions;
using TaskManager_Models.Context;
using TaskManager_Services.Utility;

namespace TaskManager_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfiguration configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            TaskManagerApiConfig managerApiConfig = configuration.Get<TaskManagerApiConfig>()!;

            //TaskManagerApiConfig managerApiConfigB = builder.Services.BindConfigu

            services.AddRedisCache(managerApiConfig.Redis);

            services.AddSingleton(managerApiConfig);
            JwtConfig jwtConfig = managerApiConfig.JwtConfig;
            services.AddSingleton(jwtConfig);

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
           "/nlog.config"));

            builder.Services.AddHttpContextAccessor();

            builder.Services.RegisterDbContext(builder.Configuration);
            services.RegisterAuthentication(jwtConfig);
            // Add services to the container.
            services.RegisterAppServices();
            //add email configuration
            var emailConfig1 = configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
            builder.Services.AddSingleton(emailConfig1);

            builder.Services.ConfigureCors();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureSwagger();


            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TaskManagerDbContext context = serviceProvider.GetRequiredService<TaskManagerDbContext>();

            services.AddControllers(x =>
            {
                x.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ConfigureException(builder.Environment);

            app.UseHttpsRedirection();

            app.UseAuthorization();

           // app.UseStaticFiles();
            app.UseRouting();

            app.MapControllers();

            //context.Database.Migrate();
            //context.Database.EnsureCreated();

            app.Run();
        }
    }
}