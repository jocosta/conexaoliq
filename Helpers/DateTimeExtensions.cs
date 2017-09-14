using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Helpers
{
    public enum SaudacaoForaDePeriodo
    {
        BoaTardeManha,
        BoaTardeNoite,
        BoaNoiteManha,
        BoaNoiteTarde,
        BomDiaNoite,
        BomDiaTarde,
        Indefinido
    }
    public enum SaudacaoPeriodoDia
    {
        BomDia,
        BoaTarde,
        BoaNoite
    }

    public static class DateTimeExtensions
    {
        public static SaudacaoPeriodoDia ObterPeriodoDia(this DateTime data)
        {
            if (data.Hour < 12)
                return SaudacaoPeriodoDia.BomDia;
            else if (data.Hour < 18)
                return SaudacaoPeriodoDia.BoaTarde;
            else
                return SaudacaoPeriodoDia.BoaNoite;
        }

        public static string DiaDaSemana(this DateTime data)
        {

            var culture = new System.Globalization.CultureInfo("pt-BR");
            return culture.DateTimeFormat.GetDayName(data.DayOfWeek);

        }

        //public static DateTime GerarDataPorHorario(this DateTime data, string horario)
        //{


        //}
    }
}