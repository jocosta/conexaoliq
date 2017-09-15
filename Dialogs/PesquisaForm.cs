using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace CTX.Bot.ConexaoLiq.Dialogs
{
    public enum SimNaoOptions
    {
        [Prompt("\\U0001F601")]
        Sim,
        Não
    }

    public enum EmojisOptions
    {
        NaoGostei,
        Bom,
        Gostei,
        Excelente
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

        [Prompt("Deixe um comentário construtivo para melhoria contínua do nosso evento. (Opcional) Texto de 1000 Carácteres")]
        public string OpniaoMelhoriaEvento;

        public static IForm<PesquisaEventoForm> BuildForm()
        {
            return new FormBuilder<PesquisaEventoForm>()
                    .Message("Pesquisa de satisfação - Conexão Liq")
                    .OnCompletion(ResumeOnCompletion)
                    .Build();
        }

        private static async Task ResumeOnCompletion(IDialogContext context, PesquisaEventoForm state)
        {
            //throw new NotImplementedException();

            context.UserData.RemoveValue("IniciouPesquisa");
        }
    };
}