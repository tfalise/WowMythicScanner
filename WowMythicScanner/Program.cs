using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var apiConfiguration = new ApiConfiguration
            {
                Locale = ApiLocale.fr_FR,
                Region = ApiRegion.Europe
            };

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton(configuration);
                    services.AddScoped<IApiTokenProvider, ApiAuthenticationClient>();
                    services.AddScoped<ApiReader>();
                    services.AddScoped<WowApiReader>();
                    services.AddSingleton(apiConfiguration);
                }).UseConsoleLifetime();

            var host = builder.Build();

            var reader = host.Services.GetService<WowApiReader>();

            var list = reader.GetConnectedRealmsList().Result;

            foreach(var data in list)
            {
                Console.WriteLine($@"{data.Id}: {string.Join(",", data.Realms.Select(r => r.Name))}");
            }

            Console.ReadLine();
        }
    }
}
