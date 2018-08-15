using JamesMann.BotFramework.Middleware;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoomBookingBot.Chatbot.Extensions
{
    public static class CardExtensions
    {

        public static void AddAdaptiveCardRoomConfirmationAttachment(this Activity activity, string room, string date, string time, string attendees)
        {
            activity.Attachments = new List<Attachment>() { CreateAdaptiveCardRoomConfirmationAttachment(room, date, time, attendees) };
        }

        public static void AddSignInCard(this Activity activity, string url)
        {
            activity.Attachments = new List<Attachment>() { CreateSignInCard(url) };
        }

        public static void AddAdaptiveCardChoiceForm(this Activity activity, (string text, object value)[] choices)
        {
            activity.Attachments = new List<Attachment> { CreateChoiceAdaptiveCardAttachment(choices) };
        }

        private static Attachment CreateChoiceAdaptiveCardAttachment((string text, object value)[] choices)
        {
            var choiceItems = new List<dynamic>(choices.Select(choice => new { title = choice.text, choice.value }));

            var serializedChoices = JsonConvert.SerializeObject(choiceItems.ToArray());

            var adaptiveCard = Resource.AdaptiveCardChoiceTemplate;
            adaptiveCard = adaptiveCard.Replace("$(choices)", serializedChoices);

            return new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard)
            };
        }


        private static Attachment CreateAdaptiveCardRoomConfirmationAttachment(string room, string date, string time, string attendees)
        {
            var adaptiveCard = Resource.AdaptiveCardRoomTemplate;
            adaptiveCard= adaptiveCard.Replace("$(room)", room);
            adaptiveCard= adaptiveCard.Replace("$(date)", date);
            adaptiveCard= adaptiveCard.Replace("$(time)", time);
            adaptiveCard= adaptiveCard.Replace("$(attendees)", attendees);
            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard)
            };
        }

        private static Attachment CreateSignInCard(string url)
        {
                return new SigninCard()
                {
                    Text = "Please sign in with your Office 365 account to continue",
                    Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.Signin,
                        Title = "Sign in",
                        Value = url
                    }
                }
                }.ToAttachment();
        }
    }
}
