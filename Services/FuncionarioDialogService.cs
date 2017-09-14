using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.LUIS;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class FuncionarioDialogService : DialogServiceBase, IDialogService
    {
        private readonly MensagemRepository _mensagemRepository;
        public FuncionarioDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();

            await SendTyping();
            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.QuantidadeColaboradores));

        }
    }
}