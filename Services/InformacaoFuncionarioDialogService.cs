using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{

    public class InformacaoFuncionarioDialogService : DialogServiceBase, IDialogService
    {

        private readonly CargosRepository _cargoRepository;
        private readonly MensagemRepository _mensagemRepository;
        public InformacaoFuncionarioDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _cargoRepository = new CargosRepository();
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();

            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            _activity.Attachments = new List<Attachment>();

            await SendTyping();

            _activity.Attachments?.Clear();

            if (!string.IsNullOrEmpty(ValorEntidade.NomePessoa))
            {
                var cargos = _cargoRepository.PesquisarPorNome(ValorEntidade.NomePessoa);

                if (cargos.Any())
                {

                    if (cargos.Count() == 1 && _textToSpeech)
                    {
                        var cargo = cargos.First();
                        await PostAsync($"{cargo.NomePessoa} {cargo.DescricaoCargo}");
                    }

                    await SendTyping();
                    await MensagemDadosCargoAsync(cargos);

                    return;
                }

            }

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoEntendiONomeDoFuncionario));
        }



        private async Task MensagemDadosCargoAsync(ICollection<Cargo> cargos)
        {
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