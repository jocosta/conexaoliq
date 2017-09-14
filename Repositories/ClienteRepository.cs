using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class ClienteRepository
    {
        BrowserSession _browserSession = new BrowserSession();

        public ICollection<Cliente> Listar()
        {

            return JsonResources.Get<ICollection<Cliente>>("clientes");
        }
    }
}