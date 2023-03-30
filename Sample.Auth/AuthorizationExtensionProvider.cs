using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth
{
    public class AuthorizationExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var provider = new AuthorizationBindingProvider();
            var rule = context.AddBindingRule<AuthorizationAttribute>().Bind(provider);
        }
    }
}
