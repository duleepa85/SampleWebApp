using Sample.DataAccess.DBFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.Utill
{
    public class GetConnectionStringByDomainCode
    {
        public readonly string _domainCode;
        public readonly string _dbType;
        public readonly string _connectionString;

        public GetConnectionStringByDomainCode(EnumDatabase dbType, string domainCode, string connectionString)
        {
            _domainCode = domainCode;
            _dbType = dbType.ToString();
            _connectionString = connectionString;
        }

        internal string Execute()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "USDataAccess_BoxGetDBConnection";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserCode", _domainCode));
                    command.Parameters.Add(new SqlParameter("@DbType", _dbType));
                    return (string)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_connectionString + " Error occurred while connectiong to the workstatoin database, "
                    + ex.Message);
            }
        }
    }
}
