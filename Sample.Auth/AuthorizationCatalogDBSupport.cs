using Sample.Auth.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sample.DataAccess.DBFramework;
using System.Data.SqlClient;

namespace Sample.Auth
{
    public class AuthorizationCatalogDBSupport : DBCommonAction<TokenUser>
    {
        private readonly TokenUser _user;

        public AuthorizationCatalogDBSupport(TokenUser user)
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
                    CommandText = @"[dbo].[GetTenantDBCommonConnection]",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@TenantId", _user.TenantId));

                DbDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        _user.TenantServer = Convert.ToString(dataReader["TenantServer"]);
                        _user.TenantDatabase = Convert.ToString(dataReader["TenantDatabase"]);
                        _user.TenantCommonLogin = Convert.ToString(dataReader["TenantCommonLogin"]);
                        _user.TenantCommonPassword = Convert.ToString(dataReader["TenantCommonPassword"]);
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
