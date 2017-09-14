using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Builder.Location.Bing;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class LocalizacaoDialogService : DialogServiceBase, IDialogService
    {
        private readonly string ApiKey;
        public LocalizacaoDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            ApiKey = WebConfigurationManager.AppSettings["BingMapsApiKey"];
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();

            var resourceManager = new LocationResourceManager();
            var cardBuilder = new LocationCardBuilder(ApiKey, resourceManager);
            await SendTyping();

            await PostAsync("Segue o endereço do encontro:");

            IGeoSpatialService bingService = new Bing.BingGeoSpatialService(ApiKey);

            var locationSet = await bingService.GetLocationsByQueryAsync(@"Rodovia Engenheiro Candido do Rêgo Chaves, Km 50, 4500 - Jundiapeba, Mogi das Cruzes - SP, 08751-001");
            
            var locations = locationSet.Locations.First();

            _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            
            _activity.Attachments = cardBuilder.CreateHeroCards(new List<Location>{ locations}).Select(C => C.ToAttachment()).ToList();

            await PostAsync();

            await PostAsync("Ah... dirija com cuidado e tenha um excelente evento!");
        }

    }
}