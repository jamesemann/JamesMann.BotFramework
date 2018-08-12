using Microsoft.Bot.Builder;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using JamesMann.BotFramework.Middleware.Extensions;

namespace JamesMann.BotFramework.Middleware
{
    public class SpellCheckMiddleware :IMiddleware
    {
        public SpellCheckMiddleware(IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("SpellCheckKey");
        }

        public string ApiKey { get; }

        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            context.Activity.Text = await context.Activity.Text.SpellCheck(ApiKey);

            await next();
        }
    }
}
