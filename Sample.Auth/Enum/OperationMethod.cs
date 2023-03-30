using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth.Enum
{
    public enum OperationMethod
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4,
        PATCH = 5
    }

    public enum TokenType
    {
        B2C,
        AD
    }
}
