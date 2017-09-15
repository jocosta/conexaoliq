using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System;
using System.Threading;
using CTX.Bot.ConexaoLiq.Helpers;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class SenhaAppDialogService : DialogServiceBase, IDialogService
    {

        public SenhaAppDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {


        }
        public async override Task Processar()
        {

            await SendTyping();
            await base.Processar();
            await SendTyping();


            var mensagens = new List<string>
            {

                "Ei! Eu vou te enviar a senha, mas não passe para ninguem combinado? @piscada@piscada",
                "Eu deixei a senha anotada em algum pedaço de papel por aqui.... espera ai que eu já te mando. @piscada@piscada",
                "Espera um pouquinho que eu já te mando... eu só preciso lembrar. @piscada@piscada",
                "Pois é! A senha.... nossa ta dificil de lembrar qual era. Só um instantinho please. @piscada@piscada",

            };

            Random random = new Random();
            await PostAsync(mensagens[random.Next(mensagens.Count)].IncluirEmojis());
            Thread.Sleep(3000);
            await SendTyping();
            await PostAsync("\U0001F447\U0001F447\U0001F447\U0001F447\U0001F447\U0001F447\n>Usuário: liqlider Senha: Liq@2017");

            Thread.Sleep(1500);


        }
    }
}