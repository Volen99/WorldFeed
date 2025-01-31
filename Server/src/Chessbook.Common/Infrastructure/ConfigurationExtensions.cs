﻿namespace Chessbook.Common.Infrastructure
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection");
        }

        // by me
        public static string GetLocalConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionString");
        }
    }
}
