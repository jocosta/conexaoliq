using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class FaturamentoDialogService : DialogServiceBase, IDialogService
    {
        private readonly FaturamentoRepository _faturamentoRepository;
        private readonly MensagemRepository _mensagemRepository;

        public FaturamentoDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _faturamentoRepository = new FaturamentoRepository();
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();
            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.QualFaturamentoEmpresa));

        }
    }
}