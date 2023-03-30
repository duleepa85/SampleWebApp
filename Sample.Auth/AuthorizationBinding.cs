using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth
{
    public class AuthorizationBinding : IBinding
    {
        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var request = context.BindingData["req"] as HttpRequest;

            return Task.FromResult<IValueProvider>(new AuthorizationValueProvider(request));
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}
