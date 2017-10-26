using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
public class Bootstrapper
{
    public void Initialize(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddDbContext<ForumContext>((options) => {
            options.UseMySql(config.GetConnectionString("Forum"));
        }, ServiceLifetime.Transient);

        services.AddTransient<ForumContextFactory>();
        services.AddTransient<DatabaseChatPoller>();
        services.AddTransient<LetChatBot>();
    }

    public void Startup(IServiceProvider serviceProvider)
    {
        var bot = serviceProvider.GetRequiredService<LetChatBot>();
        bot.Start();
        
        while(true)
        {
            Console.WriteLine("Bot is working");
            Thread.Sleep(5000);
        }

        
    }
}
}