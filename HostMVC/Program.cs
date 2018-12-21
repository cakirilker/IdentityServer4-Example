using HostMVC.Data;
using HostMVC.Extensions;
using HostMVC.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace HostMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .MigrateDbContext<PersistedGrantDbContext>((_, __) =>
                {

                })
                .MigrateDbContext<ApplicationDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                    var settings = services.GetService<IOptions<AppSettings>>();
                    new ApplicationDbContextSeed()
                    .SeedAsync(context, env, logger, settings)
                    .Wait();
                })
                .MigrateDbContext<ConfigurationDbContext>((context, services) =>
                {
                    var configuration = services.GetService<IConfiguration>();
                    new ConfigurationDbContextSeed()
                    .SeedAsync(context, configuration)
                    .Wait();
                })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            // This line causes problem, when enabled configuration in appsettings.json is not picked up
            //.UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseIISIntegration()
            .UseStartup<Startup>()
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var builtConfig = config.Build();
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddEnvironmentVariables();
                config.AddConfiguration(configBuilder.Build());
            })
            .UseSerilog((context, config) =>
            {
                config
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                    .MinimumLevel.Override("System", LogEventLevel.Verbose)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Verbose)
                    .Enrich.FromLogContext()
                    .WriteTo.File(@"identityserver4_log.txt")
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
            });
    }
}
