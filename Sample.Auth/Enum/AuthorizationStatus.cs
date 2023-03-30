using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth.Enum
{
    public enum AuthorizationStatus
    {
        Valid,
        Expired,
        Error,
        NoToken,
        Unauthorized,
        NoTenantId
    }
}
