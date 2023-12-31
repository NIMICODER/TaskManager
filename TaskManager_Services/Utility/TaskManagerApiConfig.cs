﻿using TaskManager_Cache.Configuration;

namespace TaskManager_Services.Utility
{
    public class TaskManagerApiConfig
    {
        public string ConnectionString { get; set; } = null!;
        public JwtConfig JwtConfig { get; set; } = null!;
        public EmailConfig EmailConfig { get; set; } = null!;
        public RedisConfig Redis { get; set; } = null!;
    }
}
