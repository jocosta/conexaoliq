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

                foreach (var vestimenta in vestimentas)
                {
                    await PostAsync($"{vestimenta.Texto}");
                    if (!string.IsNullOrEmpty(vestimenta.Imagem.Caminho))
                    {
                        _activity.Attachments.Add(new Attachment()
                        {
                            ContentUrl = $"{UrlApi}{vestimenta.Imagem.Caminho}",
                            ContentType = "image/gif",
                            Name = "Liq"
                        });
                        await PostAsync();
                    }
                    await SendTyping();

                }
            }
        }
    }
}