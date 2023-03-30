using Sample.Auth.Models;
using Sample.DataAccess.DBFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sample.DataAccess.DBFramework;

namespace Sample.Auth
{
    public class AuthorizationTenantDBSupport : DBCommonAction<TokenUser>
    {
        private readonly TokenUser _user;

        public AuthorizationTenantDBSupport(TokenUser user)
        {
            _user = user;
        }

        protected override TokenUser Body(DbConnection connection)
        {
            try
            {
                DbCommand command = new SqlCommand
                {
                    Connection = (SqlConnection)connection,
                    CommandText = @"[UserService].[GetUserDBConnection]",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@AzureId", _user.AzureUserId));

                DbDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        _user.TenantUserLogin = Convert.ToString(dataReader["DbUserName"]);
                        _user.TenantUserPassword = Convert.ToString(dataReader["DbUserPassword"]);
                    }
                    return _user;
                }
                else
                {
                    throw new AuthorizationError(HttpStatusCode.Unauthorized, "Unauthorized Access");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
