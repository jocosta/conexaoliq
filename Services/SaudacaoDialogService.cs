using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Repositories;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class SaudacaoDialogService : DialogServiceBase, IDialogService
    {
        private readonly SaudacaoRepository _saudacaoRepository;
        private readonly MensagemRepository _mensagemRepository;
        public SaudacaoDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _saudacaoRepository = new SaudacaoRepository();
            _mensagemRepository = new MensagemRepository();

        }
        public async override Task Processar()
        {

            await SendTyping();
            await base.Processar();
            await SendTyping();

            var saudacao = ValorEntidade.Saudacao + string.Empty;
            var periodoDia = DateTime.Now.ObterPeriodoDia();

            var informacao = _saudacaoRepository.Obter(saudacao);
            var saudacaoForaDePeriodo = saudacao.ObterSaudacaoForaDePeriodo(periodoDia);

            if (saudacao.EhSaudacaoPeriodoDia() && saudacao.EhSaudacaoPeriodoDiaInvalida(periodoDia) && saudacaoForaDePeriodo != SaudacaoForaDePeriodo.Indefinido)
            {
                informacao = _saudacaoRepository.Obter(saudacaoForaDePeriodo.ToString());
                if (informacao.Texto.Contains("{0}"))
                    informacao.Texto = string.Format(informacao.Texto, DateTime.Now.ToString("HH:mm"));
            }
            else
            {
                informacao = _saudacaoRepository.Obter(saudacao);
            }
            
            if (informacao == null)
            {
                _activity.Attachments.Add(new Attachment()
                {
                    ContentUrl = $"{UrlApi}/images/bot.gif",
                    ContentType = "image/gif",
                    Name = "Liq"
                });
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.SaudacaoPadrao));
            }
            else
            {
                if (!string.IsNullOrEmpty(informacao.Imagem.Caminho))
                {
                    _activity.Attachments.Add(new Attachment()
                    {
                        ContentUrl = $"{UrlApi}{informacao.Imagem.Caminho}",
                        ContentType = "image/gif",
                        Name = "Liq"
                    });
                }

                await PostAsync(informacao.Texto);
            }
        }
    }
}