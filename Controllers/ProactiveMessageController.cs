using CTX.Bot.ConexaoLiq.Infra.Context;
using CTX.Bot.ConexaoLiq.Models;
using CTX.Bot.ConexaoLiq.Services;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CTX.Bot.ConexaoLiq.Controllers
{
    public class ProactiveMessageController : ApiController
    {


        public async Task<IHttpActionResult> Get()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.StartNew(async () =>
            {
                List<UserActivity> activities = new List<UserActivity>();

                var appId = ConfigurationManager.AppSettings["MicrosoftAppId"];
                var appPassword = ConfigurationManager.AppSettings["MicrosoftAppPassword"];

                var service = new ProactiveService();
                foreach (var item in service.GetRecipients())
                {
                    var botAccount = new ChannelAccount(item.Recipient.Id, item.Recipient.Name);

                    var reply = item.CreateReply();
                    reply.From = botAccount;
                    ThumbnailCard thumbnailCard = new ThumbnailCard("Pesquisa de Satisfação", "", $"Gostariamos de saber sua opnião sobre a conexão. Por favor, reserve um tempinho para responder nossa pesquisa de satisfação. Ah! Quem já respondeu não precisa fazer novamente 😊😊",
                        null,
                        new List<CardAction> {
                            new CardAction { Title = "Iniciar Pesquisa", Type = "imBack", Value = "/iniciar-pesquisa-satisfacao" }
                        });
                    reply.Attachments.Add(thumbnailCard.ToAttachment());

                    MicrosoftAppCredentials.TrustServiceUrl(item.ServiceUrl);
                    var client = new ConnectorClient(new Uri(item.ServiceUrl), new MicrosoftAppCredentials(appId, appPassword));

                    var result = await client.Conversations.ReplyToActivityAsync(reply);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return Ok();
        }


        public async Task<IHttpActionResult> Post([FromBody]Message message)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.StartNew(async () =>
            {
                List<UserActivity> activities = new List<UserActivity>();

                var appId = ConfigurationManager.AppSettings["MicrosoftAppId"];
                var appPassword = ConfigurationManager.AppSettings["MicrosoftAppPassword"];

                var service = new ProactiveService();
                foreach (var item in service.GetRecipients())
                {
                    var botAccount = new ChannelAccount(item.Recipient.Id, item.Recipient.Name);

                    var reply = item.CreateReply();
                    reply.From = botAccount;
                    ThumbnailCard thumbnailCard = new ThumbnailCard($"{message.descricao}  {message.inicio} às {message.fim}", message.responsavel, null);
                    reply.Attachments.Add(thumbnailCard.ToAttachment());

                    MicrosoftAppCredentials.TrustServiceUrl(item.ServiceUrl);
                    var client = new ConnectorClient(new Uri(item.ServiceUrl), new MicrosoftAppCredentials(appId, appPassword));

                    var result = await client.Conversations.ReplyToActivityAsync(reply);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return Ok();
        }
    }


    public class Message
    {
        public string inicio { get; set; }
        public string fim { get; set; }
        public string descricao { get; set; }
        public string responsavel { get; set; }
    }
}
