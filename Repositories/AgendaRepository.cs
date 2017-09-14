
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json.Converters;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class AgendaRepository
    {

        public ICollection<Agenda> Listar()
        {

            return JsonResources.Get<ICollection<Agenda>>("agenda");
        }

        public Agenda Obter(DateTime time)
        {
            return Listar().FirstOrDefault(c => c.Data.Date == time.Date);
        }


        public ICollection<Agenda> PesquisarPorNome(string nome)
        {
            var agendasResultado = new List<Agenda>();
            var agendas = Listar();
            foreach (var agenda in agendas)
            {
                var atividades = agenda.Atividades.Where(x => x.Responsavel.ToSearch().Like(nome.ToSearch()));
                if (atividades.Any())
                {
                    agendasResultado.Add(new Agenda
                    {
                        Data = agenda.Data,
                        Atividades = atividades.ToList()
                    });
                }
            }

            return agendasResultado;
        }

        public ICollection<Agenda> PesquisarRefeicao(string refeicao, DateTime dataAtual)
        {
            var agendasResultado = new List<Agenda>();
            var agendas = Listar().Where(c => c.Data.Date == dataAtual.Date).ToList();

            foreach (var agenda in agendas)
            {
                var atividades = agenda.Atividades.Where(x => x.Descricao.ToSearch().Like(refeicao.ToSearch()) && x.Inicio.IncluirHoraData(dataAtual) > dataAtual).ToList();

                if (refeicao == "fome")
                {
                    var atividade = agenda.Atividades.FirstOrDefault(x =>
                     (x.Descricao.ToSearch().Like("coffee break")
                     ||
                     x.Descricao.ToSearch().Like("almoço")
                      ||
                     x.Descricao.ToSearch().Like("jantar"))
                     &&
                     x.Inicio.IncluirHoraData(dataAtual) > dataAtual
                     );

                    if (atividade != null)
                    {
                        atividades.Clear();
                        atividades.Add(atividade);

                    }
                }

                if (atividades.Any())
                {
                    agendasResultado.Add(new Agenda
                    {
                        Data = agenda.Data,
                        Atividades = atividades.ToList()
                    });
                }
            }

            return agendasResultado;
        }


        public ICollection<Atividade> PesquisarAtividades(DateTime data, string inicio, string fim)
        {
            var agenda = Obter(data);

            if (agenda == null)
                return new List<Atividade>();

            if (string.IsNullOrEmpty(inicio))
                inicio = fim;

            if (string.IsNullOrEmpty(fim))
                fim = inicio;

            var datInicio = inicio.IncluirHoraData(agenda.Data);
            var datFinal = fim.IncluirHoraData(agenda.Data);

            return agenda.Atividades.Where(c =>
               c.Inicio.IncluirHoraData(agenda.Data) <= datFinal &&
               c.Fim.IncluirHoraData(agenda.Data) >= datInicio
            ).ToList();

        }

        public ICollection<Atividade> PesquisarAtividades(DateTime data)
        {
            var agenda = Obter(data);

            if (agenda == null)
                return new List<Atividade>();


            return agenda.Atividades.Where(c =>
               c.Inicio.IncluirHoraData(agenda.Data) >= data
            ).ToList();

        }

    }
}