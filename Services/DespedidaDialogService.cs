using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class DespedidaDialogService : DialogServiceBase, IDialogService
    {
        private readonly DespedidaRepository _despedidaRepository;
        private readonly MensagemRepository _mensagemRepository;
        public DespedidaDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _despedidaRepository = new DespedidaRepository();
            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {

            await SendTyping();
            await base.Processar();
            await SendTyping();




            var informacao = _despedidaRepository.Obter();


            if (!string.IsNullOrEmpty(informacao.Imagem?.Caminho))
            {
                _activity.Attachments.Add(new Attachment()
                {
                    ContentUrl = $"{UrlApi}{informacao.Imagem.Caminho}",
                    ContentType = "image/gif",
                    Name = "Liq"
                });
            }

            await PostAsync(informacao.Texto);

        }
    }
}