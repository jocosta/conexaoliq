using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot
{
    public class LuisClient
    {

        public const string urlApi = "https://eastus2.api.cognitive.microsoft.com/luis/v2.0/apps/{0}?subscription-key={1}&timezoneOffset=0&verbose=true&q={2}";
        private string _appId;
        private string _appKey;

        public LuisClient(string _appId, string _appKey)
        {
            this._appId = _appId;
            this._appKey = _appKey;
        }

        public LuisResult Get(string text)
        {

            var client = new RestClient(string.Format(urlApi, _appId, _appKey, HttpUtility.UrlEncode(text)));
            var request = new RestRequest(Method.GET);

            request.AddHeader("content-type", "application/json");

            var resp = client.Execute(request);

            return JsonConvert.DeserializeObject<LuisResult>(resp.Content);
        }
    }
}