using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Web.Configuration;

    namespace Ctx.Baas.Core.Helpers
    {
        public class JsonResources
        {

            public static T Get<T>(string resourceName)
            {
                var format = "dd/MM/yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                var path = WebConfigurationManager.AppSettings["ResourcesPath"];
                string file = File.ReadAllText($"{path}/{resourceName}.json");
                return JsonConvert.DeserializeObject<T>(file, dateTimeConverter);

            }
        }
    }

}