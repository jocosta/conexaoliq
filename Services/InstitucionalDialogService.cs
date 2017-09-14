using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.LUIS;
using CTX.Bot.ConexaoLiq.Repositories;
using Microsoft.Bot.Builder.Luis;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class InstitucionalDialogService : DialogServiceBase, IDialogService
    {
        private readonly MensagemRepository _mensagemRepository;
        public InstitucionalDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {
            await SendTyping();
            await base.Processar();
            await SendTyping();

            var empresa = _empresaRepository.Obter();
            var mensagem = empresa.Institucional.FirstOrDefault(c => c.Tags.Any(x => x.ToSearch().Like( ValorEntidade.Institucional.ToSearch())));
            var msgs = new[] { "por que", "pq", "porque", "o que é", "o que" };

            if(string.IsNullOrEmpty(ValorEntidade.Institucional) &&  msgs.Any(c =>  _activity.Text.ToLower().Contains(c)))
                await PostAsync("Entendemos que precisávamos refletir uma de nossas principais características: a adaptabilidade LIQ é um nome criado a partir da palavra \"líquido\". Este nome foi escolhido por ser curto, moderno e disruptivo, fato que reforça esta flexibilidade fluidez e dinamismo. Uma empresa que se molda perfeitamente às necessidades dos clientes e ocupa, naturalmente, novos espaços.");
            else if (mensagem == null)
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.DadoInstitucionalNaoEncontrado));
            else
                await PostAsync(mensagem.Texto);
        }
    }
}