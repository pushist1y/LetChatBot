using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LetChatBot
{
    public class Startup
    {

        public static IConfiguration Configuration;
        public static Bootstrapper Bootstrapper;
        public static IServiceProvider ServiceProvider;
        public void Start()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.unversioned.json");

            Configuration = builder.Build();

            var services = new ServiceCollection();
            Bootstrapper = new Bootstrapper();

            services.AddSingleton(Configuration);
            services.AddSingleton(Bootstrapper);
            services.AddLogging(options => { options.AddConfiguration(Configuration.GetSection("Logging")).AddConsole(); });

            Bootstrapper.Initialize(services, Configuration);

            ServiceProvider = services.BuildServiceProvider();


            Bootstrapper.Startup(ServiceProvider);
        }
    }
}