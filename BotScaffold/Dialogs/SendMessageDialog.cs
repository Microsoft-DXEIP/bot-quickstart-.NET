using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;

namespace BotScaffold
{

    /// <summary>
    /// A Dialog that takes the User's message and maps it to a below intent. 
    /// Our bot currently only understands how to send a
    /// </summary>
    [LuisModel("YOUR_LUIS_APP_ID", "YOUR_LUIS_APP_KEY")]
    [Serializable]
    public class SendMessageDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I didn't understand. I am a very simple bot. Try asking me to send a Text Message");
            context.Wait(MessageReceived);
        }

        [LuisIntent("builtin.intent.communication.send_text")]
        public async Task SendMessage(IDialogContext context, LuisResult result)
        {
            EntityRecommendation contactName;
            EntityRecommendation message;

            result.TryFindEntity("builtin.communication.contact_name", out contactName);
            result.TryFindEntity("builtin.communication.message", out message);

            // Write some logic here to send the text message
            if (contactName == null || message == null)
            {
                await None(context, result);
            }
            else
            {
                var reply = String.Format("Okay I'll send the message \"{0}\" to {1}", message.Entity, contactName.Entity);
                await context.PostAsync(reply);
                context.Wait(MessageReceived);
            }
        }
    }
}