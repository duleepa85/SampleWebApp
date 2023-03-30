using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth
{
    public class RouteMatcher
    {
        public static bool MatchPath(string requestPath, string routeTemplate)
        {
            var template = TemplateParser.Parse(routeTemplate);
            var matcher = new TemplateMatcher(template, GetDefaults(template));
            return matcher.TryMatch(requestPath, new RouteValueDictionary());
        }

        private static RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate)
        {
            var result = new RouteValueDictionary();

            foreach (var parameter in parsedTemplate.Parameters)
            {
                if (parameter.DefaultValue != null)
                {
                    result.Add(parameter.Name, parameter.DefaultValue);
                }
            }

            return result;
        }
    }
}
