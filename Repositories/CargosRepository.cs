
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class CargosRepository
    {
        private readonly ICollection<Cargo> _cargos;

        public CargosRepository()
        {
            _cargos = JsonResources.Get<ICollection<Cargo>>("cargos");
        }

        public ICollection<Cargo> Listar()
        {
            return _cargos;
        }

        public ICollection<Cargo> PesquisarPorCargo(string valor)
        {
            var cargoTermos = valor.ToTagSearch();

            return _cargos.Where(c => cargoTermos.All(
                        x => c.Tags.Any(y => y.ToSearch().Like(x))
                )).ToList();


        }

        public ICollection<Cargo> PesquisarPorNome(string valor)
        {


            var cargoTermos = valor.ToTagSearch();

            return _cargos.Where(c => cargoTermos.All(
                        x => c.NomePessoa.ToTagSearch().Any(y => y.Like(x))
                )).ToList();


        }

        public ICollection<Cargo> Pesquisar(string valor)
        {

            var cargoTermos = valor.ToTagSearch();

            return _cargos.Where(c => cargoTermos.All(
                        x => c.NomePessoa.ToTagSearch().Any(y => y == x)
                        ||
                        c.Tags.Any(y => y.ToSearch().Like(x.ToSearch()))
                )).ToList();



        }

    }
}