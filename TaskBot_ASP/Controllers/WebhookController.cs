using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskBot_ASP.Services;
using Telegram.Bot.Types;

namespace TaskBot_ASP.Controllers
{
    public class WebhookController : Controller
    {
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService, 
            [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
