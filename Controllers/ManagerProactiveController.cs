using CTX.Bot.ConexaoLiq.Models;
using CTX.Bot.ConexaoLiq.Services;
using Microsoft.Bot.Connector;
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
    public class ManagerProactiveController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
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

                    reply.Text = activity.Text;
                    reply.Attachments = activity.Attachments;
                    reply.AttachmentLayout = activity.AttachmentLayout;

                    MicrosoftAppCredentials.TrustServiceUrl(item.ServiceUrl);
                    var client = new ConnectorClient(new Uri(item.ServiceUrl), new MicrosoftAppCredentials(appId, appPassword));

                    var result = await client.Conversations.ReplyToActivityAsync(reply);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
