using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetChatBot
{
public class Startup
{

    public static IConfigurationRoot Configuration;
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

        services.AddSingleton<IConfigurationRoot>(Configuration);
        services.AddSingleton(Bootstrapper);

        Bootstrapper.Initialize(services, Configuration);

        ServiceProvider = services.BuildServiceProvider();
        
        Bootstrapper.Startup(ServiceProvider);
    }
}
}