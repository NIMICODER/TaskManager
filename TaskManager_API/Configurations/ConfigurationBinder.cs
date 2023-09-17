using TaskManager_Services.Utility;

namespace TaskManager_API.Configurations
{
    public static class ConfigurationBinder
    {
        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            JwtConfig jwt = new JwtConfig();
            configuration.GetSection("Jwt").Bind(jwt);

            services.AddSingleton(jwt);

            TaskManagerApiConfig managerApiConfig = new TaskManagerApiConfig();
            configuration.GetSection("AppSetting").Bind(managerApiConfig);
            services.AddSingleton(managerApiConfig);

            return services;
        }

    }
}
