using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Repositories;
using System.Threading.Tasks;
using CTX.Bot.ConexaoLiq.Helpers;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class VestimentaDialogService : DialogServiceBase, IDialogService
    {
        private const int ULT_DIA_EVENTO = 15;
        private readonly MensagemRepository _mensagemRepository;
        public VestimentaDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();


            DateTime datenow = DateTime.Now;
            var periodoDia = datenow.ObterPeriodoDia();

            if ((datenow.Day == ULT_DIA_EVENTO && periodoDia == SaudacaoPeriodoDia.BoaNoite) || datenow.Day > ULT_DIA_EVENTO)
            {
                _activity.Attachments.Add(new Attachment()
                {
                    ContentUrl = $"{UrlApi}/images/gifs/tpo.gif",
                    ContentType = "image/gif",
                    Name = "Liq"
                });

                await PostAsync();
            }
            else
            {
                var vestimentas = new VestimentaRepository().Listar().OrderBy(o => o.Chave).ToList(); ;
                _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                _activity.Attachments = new List<Attachment>();
                foreach (var vestimenta in vestimentas)
                {
                    await PostAsync($"{vestimenta.Texto}");
                    if (!string.IsNullOrEmpty(vestimenta.Imagem.Caminho))
                    {
                        System.Threading.Thread.Sleep(500);
                        await SendTyping();
                        HeroCard thumb = new HeroCard()
                        {
                            Images = new List<CardImage>()
                        {
                            new CardImage() { Url =  $"{UrlApi}{vestimenta.Imagem.Caminho}" }
                        }
                        };
                        _activity.Attachments.Add(thumb.ToAttachment());
                        await PostAsync();
                    }
                    await SendTyping();
                    System.Threading.Thread.Sleep(2000);
                }
            }
            


        }
    }
}