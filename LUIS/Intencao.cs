using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.LUIS
{
    internal static class Intencao
    {
        public const string Cargos = "cargos";
        public const string informacoes_institucionais_clientes = "informacoes.institucionais.clientes";
        public const string informacoes_institucionais_faturamento = "informacoes.institucionais.faturamento";
        public const string informacoes_institucionais_premios = "informacoes.institucionais.premios";
        public const string informacoes_intitucionais_sites = "informacoes.intitucionais.sites";
        public const string numeros_funcionarios = "numeros.funcionarios";
        public const string informacao_funcionario = "informacao.funcionario";
        public const string informacoes_institucionais = "informacoes.institucionais";
        public const string cumprimento_saudacao = "cumprimento.saudacao";
        public const string informacoes_bot = "informacoes.bot";
        public const string usuario_malcriado = "usuario.malcriado";
        public const string agenda_consultar_atividade = "agenda.consultar.atividade";
        public const string Help = "help";
        public const string Vestimenta = "conexao.liq.vestimenta";
        public const string ConexaoLiqLocalizacao = "conexao.liq.localizacao";
        public const string AgradecimentoDespedida = "agradecimento.despedida";

        public const string ConexaoAppSenha = "conexao.app.senha";

        public static double GetScoreMatch(string constIntent)
        {
            double score = 0.4;
            switch (constIntent)
            {
                case Cargos:
                    score = 0.5;
                    break;
                case informacoes_institucionais_clientes:
                    score = 0.5;
                    break;
                case informacoes_institucionais_faturamento:
                    score = 0.4;
                    break;
                case informacoes_institucionais_premios:
                    score = 0.5;
                    break;
                case informacoes_intitucionais_sites:
                    score = 0.5;
                    break;
                case numeros_funcionarios:
                    score = 0.5;
                    break;
                case informacao_funcionario:
                    score = 0.6;
                    break;
                case informacoes_institucionais:
                    score = 0.6;
                    break;
                case cumprimento_saudacao:
                    score = 0.8;
                    break;
                case Help:
                    score = 0.5;
                    break;
                case Vestimenta:
                    score = 0.6;
                    break;
                case ConexaoLiqLocalizacao:
                    score = 0.6;
                    break;
                case AgradecimentoDespedida:
                    score = 0.6;
                    break;
                default:
                    score = 0.4;
                    break;
            }

            return score;
        }
    }
}