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
        private BotConfiguration _botConfig { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _botConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConfigureWebhook>();

            services.AddHttpClient("TelegramWebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient
                => new TelegramBotClient(_botConfig.BotToken, httpClient));

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
                var token = _botConfig.BotToken;
                endpoints.MapControllerRoute(
                    name: "TelegramWebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", 
                    action = "Post"});
            });
        }
    }
}
