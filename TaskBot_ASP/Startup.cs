using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TaskBot_ASP.Services;

namespace TaskBot_ASP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private BotConfiguration BotConfig { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BotConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConfigureWebhook>();

            services.AddHttpClient("TelegramWebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient
                => new TelegramBotClient(BotConfig.BotToken, httpClient));

            services.AddScoped<HandleUpdateService>();

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = BotConfig.BotToken;
                endpoints.MapControllerRoute(
                    name: "TelegramWebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", 
                    action = "Post"});
            });
        }
    }
}
