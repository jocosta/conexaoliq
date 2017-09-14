using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Repositories;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class HelpDialogService : DialogServiceBase, IDialogService
    {

        private readonly MensagemRepository _mensagemRepository;
        public HelpDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();
            var texto = _mensagemRepository.Pesquisar(TipoMensagem.Help);

            var paragrafos = texto.Split('|');
            await SendTyping();
            await PostAsync(paragrafos[0]);
            await SendTyping();

            HeroCard card = new HeroCard();
            card.Text = paragrafos[1];
            _activity.Attachments.Add(card.ToAttachment());
            await PostAsync();
            if (paragrafos.Count() == 3)
            {
                await SendTyping();
                await PostAsync(paragrafos[2]);
            }



        }
    }
}