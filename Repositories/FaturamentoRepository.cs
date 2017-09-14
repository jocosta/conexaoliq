using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class FaturamentoRepository
    {
        BrowserSession _browserSession = new BrowserSession();

        Dictionary<int, string> trimestres = new Dictionary<int, string>
        {
            {1, "Primeiro Trimestre" },
             {2, "Segundo Trimestre" },
             {3, "Terceiro Trimestre" },
             {4, "Quarto Trimestre" },

        };

        public Faturamento LinkDemonstrativo()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(_browserSession.Get("http://ri.contax.com.br/conteudo_pt.asp?idioma=0&conta=28&tipo=64528"));

            var periodos = html.DocumentNode.SelectNodes("//table//tr");

            var demonstracoes = periodos.FirstOrDefault(c => c.ChildNodes.Any(x => x.InnerText == "Demonstrações Financeiras"));

            var trimestre = 0;
            foreach (var demonstracao in demonstracoes.ChildNodes)
            {
                if (demonstracao.GetAttributeValue("class", "") == "espaco")
                {
                    var link = demonstracao.SelectSingleNode("strong//a");
                    if (link == null)
                        break;

                    trimestre++;
                }

            }
            var pdf = demonstracoes.SelectSingleNode($"td[{trimestre}]//strong//a");
            return new Faturamento(trimestres[trimestre], $"http://ri.contax.com.br/{ pdf.GetAttributeValue("href", "#")}");


        }

    }
}