using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WowMythicScanner
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appSettings.json", optional: false)
                .AddJsonFile("appSettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton(configuration);
                    services.AddScoped<IApiTokenProvider, ApiAuthenticationClient>();
                    services.AddScoped<ApiReader>();
                    services.AddScoped<WowApiReader>();
                }).UseConsoleLifetime();

            var host = builder.Build();

            var reader = host.Services.GetService<WowApiReader>();

            var list = reader.GetAchievementCategoriesAsync().Result;

            foreach(var achievement in list)
            {
                Console.WriteLine(achievement.Name);
            }

            Console.ReadLine();
        }
    }
}
