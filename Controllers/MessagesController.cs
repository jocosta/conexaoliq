﻿namespace CTX.Bot.ConexaoLiq
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
    using System.Net.Http.Headers;
    using System.IO;
    using CTX.Bot.ConexaoLiq.Storage.BlobStorageDemo;
    using CTX.Bot.ConexaoLiq.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.IdentityModel.Protocols;
    using System.Configuration;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static readonly bool IsSpellCorrectionEnabled = bool.Parse(WebConfigurationManager.AppSettings["IsSpellCorrectionEnabled"]);
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        internal static IDialog<PesquisaEventoForm> MakeRootDialog(string channel, string user)
        {
            return Chain.From(() => FormDialog.FromForm(PesquisaEventoForm.BuildForm));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //_logger.Info(Newtonsoft.Json.JsonConvert.SerializeObject(activity));


            //var url = HttpContext.Current.Server.MapPath("~/");
            if (activity.Type == ActivityTypes.Message)
            {                 
                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                var iniciouPesquisa = userData.GetProperty<bool>("IniciouPesquisa");
               
                //Decomentar somente em dev para executar pesquisa
                //await Conversation.SendAsync(activity, () => MakeRootDialog(activity.ChannelId, activity.From.Id));
                //var response1 = Request.CreateResponse(HttpStatusCode.OK);
                //return response1;

                try
                {
                    using (var context = new BotContext())
                    {
                        var userActivity = new UserActivity();
                        userActivity.Channel = activity.ChannelId;
                        userActivity.UserId = activity.From.Id;
                        userActivity.Activity = Newtonsoft.Json.JsonConvert.SerializeObject(activity);
                        userActivity.Data = DateTime.Now;
                        userActivity.Id = Guid.NewGuid();


                        context.Activities.Add(userActivity);

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, "Erro ao salvar iteração");
                }

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
                else if (activity.Text == "/cancelar-pesquisa-satisfacao")
                {
                    userData.SetProperty<bool>("IniciouPesquisa", false);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                }
                else if (activity.Text == "/iniciar-pesquisa-satisfacao" || iniciouPesquisa)
                {
                    userData.SetProperty<bool>("IniciouPesquisa", true);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                    await Conversation.SendAsync(activity, () => MakeRootDialog(activity.ChannelId, activity.From.Id));
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
                    else if (attachment.ContentType == "image/jpeg" || attachment.ContentType == "image/png")
                    {


                        await new ImageService().UploadImageAsync(attachment.ContentUrl);

                        var reply = message.CreateReply();
                        using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, reply))
                        {
                            reply.Text = $"Salvei a(s) sua(s) foto(s) aqui, mais tarde compartilho um album com você";

                            var botClient = scope.Resolve<IConnectorClient>();
                            await botClient.Conversations.ReplyToActivityAsync(reply);
                        }
                        return;
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