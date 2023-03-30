using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.Configuration
{
    public static class EnvironmentVariables
    {
        public static string GetWorkStationConnectionString()
        {
            try
            {
                string conString = Environment.GetEnvironmentVariable("WorkstationDbConnection");
                if (string.IsNullOrEmpty(conString))
                {
                    throw new Exception("Workstation onnection string is empty.");
                }
                return conString;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetBoxConnectionString()
        {
            try
            {
                string conString = Environment.GetEnvironmentVariable("BoxDbConnection");
                if (string.IsNullOrEmpty(conString))
                {
                    throw new Exception("BoxDb connection string is empty.");
                }
                return conString;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
