using CTX.Bot.ConexaoLiq.LUIS;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class DialogServiceFactory
    {


        public static IDialogService Create(Activity activity, LuisResult luisResult, IntentRecommendation intent, bool textToSpeech)
        {
            switch (intent.Intent)
            {
                case Intencao.Cargos:
                    return new CargoDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_institucionais:
                    return new InstitucionalDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_institucionais_clientes:
                    return new ClienteDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_institucionais_faturamento:
                    return new FaturamentoDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_institucionais_premios:
                    return new PremiosDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_intitucionais_sites:
                    return new SiteDialogService(activity, luisResult, textToSpeech);
                case Intencao.numeros_funcionarios:
                    return new FuncionarioDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacao_funcionario:
                    return new InformacaoFuncionarioDialogService(activity, luisResult, textToSpeech);
                case Intencao.cumprimento_saudacao:
                    return new SaudacaoDialogService(activity, luisResult, textToSpeech);
                case Intencao.informacoes_bot:
                    return new InformacaoBotDialogService(activity, luisResult, textToSpeech);
                case Intencao.usuario_malcriado:
                    return new UsuarioMalcriadoDialogService(activity, luisResult, textToSpeech);
                case Intencao.Help:
                    return new HelpDialogService(activity, luisResult, textToSpeech);
                case Intencao.Vestimenta:
                    return new VestimentaDialogService(activity, luisResult, textToSpeech);
                case Intencao.agenda_consultar_atividade:
                    return new AgendaDialogService(activity, luisResult, textToSpeech);
                case Intencao.ConexaoLiqLocalizacao:
                    return new LocalizacaoDialogService(activity, luisResult, textToSpeech);
                case Intencao.AgradecimentoDespedida:
                    return new DespedidaDialogService(activity, luisResult, textToSpeech);
                default:
                    return new NoneDialogService(activity, luisResult, textToSpeech);
            }
        }
    }
}