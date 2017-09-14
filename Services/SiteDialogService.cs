using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Web.Configuration;
using CTX.Bot.ConexaoLiq.Helpers;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.LUIS;
using Microsoft.Bot.Builder.Location.Bing;
using Microsoft.Bot.Builder.Location;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class SiteDialogService : DialogServiceBase, IDialogService
    {

        private readonly string ApiKey;
        private readonly LocationRepository _locationRepository;
        private readonly MensagemRepository _mensagemRepository;
        public SiteDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            ApiKey = WebConfigurationManager.AppSettings["BingMapsApiKey"];
            _locationRepository = new LocationRepository();
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();

            var resourceManager = new LocationResourceManager();
            var cardBuilder = new LocationCardBuilder(ApiKey, resourceManager);
            await SendTyping();

            var foundLocations = _locationRepository.Listar();
            var locations = foundLocations.Select(x => x.Value).ToList();

            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            if (!string.IsNullOrEmpty(ValorEntidade.Site))
            {
                await PostSitePorNomeAsync(cardBuilder, foundLocations);
                return;
            }

            if (!string.IsNullOrEmpty(ValorEntidade.Cidade))
            {
                await PostSitePorCidadeAsync(cardBuilder, foundLocations);
                return;
            }

            if (!string.IsNullOrEmpty(ValorEntidade.Estado))
            {
                await PostSitePorEstadoAsync(cardBuilder, foundLocations);
                return;
            }
            
            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.MensagemSobreSites));
            _activity.Attachments = cardBuilder.CreateHeroCards(locations).Select(C => C.ToAttachment()).ToList();

            await PostAsync();


        }

        private async Task PostSitePorEstadoAsync(LocationCardBuilder cardBuilder, Dictionary<string, Location> foundLocations)
        {

            var cidadesSite = foundLocations.Select(c => c.Value).Where(x => x.Address.AdminDistrict.ToSearch().Like(ValorEntidade.Estado.ToSearch())).ToList();

            if (cidadesSite != null && cidadesSite.Any())
            {
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.TemosSiteNoEstado, ValorEntidade.Estado));
                _activity.Attachments = cardBuilder.CreateHeroCards(cidadesSite).Select(C => C.ToAttachment()).ToList();
                await PostAsync();
                return;
            }

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoTemosSiteNaEm, ValorEntidade.Estado));
        }

        private async Task PostSitePorCidadeAsync(LocationCardBuilder cardBuilder, Dictionary<string, Location> foundLocations)
        {
            var cidadesSite = foundLocations.Select(c => c.Value).Where(x => x.Address.Locality.ToSearch().Like(ValorEntidade.Cidade.ToSearch())).ToList();

            if (cidadesSite != null && cidadesSite.Any())
            {
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.TemosSiteNaCidade, ValorEntidade.Cidade));
                _activity.Attachments = cardBuilder.CreateHeroCards(cidadesSite).Select(C => C.ToAttachment()).ToList();
                await PostAsync();
                return;
            }

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoTemosSiteNaEm, ValorEntidade.Cidade));
        }

        private async Task PostSitePorNomeAsync(LocationCardBuilder cardBuilder, Dictionary<string, Location> foundLocations)
        {
            Location sites = null;
            var item = foundLocations.FirstOrDefault(c => c.Key.ToSearch().Like(ValorEntidade.Site.ToSearch()));

            sites = item.Value;

            if (sites != null)
            {
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.SegueEnderecoSite, item.Key));
                _activity.Attachments = cardBuilder.CreateHeroCards(new List<Location> { sites }).Select(C => C.ToAttachment()).ToList();
                await PostAsync();
                return;
            }

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoEntendiNomeSite, ValorEntidade.Cidade));
        }
    }
}