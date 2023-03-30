using Sample.Auth;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Data.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Data
{
    public class EmployeeDataManager : IEmployeeDataManager
    {
        public GetAllEmployeesResponse GetAllEmployees(AuthorizedRequest request)
        {
            return new GetAllEmployeesAction().Execute(request.AuthorizedUser.UserDBConnectionString);
        }

        public RegisterEmployeeResponse RegisterEmployee(AuthorizedRequest request, Employee employee)
        {
            return new RegisterEmployeeAction(employee).Execute(request.AuthorizedUser.UserDBConnectionString);
        }
    }
}
