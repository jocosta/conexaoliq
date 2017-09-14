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
    public class PremiosDialogService : DialogServiceBase, IDialogService
    {
        private readonly MensagemRepository _mensagemRepository;
        public PremiosDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();
        }
        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();

            await SendTyping();


            var premios = new PremioRepository().Listar();
            _activity.Type = ActivityTypes.Message;

            _activity.Text = _mensagemRepository.Pesquisar(TipoMensagem.PremiosConquistados);


            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            _activity.Attachments = new List<Attachment>();

            foreach (var premio in premios)
            {
                ThumbnailCard thumb = new ThumbnailCard()
                {
                    Title = premio.Titulo,
                    Text = premio.Texto,
                    Images = new List<CardImage>()
                        {
                            new CardImage() { Url =  $"{UrlApi}/{premio.Imagem.Caminho}" }
                        }
                };
                _activity.Attachments.Add(thumb.ToAttachment());
            }

            await PostAsync();

        }
    }
}