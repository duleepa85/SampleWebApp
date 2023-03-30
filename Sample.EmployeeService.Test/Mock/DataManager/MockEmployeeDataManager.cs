using Moq;
using Sample.Auth;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Test.Mock.DataManager
{
    public class MockEmployeeDataManager : Mock<IEmployeeDataManager>
    {
        public MockEmployeeDataManager RegisterEmployee(RegisterEmployeeResponse registerEmployeeResponse)
        {
            Setup(x => x.RegisterEmployee(It.IsAny<AuthorizedRequest>(), It.IsAny<Employee>())).Returns(registerEmployeeResponse);
            return this;
        }
    }
}
