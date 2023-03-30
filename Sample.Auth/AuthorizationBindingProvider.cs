using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth
{
    public class AuthorizationBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AuthorizationBinding();
            return Task.FromResult(binding);
        }
    }
}
