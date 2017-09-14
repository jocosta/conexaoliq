using LuisBot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CTX.Bot.ConexaoLiq.Helpers
{
    public static class StringExtensions
    {

        public static bool Like(this string toSearch, string toFind)
        {
            if (string.IsNullOrEmpty(toFind))
                return false;


            toFind = new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*");

            return new Regex(@"\b" + toFind + @"\b", RegexOptions.Singleline).IsMatch(toSearch);
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string IncluirEmojis(this string text)
        {
            return Emojis.SubstituirTags(text);
        }

        public static string ReplaceWholeWord(this string original, string wordToFind, string replacement, RegexOptions regexOptions = RegexOptions.None)
        {

            string pattern = string.Format(@"\b{0}\b", wordToFind);
            string ret = Regex.Replace(original, pattern, replacement, regexOptions);
            return ret;
        }

        public static string ToSearch(this string text)
        {

            text = text + string.Empty;

            return text.ToLower().RemoveAccents();
        }

        public static string[] ToTagSearch(this string text)
        {


            return (text + string.Empty).Split(' ').Select(c => c.ToSearch()).ToArray();

        }

        public static bool EhSaudacaoPeriodoDia(this string text)
        {
            text = text + string.Empty;
            text = Regex.Replace(text, "[^A-Za-z]", string.Empty);

            return SaudacaoPeriodoDia.BomDia.ToString().ToLower() == text ||
                SaudacaoPeriodoDia.BoaTarde.ToString().ToLower() == text ||
                SaudacaoPeriodoDia.BoaNoite.ToString().ToLower() == text;
        }

        public static bool EhSaudacaoPeriodoDiaInvalida(this string text, SaudacaoPeriodoDia saudacaoPeriodoDia)
        {
            text = text + string.Empty;
            text = Regex.Replace(text, "[^A-Za-z]", string.Empty);

            return saudacaoPeriodoDia.ToString().ToLower() != text;
        }

        public static SaudacaoForaDePeriodo ObterSaudacaoForaDePeriodo(this string text, SaudacaoPeriodoDia saudacaoPeriodoDia)
        {
            text = text + string.Empty;
            text = Regex.Replace(text, "[^A-Za-z]", string.Empty);


            if (saudacaoPeriodoDia == SaudacaoPeriodoDia.BomDia)
            {
                if (text == SaudacaoPeriodoDia.BoaTarde.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BoaTardeManha;
                else if (text == SaudacaoPeriodoDia.BoaNoite.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BoaNoiteManha;
            }

            if (saudacaoPeriodoDia == SaudacaoPeriodoDia.BoaTarde)
            {
                if (text == SaudacaoPeriodoDia.BoaNoite.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BoaNoiteTarde;
                else if (text == SaudacaoPeriodoDia.BomDia.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BomDiaTarde;
            }


            if (saudacaoPeriodoDia == SaudacaoPeriodoDia.BoaNoite)
            {
                if (text == SaudacaoPeriodoDia.BoaTarde.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BoaTardeNoite;
                else if (text == SaudacaoPeriodoDia.BomDia.ToString().ToLower())
                    return SaudacaoForaDePeriodo.BomDiaNoite;
            }

            return SaudacaoForaDePeriodo.Indefinido;

        }

        public static DateTime IncluirHoraData(this string hora, DateTime dataAtual)
        {


            DateTime horaData;
            DateTime.TryParse($"{dataAtual.ToShortDateString()} {hora}", out horaData);

            return horaData;
        }

        public static string FormatarHora(this string hora)
        {

            if (!string.IsNullOrEmpty(hora))
            {
                if (Regex.IsMatch(hora, "[0-9a-zA-z]"))
                    hora = Regex.Replace(hora, "[^0-9:]", string.Empty);

                if (hora.Contains(":") && hora.Length == 5)
                    return hora;
                if (hora.Length <= 2)
                    return $"{hora}:00";
            }


            return string.Empty;
        }

        public static DateTime? RetornarHoraFinalPeriodoDia(this string periodo, DateTime dataAtual)
        {

            switch (periodo)
            {
                case ("manhã"):
                    return "11:59".IncluirHoraData(dataAtual);
                case ("tarde"):
                    return "18:59".IncluirHoraData(dataAtual);
                case ("noite"):
                    return "23:59".IncluirHoraData(dataAtual);
                case ("madrugada"):
                    return "05:59".IncluirHoraData(dataAtual);
                case ("meio-dia"):
                    return "12:59".IncluirHoraData(dataAtual);
                case ("agora"):
                    return dataAtual.ToString("hh:MM").IncluirHoraData(dataAtual);

                default:
                    return null;
            }
        }
        public static DateTime? RetornarHoraInicioPeriodoDia(this string periodo, DateTime dataAtual)
        {

            switch (periodo)
            {
                case ("manhã"):
                    return "06:00".IncluirHoraData(dataAtual);
                case ("tarde"):
                    return "12:59".IncluirHoraData(dataAtual);
                case ("noite"):
                    return "18:59".IncluirHoraData(dataAtual);
                case ("madrugada"):
                    return "00:00".IncluirHoraData(dataAtual);
                case ("meio-dia"):
                    return "12:00".IncluirHoraData(dataAtual);
                case ("agora"):
                    return dataAtual.ToString("hh:MM").IncluirHoraData(dataAtual);

                default:
                    return null;
            }
        }

        public static DateTime RetornarDataDoDia(this string dia, DateTime dataAtual)
        {

            switch (dia)
            {
                case ("amanhã"):
                    return dataAtual.AddDays(1);
                case ("hoje"):
                    return dataAtual;
                default:
                    return ObterDataPorDiaDaSemana(dia, dataAtual);
            }
        }

        private static DayOfWeek ObterDiaDaSemana(string dia)
        {

            switch (dia)
            {
                case ("segunda-feira"):
                    return DayOfWeek.Monday;
                case ("terça-feira"):
                    return DayOfWeek.Tuesday;
                case ("quarta-feira"):
                    return DayOfWeek.Wednesday;
                case ("quinta-feira"):
                    return DayOfWeek.Thursday;
                case ("sexta-feira"):
                    return DayOfWeek.Friday;
                case ("sábado"):
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Sunday;
            }
        }

        private static DateTime ObterDataPorDiaDaSemana(string dia, DateTime dataAtual)
        {


            var hoje = dataAtual;

            if (string.IsNullOrEmpty(dia))
                return hoje;


            var i = 1;
            while (true)
            {

                if (i > 7)
                    return dataAtual;

                if (hoje.DayOfWeek == ObterDiaDaSemana(dia))
                    break;

                hoje = hoje.AddDays(1);

                i++;
            }

            return hoje;
        }
    }
}