using Sample.Auth;
using Sample.EmployeeService.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.EmployeeManager
{
    public interface IEmployeeService
    {
        GetAllEmployeesResponse GetAllEmployees(AuthorizedRequest request);
        RegisterEmployeeResponse RegisterEmployee(AuthorizedRequest request, Employee employee);
    }
}
