using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Helpers
{
    public static class EntityRecommendationExtensions
    {

        public static string ValorCanonico(this EntityRecommendation entityRecommendation)
        {
            if (entityRecommendation != null)
            {
                if (entityRecommendation.Resolution != null)
                {
                    dynamic value = entityRecommendation.Resolution.First();

                    return value.Value.First.Value;
                }


                return entityRecommendation.Entity;
            }


            return string.Empty;
        }
    }
}