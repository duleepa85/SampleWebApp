using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.DomainObjects
{
    public class ErrorsResponse
    {
        public List<ErrorResponseClient> Errors { get; set; }
    }
}
