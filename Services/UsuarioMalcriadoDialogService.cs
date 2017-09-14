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
    public class UsuarioMalcriadoDialogService : DialogServiceBase, IDialogService
    {

        private readonly MensagemRepository _mensagemRepository;
        public UsuarioMalcriadoDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {

            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();

            var gifs = new List<string>
            {
                "bot_crying.gif",
                "walle.gif"
            };

            Random random = new Random();

            var gif = gifs[random.Next(gifs.Count)];

            _activity.Attachments.Add(new Attachment()
            {
                ContentUrl = $"{UrlApi}/images/{gif}",
                ContentType = "image/gif",
                Name = "Fiquei Triste"
            });

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.RespostaUsuarioMalcriado));

        }
    }
}