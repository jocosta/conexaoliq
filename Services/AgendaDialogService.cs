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

    public class AgendaDialogService : DialogServiceBase, IDialogService
    {

        private readonly AgendaRepository _agendaRepository;
        private readonly MensagemRepository _mensagemRepository;
        public AgendaDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _agendaRepository = new AgendaRepository();
            _mensagemRepository = new MensagemRepository();
        }

        public async override Task Processar()
        {
            try
            {


                await SendTyping();
                await base.Processar();

                _activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                _activity.Attachments = new List<Attachment>();

                await SendTyping();

                var inicio = string.Empty;
                var final = string.Empty;
                var periodo = string.Empty;
                var dia = string.Empty;
                var refeicao = string.Empty;
                var nome = string.Empty;

                var dataAtual = DateTime.Now;

                if (!string.IsNullOrEmpty(ValorEntidade.NomePessoa))
                    nome = ValorEntidade.NomePessoa;

                if (!string.IsNullOrEmpty(nome))
                {
                    await MensagemDadosAgendaPorResponsavelAsync(nome);
                    return;
                }

                if (!string.IsNullOrEmpty(ValorEntidade.AgendaAtividadeRefeicao))
                    refeicao = ValorEntidade.AgendaAtividadeRefeicao;

                if (!string.IsNullOrEmpty(ValorEntidade.AgendaAtividadePeriodo))
                    periodo = ValorEntidade.AgendaAtividadePeriodo;

                if (!string.IsNullOrEmpty(periodo))
                {
                    await MensagemDadosAgendaPorPeriodoAsync(periodo, dataAtual);
                    return;
                }

                if (!string.IsNullOrEmpty(ValorEntidade.AgendaAtividadeDia))
                    dia = ValorEntidade.AgendaAtividadeDia;

                if (!string.IsNullOrEmpty(refeicao))
                {
                    await MensagemDadosAgendaPorRefeicaoAsync(refeicao, dia, dataAtual);
                    return;
                }

                if (!string.IsNullOrEmpty(ValorEntidade.AgendaAtividadeInicio))
                    inicio = ValorEntidade.AgendaAtividadeInicio;

                if (!string.IsNullOrEmpty(ValorEntidade.AgendaAtividadeFim))
                    final = ValorEntidade.AgendaAtividadeFim;

                inicio = inicio.FormatarHora();
                final = final.FormatarHora();

                if (!string.IsNullOrEmpty(dia))
                    dataAtual = dia.RetornarDataDoDia(dataAtual);

                var atividades = _agendaRepository.PesquisarAtividades(dataAtual, inicio, final);

                if (!atividades.Any())
                {
                    await SendTyping();
                    await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.AlgumasAtividadesEncontradas));

                    var agendas = _agendaRepository.Listar().Where(c => c.Data.Date > dataAtual.Date).ToList();

                    await SendTyping();
                    await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.EnviandoAgendaCompleta));
                    await MensagemDadosAgendaCompletaAsync(agendas);

                }
                else
                {
                    await SendTyping();
                    await MensagemDadosAgendaAsync(atividades);
                }
            }
            catch (Exception ex)
            {

                await PostAsync(ex.Message + ex.StackTrace);
            }

        }

        private async Task MensagemDadosAgendaCompletaAsync(ICollection<Agenda> agendas)
        {
            foreach (var agenda in agendas)
            {
                var data = agenda.Data;
                foreach (var atividade in agenda.Atividades)
                {
                    ThumbnailCard thumbnailCard = new ThumbnailCard($"{data.DiaDaSemana()} - {data.ToString("dd/MM")}", $"{atividade.Responsavel}  {atividade.Inicio} às {atividade.Fim}", atividade.Descricao, null);
                    _activity.Attachments.Add(thumbnailCard.ToAttachment());
                }
            }

            await PostAsync();
        }

        private async Task MensagemDadosAgendaAsync(ICollection<Atividade> atividades)
        {
            foreach (var atividade in atividades)
            {

                ThumbnailCard thumbnailCard = new ThumbnailCard($"{atividade.Inicio} às {atividade.Fim}", atividade.Responsavel, atividade.Descricao, null);
                _activity.Attachments.Add(thumbnailCard.ToAttachment());
            }

            await PostAsync();
        }

        private async Task MensagemDadosAgendaPorResponsavelAsync(string nome)
        {
            var agendas = _agendaRepository.PesquisarPorNome(nome);

            if (!agendas.Any())
            {
                await SendTyping();
                await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoEncontreiAtividadeDoResponsavel, nome));
                return;
            }

            await MensagemDadosAgendaCompletaAsync(agendas);

        }

        private async Task MensagemDadosAgendaPorRefeicaoAsync(string refeicao, string dia, DateTime dataAtual)
        {
            var agendas = _agendaRepository.PesquisarRefeicao(refeicao, dia.RetornarDataDoDia(dataAtual));

            if (!agendas.Any())
            {
                await SendTyping();
                if (refeicao == "almoço")
                    await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.AchoQueOHorarioDeAlmoçoJaPassou));
                else
                    await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.NaoConseguiEncontrarUmaRefeicao));

                return;
            }

            await MensagemDadosAgendaCompletaAsync(agendas);

        }

        private async Task MensagemDadosAgendaPorPeriodoAsync(string periodo, DateTime dataAtual)
        {
            var atividades = new List<Atividade>();
            var agenda = _agendaRepository.Listar().FirstOrDefault(c => c.Data.Date == dataAtual.Date);

            if (periodo == "depois")
            {

                var atividade = _agendaRepository.PesquisarAtividades(dataAtual, dataAtual.ToShortDateString(), dataAtual.ToShortDateString()).FirstOrDefault();

                if (atividade != null)
                {
                    var proximaAtividade = agenda.Atividades.Where(c => c.Inicio == atividade.Fim);

                    if (proximaAtividade.Any())
                    {
                        atividades.Add(proximaAtividade.FirstOrDefault());
                        await MensagemDadosAgendaAsync(atividades);
                        return;
                    }
                }

            }

            await PostAsync(_mensagemRepository.Pesquisar(TipoMensagem.AlgumasAtividadesEncontradas));
            var inicio = periodo.RetornarHoraInicioPeriodoDia(dataAtual);
            var fim = periodo.RetornarHoraFinalPeriodoDia(dataAtual);

            if (inicio == null)
                atividades = _agendaRepository.PesquisarAtividades(dataAtual).ToList();
            else
                atividades = _agendaRepository.PesquisarAtividades(dataAtual, inicio.Value.ToShortTimeString(), fim.Value.ToShortTimeString()).ToList();

            await MensagemDadosAgendaAsync(atividades);

        }

    }
}