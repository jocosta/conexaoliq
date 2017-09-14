namespace CTX.Bot.ConexaoLiq.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using LUIS;
    using Repositories;
    using System.Web.Configuration;
    using Helpers;
    using Microsoft.Bot.Builder.Location;
    using Microsoft.Bot.Builder.Location.Bing;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Services;

    [LuisModel("a3a21743-2971-4d4f-b553-b8990c53d6f6", "233809556e3a4ebb91911099b1870530")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {


            string message = $"Desculpe, mais eu não consegui entender. Digite <help> se você precisa de ajudar.";
            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        //[LuisIntent(Intencao.Cargos)]
        //public async Task Cargos(IDialogContext context, LuisResult result)
        //    => await new CargoDialogService(context, result, MessageReceived).Processar();

        //[LuisIntent(Intencao.informacoes_institucionais_clientes)]
        //public async Task Clientes(IDialogContext context, LuisResult result)
        //        => await new ClienteDialogService(context, result, MessageReceived).Processar();

        //[LuisIntent(Intencao.informacoes_institucionais_faturamento)]
        //public async Task Faturamento(IDialogContext context, LuisResult result)
        //       => await new FaturamentoDialogService(context, result, MessageReceived).Processar();

        //[LuisIntent(Intencao.informacoes_institucionais_premios)]
        //public async Task Premios(IDialogContext context, LuisResult result)
        //    => await new PremiosDialogService(context, result, MessageReceived).Processar();


        //[LuisIntent(Intencao.informacoes_intitucionais_sites)]
        //public async Task Sites(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        //      => await new SiteDialogService(context, result, MessageReceived).Processar();


        //[LuisIntent(Intencao.numeros_funcionarios)]
        //public async Task Funcionarios(IDialogContext context, LuisResult result)
        //      => await new FuncionarioDialogService(context, result, MessageReceived).Processar();

        //[LuisIntent(Intencao.informacoes_institucionais)]
        //public async Task Intitucionais(IDialogContext context, LuisResult result)
        //   => await new InstitucionalDialogService(context, result, MessageReceived).Processar();


        [LuisIntent(Intencao.Help)]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Tente me perguntar coisa como: Onde ficam os sites da empresa? Qual o faturamento da empresa?");

            context.Wait(this.MessageReceived);
        }

        protected override LuisServiceResult BestResultFrom(IEnumerable<LuisServiceResult> results)
        {
            LuisServiceResult nonNoneWinner = results.Where(i => string.IsNullOrEmpty(i.BestIntent.Intent) == false && i.BestIntent.Score > Intencao.GetScoreMatch(i.BestIntent.Intent)).MaxBy(i => i.BestIntent.Score ?? 0d);

            if (nonNoneWinner == null)
            {
                nonNoneWinner = results.MaxBy(i => i.BestIntent.Score ?? 0d);
                nonNoneWinner.BestIntent.Intent = "None";
            }

            return nonNoneWinner;

        }

    }
}
