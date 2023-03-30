using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.DomainObjects
{
    public class RegisterEmployeeResponse
    {
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public bool IsError { get; set; }

    }
}
