using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Models;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Services
{

    public class CargoDialogService : DialogServiceBase, IDialogService
    {

        private readonly CargosRepository _cargoRepository;
        private readonly MensagemRepository _mensagemRepository;
        public CargoDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
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

            if (!string.IsNullOrEmpty(ValorEntidade.Pais))
                await MensagemPresidentePaisAsync();

            _activity.Attachments?.Clear();
            var cargos = _cargoRepository.Listar();
            var totalCargos = cargos.Count();

            var termoPesquisa = string.Empty;
            if (!string.IsNullOrEmpty(ValorEntidade.Cargo))
                termoPesquisa = ValorEntidade.Cargo;

            if (!string.IsNullOrEmpty(ValorEntidade.Departamento))
                termoPesquisa = $"{termoPesquisa} {ValorEntidade.Departamento}";


            cargos = _cargoRepository.PesquisarPorCargo(termoPesquisa);

            if (!cargos.Any())
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.CargoNaoEncontradoNaEmpresa));
            else
            {

                if (cargos.Count() == totalCargos)
                    await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoEntendiCargoSolicitado));

                if (cargos.Count() == 1 && _textToSpeech)
                {
                    var cargo = cargos.First();
                    await PostAsync($"{cargo.NomePessoa} {cargo.DescricaoCargo}");
                }

                await SendTyping();
                await MensagemDadosCargoAsync(cargos);

            }

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

        private async Task MensagemPresidentePaisAsync()
        {
            if (ValorEntidade.Cargo.ToSearch() != "presidente")
            {
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.CargoPaisNaoEncontrado));
                return;
            }

            var presidente = new PresidenteRepository().Pesquisar(ValorEntidade.Pais);

            if (presidente != null)
            {



                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.PresidentePaisEncontrado));


                ThumbnailCard thumbnailCard = new ThumbnailCard(presidente.Nome, presidente.Descricao, null, new List<CardImage>()
                        {
                            new CardImage() { Url = presidente.Imagem.Caminho }
                        });

                _activity.Attachments.Add(thumbnailCard.ToAttachment());

                await PostAsync();

            }
            else
            {
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.PresidentePaisNaoEncontrado));
            }


            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.QueroQueConhecaPresidenteEmpresa));




        }
    }
}