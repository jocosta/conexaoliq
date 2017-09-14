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
    public class InformacaoBotDialogService : DialogServiceBase, IDialogService
    {
        private readonly InformacaoBotRepository _informacaoBotRepository;
        private readonly MensagemRepository _mensagemRepository;
        public InformacaoBotDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _informacaoBotRepository = new InformacaoBotRepository();
            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();

            var informacao = _informacaoBotRepository.Obter(ValorEntidade.InformacaoBot);
            if (informacao == null)
            {
                _activity.Attachments.Add(new Attachment()
                {
                    ContentUrl = $"{UrlApi}/images/bot.gif",
                    ContentType = "image/gif",
                    Name = "Liq"
                });
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.InformacaoBotPadrao));
            }
            else
            {
                await PostAsync(informacao.Texto);

                if (informacao.ExibirImagemBot)
                    await ImagemBotAsync(informacao);

                if (informacao.ExibirCriadores)
                    await MensagemDadosCargoAsync(informacao.Criadores);

            }
        }

        private async Task ImagemBotAsync(InformacaoBot informacao)
        {


            _activity.Attachments.Add(new Attachment()
            {
                ContentUrl = $"{UrlApi}/images/{informacao.ImagemBot}",
                ContentType = "image/gif",
                Name = "Liq"
            });


            await PostAsync();
        }

        private async Task MensagemDadosCargoAsync(ICollection<Cargo> cargos)
        {
            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (var cargo in cargos)
            {

                ThumbnailCard thumbnailCard = new ThumbnailCard(cargo.NomePessoa, cargo.DescricaoCargo, null, new List<CardImage>()
                        {
                             new CardImage() { Url = $"{UrlApi}/{cargo.Imagem.Caminho}" }
                        });


                _activity.Attachments.Add(thumbnailCard.ToAttachment());
            }

            await PostAsync();
        }
    }
}