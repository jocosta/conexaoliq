using AutoMapper;
using CTX.Bot.ConexaoLiq.Dialogs;
using CTX.Bot.ConexaoLiq.Models;

namespace CTX.Bot.ConexaoLiq.Infra
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : base("MappingProfile", Configure)
        {
        }

        private static void Configure(IProfileExpression obj)
        {
            obj.CreateMap<PesquisaEventoForm, Pesquisa>().ReverseMap();
        }


    }
}