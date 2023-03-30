using Sample.Auth;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.EmployeeManager
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDataManager _employeeDataManager;

        /// <param name="employeeDataManager">Injecting the dependency to the constructor</param>
        public EmployeeService(IEmployeeDataManager employeeDataManager)
        {
            _employeeDataManager = employeeDataManager;
        }

        /// <summary>
        /// Call data manager to get employees
        /// </summary>
        /// <returns>List of employees</returns>
        public GetAllEmployeesResponse GetAllEmployees(AuthorizedRequest request)
        { 
            return _employeeDataManager.GetAllEmployees(request);
        }

        /// <summary>
        /// Call data manager to register en employee
        /// </summary>
        /// <param name="employee">The object that save in the db</param>
        /// <returns>type of RegisterEmployeeResponse which contains response summery of the execution</returns>
        public RegisterEmployeeResponse RegisterEmployee(AuthorizedRequest request, Employee employee)
        {
            return _employeeDataManager.RegisterEmployee(request, employee);
        }

    }
}
