using Sample.DataAccess.DBFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.Utill
{
    internal class GetConStringFromDbAction : DBActionBase<string>
    {
        public string _companyCode = string.Empty;
        public string _dbType = string.Empty;

        public GetConStringFromDbAction(string companyCode)
        {
            _companyCode = companyCode;
        }

        public GetConStringFromDbAction(string companyCode, string DbType)
        {
            _companyCode = companyCode;
            _dbType = DbType;
        }

        protected override string Body(DbConnection connection)
        {
            string spName = "USDataAccess_BoxGetDBConnection";
            string conection = string.Empty;
            try
            {
                DbCommand command = CreateCommand(System.Data.CommandType.StoredProcedure, spName);
                command.Parameters.Add(DataAcessUtils.CreateParam("@UserCode", DbType.String, _companyCode));

                if (!String.IsNullOrEmpty(_dbType))
                {
                    command.Parameters.Add(DataAcessUtils.CreateParam("@DbType", DbType.String, _dbType));
                }

                DbDataReader dataReader;
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    conection = Convert.ToString(dataReader["USBOXDB"]);
                }

                return conection;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
