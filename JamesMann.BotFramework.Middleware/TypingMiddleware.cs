using Microsoft.Bot.Builder;
using JamesMann.BotFramework.Middleware.Extensions;
using System.Threading.Tasks;

namespace JamesMann.BotFramework.Middleware
{
    public class TypingMiddleware : IMiddleware
    {
        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            if (context.Activity.UserHasJustJoinedConversation() || context.Activity.UserHasJustSentMessage())
            {
                await context.Activity.DoWithTyping(async () =>
                {
                    await next();
                });
            }
            else
            {
                await next();
            }
        }
    }
}
