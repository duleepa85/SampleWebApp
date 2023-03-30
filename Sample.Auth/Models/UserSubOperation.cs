using Sample.Auth.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth.Models
{
    public sealed class UserSubOperation
    {
        public OperationMethod Method { get; set; }
        public string RouteTemplate { get; set; }
    }
}
