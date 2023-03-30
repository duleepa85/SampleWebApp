using Sample.DataAccess.DBFramework;
using Sample.EmployeeService.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Data.Actions
{
    public class RegisterEmployeeAction : DBActionBase<RegisterEmployeeResponse>
    {
        private readonly Employee _employee;
        public RegisterEmployeeAction(Employee employee)
        {
            _employee = employee;
        }

        protected override RegisterEmployeeResponse Body(DbConnection connection)
        {
            try
            {
                DbCommand command = connection.CreateCommand();
                command.CommandText = @"[employeeservice].[RegisterEmployee]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(DataAcessUtils.CreateParam("@EmployeeName", DbType.String, _employee.Name));
                command.Parameters.Add(DataAcessUtils.CreateParam("@Age", DbType.Int32, _employee.Age));
                int employeeId = Convert.ToInt32(command.ExecuteScalar());
                return new RegisterEmployeeResponse()
                {EmployeeId = employeeId, Message = "Employee is registered successfully", IsError = false} ;
            }
            catch (SqlException se)
            {
                if (se.Number == 50000)
                {
                    throw new ErrorResponse(HttpStatusCode.Conflict, se.Message);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
