using Microsoft.Bot.Builder.FormFlow;
using System;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using CTX.Bot.ConexaoLiq.Models;
using System.Linq.Expressions;
using CTX.Bot.ConexaoLiq.Infra.Context;
using System.Linq;
using CTX.Bot.ConexaoLiq.Helpers;
using System.Configuration;
using Microsoft.Bot.Connector;

namespace CTX.Bot.ConexaoLiq.Dialogs
{
    public enum SimNaoOptions
    {
        [Terms("✅ Sim")]
        [Describe("✅ Sim")]
        Sim,
        [Terms("❌ Não")]
        [Describe("❌ Não")]
        Não
    }

    public enum EmojisOptions
    {
        [Terms("😍 Excelente")]
        [Describe("😍 Excelente")]
        Excelente,

        [Terms("😊 Gostei")]
        [Describe("😊 Gostei")]
        Gostei,

        [Terms("😐 Bom")]
        [Describe("😐 Bom")]
        Bom,

        [Terms("😔 Não curti")]
        [Describe("😔 Não curti")]
        NaoGostei
    }


    [Serializable]
    public class PesquisaEventoForm
    {

        [Prompt("Você gostou da dinâmica da nossa mestre de Cerimônias Fabrícia Ouriveis? {||}")]
        public EmojisOptions? DinamicaMestreCerimonia;

        [Prompt("O tema da marca, palestrado pela Futurebrand, ajudará em nosso reposicionamento? {||}")]
        public SimNaoOptions? TemaMarcaFutureBrand;

        [Prompt("A palestra da super campeã Hortencia motivou e inspirou a sua jornada profissional na Liq? {||}")]
        public SimNaoOptions? PalestraHortencia;


        [Prompt("Qual seu sentimento com os temas palestrados pelos membros do Comex?")]
        public string QualConhecimento;

        [Prompt("Desafios de hoje, rumo ao futuro - Nelson {||}")]
        public EmojisOptions? DesafiosHojeNelson;

        [Prompt("Planejamento Estratégico (5 anos) - Cris Barretto {||}")]
        public EmojisOptions? PlanejamentoEstrategico;

        [Prompt("Plano de Marketing, Novos Mercados e Trade - Marcelo Chianello {||}")]
        public EmojisOptions? PlanoMarketionChianello;

        [Prompt("Comercial e Produtos - Fátima Oliveira {||}")]
        public EmojisOptions? ComercialProdutosFatimaOliveira;

        [Prompt("Execução Operacional com Estratégia - Ana Coelho e Marcelo Chianello {||}")]
        public EmojisOptions? ExecucaoOperacional;

        [Prompt("Capital Humano como Diferencial Estratégico - Andrei Passig {||}")]
        public EmojisOptions? CapitalHumano;

        [Prompt("Tecnologia para um futuro smart - João Mendes {||}")]
        public EmojisOptions? TecnologiaSmart;

        [Prompt("Responsabilidade é Inegociável - Cris Cé {||}")]
        public EmojisOptions? ResponsabilidadeInegociavel;

        [Prompt("Você gostou de mim (LiqBot)? Pode ser sincero, ainda não desenvolvi inteligência emocional 😂😂 {||}")]
        public EmojisOptions? OpniaoBot;

        [Prompt("Você gostou da infraestrutura do evento (localização, hotel, comida, internet, limpeza, etc.)? {||}")]
        public EmojisOptions? InfraEvento;

        [Prompt("Deixe um comentário construtivo para melhoria contínua do nosso evento.")]
        public string OpniaoMelhoriaEvento;

