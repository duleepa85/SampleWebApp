using Sample.DataAccess.DBFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.Utill
{
    public class ConnectionStringProvider
    {
        public static string GetConStringFromDb(string companyCode)
        {
            GetConStringFromDbAction action = new GetConStringFromDbAction(companyCode);
            return action.Execute(EnumDatabase.WorkStation);
        }

        //get the connetction according to DB Type
        public static string GetConStringFromDb(string companyCode, string DbType)
        {
            GetConStringFromDbAction action = new GetConStringFromDbAction(companyCode, DbType);
            return action.Execute(EnumDatabase.WorkStation);
        }
    }
}
