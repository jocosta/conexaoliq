using CTX.Bot.ConexaoLiq.Infra.Context;
using CTX.Bot.ConexaoLiq.Models;
using Microsoft.Bot.Connector;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class ProactiveService
    {
        private object activities;

        public IEnumerable<Activity> GetRecipients()
        {
            List<UserActivity> activities = new List<UserActivity>();
            List<string> sendedTo = new List<string>();

            using (var context = new BotContext())
            {
                activities = context.Activities.ToList();
            }

            foreach (var item in activities)
            {
                if (string.IsNullOrEmpty(item.UserId) || sendedTo.Contains(item.UserId)) continue;

                sendedTo.Add(item.UserId);
                yield return JsonConvert.DeserializeObject<Activity>(item.Activity);
            }
        }
    }
}