        public static IForm<PesquisaEventoForm> BuildForm()
        {
            return new FormBuilder<PesquisaEventoForm>()
                    .Message("Pesquisa de satisfação - Conexão Liq")
                    .Field(nameof(DinamicaMestreCerimonia),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };
                                state.DinamicaMestreCerimonia = (EmojisOptions)value;
                                Registrar(state, c => c.DinamicaMestreCerimonia, value);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(TemaMarcaFutureBrand),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.TemaMarcaFutureBrand = (SimNaoOptions)value;
                                Registrar(state, c => c.TemaMarcaFutureBrand, value);

                                return await Task.FromResult(result);
                            })
                        .Field(nameof(PalestraHortencia),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.PalestraHortencia = (SimNaoOptions)value;

                                Registrar(state, c => c.PalestraHortencia, state.PalestraHortencia);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(QualConhecimento),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.QualConhecimento = nome;

                                Registrar(state, c => c.QualConhecimento, state.QualConhecimento);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(DesafiosHojeNelson),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.DesafiosHojeNelson = (EmojisOptions)value;

                                Registrar(state, c => c.DesafiosHojeNelson, state.DesafiosHojeNelson);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(PlanejamentoEstrategico),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.PlanejamentoEstrategico = (EmojisOptions)value;

                                Registrar(state, c => c.PlanejamentoEstrategico, state.PlanejamentoEstrategico);
                                return await Task.FromResult(result);
                            })
                         .Field(nameof(PlanoMarketionChianello),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.PlanoMarketionChianello = (EmojisOptions)value;

                                Registrar(state, c => c.PlanoMarketionChianello, state.PlanoMarketionChianello);
                                return await Task.FromResult(result);
                            })
                         .Field(nameof(ComercialProdutosFatimaOliveira),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.ComercialProdutosFatimaOliveira = (EmojisOptions)value;

                                Registrar(state, c => c.ComercialProdutosFatimaOliveira, state.ComercialProdutosFatimaOliveira);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(ExecucaoOperacional),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.ExecucaoOperacional = (EmojisOptions)value;

                                Registrar(state, c => c.ExecucaoOperacional, state.ExecucaoOperacional);
                                return await Task.FromResult(result);
                            })
                         .Field(nameof(CapitalHumano),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.CapitalHumano = (EmojisOptions)value;

                                Registrar(state, c => c.CapitalHumano, state.CapitalHumano);
                                return await Task.FromResult(result);
                            })
                         .Field(nameof(TecnologiaSmart),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.TecnologiaSmart = (EmojisOptions)value;

                                Registrar(state, c => c.TecnologiaSmart, state.TecnologiaSmart);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(ResponsabilidadeInegociavel),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.ResponsabilidadeInegociavel = (EmojisOptions)value;

                                Registrar(state, c => c.ResponsabilidadeInegociavel, state.ResponsabilidadeInegociavel);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(InfraEvento),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.InfraEvento = (EmojisOptions)value;

                                Registrar(state, c => c.InfraEvento, state.InfraEvento);
                                return await Task.FromResult(result);
                            })
                         .Field(nameof(OpniaoBot),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.OpniaoBot = (EmojisOptions)value;

                                Registrar(state, c => c.OpniaoBot, state.OpniaoBot);
                                return await Task.FromResult(result);
                            })
                        .Field(nameof(OpniaoMelhoriaEvento),
                            validate: async (state, value) =>
                            {
                                var nome = value + string.Empty;
                                var result = new ValidateResult { IsValid = true, Value = value };

                                state.OpniaoMelhoriaEvento = nome;

                                Registrar(state, c => c.OpniaoMelhoriaEvento, state.OpniaoMelhoriaEvento);
                                return await Task.FromResult(result);
                            })
                    .OnCompletion(ResumeOnCompletion)
                    .Message("Obrigado por ter respondido a pesquisa e até logo")
                    .Build();
        }

        private static async Task ResumeOnCompletion(IDialogContext context, PesquisaEventoForm state)
        {
            var channel = context.Activity.ChannelId;
            var userId = context.Activity.From.Id;
            var username = context.Activity.From.Name;
            Registrar(state, null, null, channel, userId, username);
            context.UserData.RemoveValue("IniciouPesquisa");
        }


        private static void Registrar(PesquisaEventoForm form, Expression<Func<Pesquisa, IComparable>> property, object value, string channel = "", string userId = "", string username = "")
        {
            if (string.IsNullOrEmpty(channel) && string.IsNullOrEmpty(userId)) return;

            var cadastro = AutoMapper.Mapper.Map<Pesquisa>(form);
            cadastro.UserId = userId;
            cadastro.Channel = channel;
            cadastro.Username = username;

            using (var dbContext = new BotContext())
            {
                Pesquisa cadastroExistente = null;

                if (!string.IsNullOrEmpty(channel) && !string.IsNullOrEmpty(userId))
                    cadastroExistente = dbContext.Pesquisas.FirstOrDefault(c => c.UserId == userId &&
                                                                                c.Channel == channel);

                if (cadastroExistente != null)
                {
                    if (property != null)
                    {
                        cadastro.Id = cadastroExistente.Id;
                        cadastroExistente.LastInteraction = DateTime.Now;
                        dbContext.Entry(cadastroExistente).Property(property.GetName()).IsModified = true;
                    }
                    else
                    {
                        cadastroExistente.Id = cadastroExistente.Id;
                        cadastroExistente.Channel = cadastroExistente.Channel;
                        cadastroExistente.UserId = cadastroExistente.UserId;
                        cadastroExistente.Username = cadastroExistente.Username;
                        cadastroExistente.Created = cadastroExistente.Created;
                        cadastroExistente.LastInteraction = cadastroExistente.LastInteraction;
                        cadastroExistente.DinamicaMestreCerimonia = cadastroExistente.DinamicaMestreCerimonia;
                        cadastroExistente.TemaMarcaFutureBrand = cadastroExistente.TemaMarcaFutureBrand;
                        cadastroExistente.PalestraHortencia = cadastroExistente.PalestraHortencia;
                        cadastroExistente.QualConhecimento = cadastroExistente.QualConhecimento;
                        cadastroExistente.DesafiosHojeNelson = cadastroExistente.DesafiosHojeNelson;
                        cadastroExistente.PlanejamentoEstrategico = cadastroExistente.PlanejamentoEstrategico;
                        cadastroExistente.PlanoMarketionChianello = cadastroExistente.PlanoMarketionChianello;
                        cadastroExistente.ComercialProdutosFatimaOliveira = cadastroExistente.ComercialProdutosFatimaOliveira;
                        cadastroExistente.ExecucaoOperacional = cadastroExistente.ExecucaoOperacional;
                        cadastroExistente.CapitalHumano = cadastroExistente.CapitalHumano;
                        cadastroExistente.TecnologiaSmart = cadastroExistente.TecnologiaSmart;
                        cadastroExistente.ResponsabilidadeInegociavel = cadastroExistente.ResponsabilidadeInegociavel;
                        cadastroExistente.OpniaoBot = cadastroExistente.OpniaoBot;
                        cadastroExistente.OpniaoMelhoriaEvento = cadastroExistente.OpniaoMelhoriaEvento;


                        dbContext.Entry(cadastroExistente).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                    dbContext.Pesquisas.Add(cadastro);

                dbContext.SaveChanges();
            }
        }
    };
}