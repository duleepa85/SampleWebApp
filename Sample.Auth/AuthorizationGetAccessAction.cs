using Sample.Auth.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.DataAccess.DBFramework;
using System.Data.SqlClient;

namespace Sample.Auth
{
    internal class AuthorizationGetAccessAction : DBCommonAction<TokenUser>
    {
        private readonly TokenUser _user;
        private readonly int _method;
        private readonly string _pathValue;

        public AuthorizationGetAccessAction(TokenUser user, int metod, string pathValue)
        {
            _user = user;
            _method = metod;
            _pathValue = pathValue;
        }

        protected override TokenUser Body(DbConnection connection)
        {
            try
            {
                DbCommand command = new SqlCommand
                {
                    Connection = (SqlConnection)connection,
                    CommandText = @"[UserService].[GetUserAuthorization]",
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@UserId", _user.AzureUserId));
                command.Parameters.Add(new SqlParameter("@Method", _method));
                command.Parameters.Add(new SqlParameter("@RouteTemplate", _pathValue));

                DbDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        _user.TenantUserLogin = Convert.ToString(dataReader["DbUserName"]);
                        _user.TenantUserPassword = Convert.ToString(dataReader["DbUserPassword"]);
                        _user.UserDBConnectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2}; Password={3}",
                        _user.TenantServer, _user.TenantDatabase, dataReader["DbUserName"].ToString(), dataReader["DbUserPassword"].ToString());
                        _user.HasAccessToSubOperation = true;
                    }
                    return _user;
                }
                else
                {
                    throw new Exception("User login failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
