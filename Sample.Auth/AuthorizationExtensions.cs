using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth
{
    public static class AuthorizationExtensions
    {
        public static IFunctionsHostBuilder AddAccessTokenBinding(this IFunctionsHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Services.AddWebJobs(x => { return; }).AddExtension<AuthorizationExtensionProvider>();
            return builder;
        }
    }
}
