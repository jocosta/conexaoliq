
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class EmpresaRepository
    {

        public Empresa Obter()
        {
            return JsonResources.Get<Empresa>("empresa");
        }

    }
}