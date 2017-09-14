namespace CTX.Bot.ConexaoLiq
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Http;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Autofac;
    using System.Web;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using LUIS;
    using LuisBot;
    using Infra.Context;
    using Models;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static readonly bool IsSpellCorrectionEnabled = bool.Parse(WebConfigurationManager.AppSettings["IsSpellCorrectionEnabled"]);
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //_logger.Info(Newtonsoft.Json.JsonConvert.SerializeObject(activity));


            var url = HttpContext.Current.Server.MapPath("~/");
            if (activity.Type == ActivityTypes.Message)
            {
                //try
                //{
                //    using (var context = new BotContext())
                //    {
                //        var userActivity = new UserActivity();
                //        userActivity.Channel = activity.ChannelId;
                //        userActivity.UserId = activity.From.Id;
                //        userActivity.Activity = Newtonsoft.Json.JsonConvert.SerializeObject(activity);
                //        userActivity.Data = DateTime.Now;
                //        userActivity.Id = Guid.NewGuid();


                //        context.Activities.Add(userActivity);

                //        context.SaveChanges();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    _logger.Fatal(ex, "Erro ao salvar iteração");
                //}

                if (activity.Text == "/start")
                {
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    var reply = activity.CreateReply();
                    reply.Text = "Olá! pergunte algo para começarmos.";
                    using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                    {
                        var client = scope.Resolve<IConnectorClient>();
                        await client.Conversations.ReplyToActivityAsync(reply);
                    }

                }
                else
                {
                    Task.Factory.StartNew(() => ConversationStart(activity));

                }


            }
            else
            {
                this.HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private async void ConversationStart(Activity message)
        {
            var textToSpeech = false;
            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                try
                {
                    var attachment = message.Attachments[0];
                    if (attachment.ContentType == "audio/ogg" || attachment.ContentType == "audio/mp4")
                    {
                        message.Text = await AudioConvert.SpeechToText(attachment.ContentUrl);
                        textToSpeech = true;
                        message.Attachments.Clear();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, nameof(ConversationStart));
                }
            }

            var appId = WebConfigurationManager.AppSettings["LuisAppId"];
            var appKey = WebConfigurationManager.AppSettings["LuisAppKey"];

            var client = new LuisClient(appId, appKey);
            var luisResult = client.Get(message.Text);

            if (luisResult == null || luisResult.TopScoringIntent == null)
                await new NoneDialogService(message, luisResult, textToSpeech).Processar();
            else
            {
                var intents = luisResult.Intents.Where(c => c.Score >= Intencao.GetScoreMatch(c.Intent));
                _logger.Info(nameof(LuisClient) + " " + Newtonsoft.Json.JsonConvert.SerializeObject(luisResult));

                if (!intents.Any())
                    await new NoneDialogService(message, luisResult, textToSpeech).Processar();
                else
                {
                    var intent = intents.OrderByDescending(c => c.Score).FirstOrDefault();
                    await DialogServiceFactory.Create(message, luisResult, intent, textToSpeech).Processar();
                }
            }
        }
    }
}