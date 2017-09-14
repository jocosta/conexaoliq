using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Helpers;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class ClienteDialogService : DialogServiceBase, IDialogService
    {
        private readonly MensagemRepository _mensagemRepository;
        public ClienteDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();
        }
        public async override Task Processar()
        {

            await SendTyping();
            await base.Processar();

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.EssesSaoNossosClientes));
            await SendTyping();

            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            _activity.Attachments = new List<Attachment>();

            var clientes = new ClienteRepository().Listar();
            var grupos = clientes.Select(c => c.GrupoCliente).Distinct();

            foreach (var grupo in grupos)
            {
                HeroCard heroCard = new HeroCard()
                {
                    Title = grupo.DisplayName
                };


                var clientesImagens = clientes.Where(c => c.GrupoCliente == grupo)
                                               .OrderBy(c => Guid.NewGuid())
                                               .Take(3);

                foreach (var cliente in clientesImagens)
                    heroCard.Images.Add(new CardImage() { Url = $"{UrlApi}/{cliente.Imagem.Caminho}" });

                _activity.Attachments.Add(heroCard.ToAttachment());
            }

            await PostAsync();

        }
    }
}