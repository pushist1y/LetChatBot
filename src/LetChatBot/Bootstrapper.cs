using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading;
using LetChatBot.Data;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class Bootstrapper
    {
        public void Initialize(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ForumContext>((options) =>
            {
                options.UseMySql(config.GetConnectionString("Forum"));
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);

            services.AddSingleton<DatabaseChatPollerService>();
            services.AddSingleton<LetChatBot>();
            services.AddSingleton<TelegramAccessService>();
            services.AddTransient<TelegramToForumLinkService>();
            services.AddTransient<TelegramMessageProcessorService>();
            services.AddTransient<ForumLinkRepository>();
            services.AddTransient<MessagesRepository>();
            services.AddTransient<CommandProcessorService>();
            services.AddTransient<PiuRepository>();
            services.AddTransient<MessageProcessorService>();
            services.Configure<TelegramOptions>(config);
        }

        public void Startup(IServiceProvider serviceProvider)
        {
            var bot = serviceProvider.GetRequiredService<LetChatBot>();
            bot.Start().Wait();

            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}