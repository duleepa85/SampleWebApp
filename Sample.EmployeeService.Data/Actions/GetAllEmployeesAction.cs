using Sample.DataAccess.DBFramework;
using Sample.EmployeeService.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Data.Actions
{
    public class GetAllEmployeesAction : DBActionBase<GetAllEmployeesResponse>
    {
        public GetAllEmployeesAction() { }

        protected override GetAllEmployeesResponse Body(DbConnection connection)
        {
            List<Employee> _employees = new List<Employee>();
            try
            {
                DbCommand command = new SqlCommand
                {
                    Connection = (SqlConnection)connection,
                    CommandText = @"[employeeservice].[GetEmployees]",
                    CommandType = CommandType.StoredProcedure
                };

                DbDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Employee emp = new Employee();
                    emp.Id = Convert.ToInt32(dataReader["EmployeeId"]);
                    emp.Name = Convert.ToString(dataReader["EmployeeName"]);
                    emp.Age = Convert.ToInt32(dataReader["Age"]);
                    _employees.Add(emp);
                }
                return new GetAllEmployeesResponse() { Employees = _employees };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
