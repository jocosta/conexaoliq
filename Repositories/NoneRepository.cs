using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using LuisBot.Ctx.Baas.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class NoneRepository
    {
        BrowserSession _browserSession = new BrowserSession();

        public NoneModel GetOne()
        {
            var nonelist =  JsonResources.Get<ICollection<NoneModel>>("none");

            Random random = new Random();

            NoneModel nonemessage = nonelist.ElementAt(random.Next(nonelist.Count()));

            nonemessage.Texto = nonemessage.Texto.IncluirEmojis();

            return nonemessage;
        }
    }
}