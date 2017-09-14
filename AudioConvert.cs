using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace LuisBot
{
    public class AudioConvert
    {
        private static readonly string urlAudioConvert = WebConfigurationManager.AppSettings["urlAudioConvert"];

        public async static Task<string> SpeechToText(string urlAudio)
        {

            var client = new RestClient($"{urlAudioConvert}/downloadAudioConvertByRecognize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(new { Url = urlAudio });

            var result = client.Execute<Result>(request).Data;
            if (result != null && result.result != null)
            {

                var audioRecognize = JsonConvert.DeserializeObject<AudioRecognize>(result.result);

                return await Task.FromResult(audioRecognize.alternatives.FirstOrDefault().text);

            }

            return await Task.FromResult(string.Empty);
        }

        public async static Task<string> TextToSpeech(string text)
        {

            var client = new RestClient($"{urlAudioConvert}/recordTextToSpeech");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(new { Message = text, Voice = "carlos-highquality" });

            var result = client.Execute<Result>(request).Data;
            if (result != null && result.result != null)
            {
                var file = JsonConvert.DeserializeObject<FileResult>(result.result);

                return await Task.FromResult(file.file);

            }

            return await Task.FromResult(string.Empty);
        }

    }
}