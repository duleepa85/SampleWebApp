using Moq;
using Sample.Auth;
using Sample.EmployeeService.EmployeeManager;
using Sample.EmployeeService.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial.Arenas;

namespace Sample.EmployeeService.Test.Mock.Service
{
    public class MockEmployeeService : Mock<IEmployeeService>
    {
        public MockEmployeeService RegisterEmployee(RegisterEmployeeResponse registerEmployeeResponse)
        {
            Setup(x => x.RegisterEmployee(It.IsAny<AuthorizedRequest>(), It.IsAny<Employee>())).Returns(registerEmployeeResponse);
            return this;
        }
        public MockEmployeeService GetEmployees(GetAllEmployeesResponse getAllEmployeesResponse)
        {
            Setup(x => x.GetAllEmployees(It.IsAny<AuthorizedRequest>())).Returns(getAllEmployeesResponse);
            return this;
        }
    }
}
