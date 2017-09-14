using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Helpers
{
    public class DialogHelper
    {
        public async static Task TypingAsync(IDialogContext context, IMessageActivity message)
        {

            message.Type = ActivityTypes.Typing;
            await context.PostAsync(message);
            message.Type = ActivityTypes.Message;
        }

        public async static Task PostAsync(IDialogContext context, IMessageActivity message, string text)
        {
            message.Type = ActivityTypes.Message;
            message.Text = text;
            await context.PostAsync(message);
            message.Text = null;
        }
    }
}