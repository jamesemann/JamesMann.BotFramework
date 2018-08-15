using JamesMann.BotFramework.Middleware.Extensions;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamesMann.BotFramework.Middleware
{
    public class SentimentMiddleware : IMiddleware
    {
        public SentimentMiddleware(IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("SemanticsKey");
        }

        public string ApiKey { get; }

        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                context.Services.Add<string>(await context.Activity.Text.Sentiment(ApiKey));
            }
            
            await next();
        }

    }
}
