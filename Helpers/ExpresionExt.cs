using CTX.Bot.ConexaoLiq.Models;
using System;
using System.Linq.Expressions;

namespace CTX.Bot.ConexaoLiq.Helpers
{
    public static class ExpresionExt
    {
        public static string GetName(this Expression<Func<Pesquisa, IComparable>> expression)
        {
            var name = string.Empty;
            if (expression.Body is MemberExpression)
            {
                name = ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                name = ((MemberExpression)op).Member.Name;
            }

            return name;
        }      

        public static string ToCompare(this string text)
        {
            return text.ToLower().RemoveAccents();
        }

    }
}