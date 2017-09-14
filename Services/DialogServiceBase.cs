using CTX.Bot.ConexaoLiq.LUIS;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Web.Configuration;
using System;
using System.Threading.Tasks;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Repositories;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using System.Collections.Generic;
using LuisBot;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class DialogServiceBase : IDialogService
    {

        protected class EntidadeValorLuis
        {
            EntityRecommendation entity;

            public EntidadeValorLuis(LuisResult result)
            {
                if (result == null)
                    return;

                Cargo = string.Empty;
                NomePessoa = string.Empty;
                Pais = string.Empty;
                Empresa = string.Empty;
                Departamento = string.Empty;

                if (result.TryFindEntity(Entidade.Cargo, out entity))
                    Cargo = entity.Entity;

                if (result.TryFindEntity(Entidade.NomePessoa, out entity))
                    NomePessoa = entity.Entity;

                if (result.TryFindEntity(Entidade.Pais, out entity))
                    Pais = entity.Entity;

                if (result.TryFindEntity(Entidade.NomeEmpresa, out entity))
                    Empresa = entity.Entity;

                if (result.TryFindEntity(Entidade.Departamento, out entity))
                    Departamento = entity.Entity;

                if (result.TryFindEntity(Entidade.NomeSite, out entity))
                    Site = entity.Entity;

                if (result.TryFindEntity(Entidade.Cargo, out entity))
                    Cidade = entity.Entity;

                if (result.TryFindEntity(Entidade.Estado, out entity))
                    Estado = entity.Entity;

                if (result.TryFindEntity(Entidade.Institucional, out entity))
                    Institucional = entity.Entity;

                if (result.TryFindEntity(Entidade.Saudacao, out entity))
                    Saudacao = entity.ValorCanonico();

                if (result.TryFindEntity(Entidade.InformacaoBot, out entity))
                    InformacaoBot = entity.Entity;

                if (result.TryFindEntity(Entidade.InformacaoBot, out entity))
                    Palavrao = entity.Entity;

                if (result.TryFindEntity(Entidade.AgendaAtividadeInicio, out entity))
                    AgendaAtividadeInicio = entity.Entity;

                if (result.TryFindEntity(Entidade.AgendaAtividadeFim, out entity))
                    AgendaAtividadeFim = entity.Entity;

                if (result.TryFindEntity(Entidade.AgendaAtividadeDia, out entity))
                    AgendaAtividadeDia = entity.ValorCanonico();

                if (result.TryFindEntity(Entidade.AgendaAtividadePeriodo, out entity))
                    AgendaAtividadePeriodo = entity.ValorCanonico();

                if (result.TryFindEntity(Entidade.AgendaAtividadeRefeicao, out entity))
                    AgendaAtividadeRefeicao = entity.ValorCanonico();
            }
            public string Cargo { get; set; }
            public string Pais { get; set; }
            public string Empresa { get; set; }
            public string Departamento { get; set; }
            public string Site { get; set; }
            public string NomePessoa { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Institucional { get; set; }
            public string Saudacao { get; set; }
            public string InformacaoBot { get; set; }
            public string Palavrao { get; set; }
            public string AgendaAtividadeInicio { get; set; }
            public string AgendaAtividadeFim { get; set; }

            public string AgendaAtividadeDia { get; set; }

            public string AgendaAtividadePeriodo { get; set; }

            public string AgendaAtividadeRefeicao { get; set; }
        }

        protected ResumeAfter<IMessageActivity> MessageReceived;
        protected readonly string UrlApi;
        protected readonly Activity _activity;
        protected readonly EntidadeValorLuis ValorEntidade;
        protected readonly EmpresaRepository _empresaRepository;
        protected readonly bool _textToSpeech;

        public DialogServiceBase(Activity activity, LuisResult result, bool textToSpeech)
        {

            UrlApi = WebConfigurationManager.AppSettings["urlApi"];
            ValorEntidade = new EntidadeValorLuis(result);
            _empresaRepository = new EmpresaRepository();
            _activity = activity;
            _textToSpeech = textToSpeech;
        }

        public async virtual Task Processar()
        {
            var empresa = _empresaRepository.Obter();

            if (!string.IsNullOrEmpty(ValorEntidade.Empresa) && ValorEntidade.Empresa.ToSearch() != empresa.Nome.ToSearch())
                await PostAsync($"Ops! \U0001F914 Infelizmente não tenho a informação que você solicitou sobre a empresa {ValorEntidade.Empresa}. Mas eu tenho informações da {empresa.Nome}.. Da uma olhadinha. \U0001F609");
        }

        public virtual async Task PostAsync()
        {
            await PostAsync(string.Empty);
        }

        public virtual async Task PostAsync(string text)
        {
            try
            {
                var reply = CreateReply();
                reply.Type = ActivityTypes.Message;
                reply.AttachmentLayout = _activity.AttachmentLayout;
                reply.Attachments = _activity.Attachments;

                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, reply))
                {
                    reply.Text = text;

                    if (_textToSpeech && !string.IsNullOrEmpty(reply.Text))
                    {

                        var urlApiMedia = WebConfigurationManager.AppSettings["urlMedia"];
                        var audio = await AudioConvert.TextToSpeech(reply.Text);
                        if (!string.IsNullOrEmpty(audio))
                        {
                            reply.Text = null;
                            if (reply.Attachments == null)
                                reply.Attachments = new List<Attachment>();


                            reply.Attachments.Add(new AudioCard
                            {
                                Media = new List<MediaUrl> { new MediaUrl { Url = $"{urlApiMedia}{audio.Replace("./wav/", "")}" } }
                            }.ToAttachment());
                        }
                    }

                    var client = scope.Resolve<IConnectorClient>();
                    await client.Conversations.ReplyToActivityAsync(reply);

                    _activity.Attachments.Clear();

                }
            }
            catch (Exception ex)
            {
                var reply = CreateReply();
                reply.Type = ActivityTypes.Message;

                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, reply))
                {
                    reply.Text = $"{ex.Message} Statck:{ex.StackTrace} Inner Exception: {ex?.InnerException.Message}";                    
                    var client = scope.Resolve<IConnectorClient>();
                    await client.Conversations.ReplyToActivityAsync(reply);
                }
            }
        }

        public async Task SendTyping()
        {
            var reply = CreateReply();
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, reply))
            {

                reply.Text = null;
                reply.Type = ActivityTypes.Typing;
                var client = scope.Resolve<IConnectorClient>();
                await client.Conversations.ReplyToActivityAsync(reply);
            }
        }

        private Activity CreateReply()
        {
            var botAccount = new ChannelAccount(_activity.From.Id, _activity.From.Name);
            _activity.From = botAccount;

            var reply = _activity.CreateReply();
            reply.Text = _activity.Text;

            return reply;
        }
    }

}