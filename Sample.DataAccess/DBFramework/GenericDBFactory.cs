using Sample.DataAccess.Configuration;
using Sample.DataAccess.Utill;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.DBFramework
{
    /// <summary>
    /// This class will hide the Db specific functionality and provide Db resources
    /// through common db interfaces as defined under ADO.NET Common Factory Model.
    /// </summary>
    /// <remarks></remarks>
    public class GenericDBFactory
    {

        /// <summary>
        /// This specifies ADO.NET 2.0 framework to use specified provider classes
        /// to provide requesting database resources
        /// </summary>
        protected const string ProviderInvarientName = "System.Data.SqlClient";

        /// <summary>
        /// Returns the appropriate factory as specified in 'ProviderInvarientName'
        /// to get the database resources. To create Db resources as Commands and Parameters
        /// with generic interface, one should use this Factory.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DbProviderFactory Factory
        {
            get
            {
                DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
                return DbProviderFactories.GetFactory(ProviderInvarientName);
            }
        }

        /// <summary>
        /// To get a new Db connection call this method. Returned connection is not yet opened.
        /// Please make sure that you open it before use and then close it after.
        /// </summary>
        /// <returns>new database connection</returns>
        /// <remarks></remarks>
        public static DbConnection GetConnection(EnumDatabase dbName)
        {
            return GetConnectionByName(dbName);
        }



        private static string GetConnectionStringByName(EnumDatabase dbName)
        {
            string ConnectionString = "";
            if (dbName == EnumDatabase.BOX)
            {
                ConnectionString = EnvironmentVariables.GetBoxConnectionString();
            }
            if (dbName == EnumDatabase.WorkStation)
            {
                ConnectionString = EnvironmentVariables.GetWorkStationConnectionString();
            }
            return ConnectionString;
        }


        private static DbConnection GetConnectionByName(EnumDatabase dbName)
        {
            DbConnection Connection = Factory.CreateConnection();
            Connection.ConnectionString = GetConnectionStringByName(dbName);
            return Connection;
        }

        #region GetConnection overload method

        /// <summary>
        /// To get a new Db connection call this method. Returned connection is not yet opened.
        /// Please make sure that you open it before use and then close it after.
        /// </summary>
        /// <returns>new database connection</returns>
        /// <remarks></remarks>
        public static DbConnection GetConnection(string connectionString)
        {

            DbConnection Connection = Factory.CreateConnection();
            Connection.ConnectionString = connectionString;

            return Connection;
        }
        #endregion

        public static DbConnection GetConnection(EnumDatabase dbName, string companyCode)
        {

            //if (USPRunTimeVariables.GetConnectionSource() == "Config")
            //{
            //    return GetConnectionByName(dbName);
            //}
            //else //Specific to Exceline 
            //{
            //    return GetConnectionByCompanyCode(dbName, companyCode);
            //}
            return GetConnectionByCompanyCode(dbName, companyCode);
        }

        public static string GetConnectionString(EnumDatabase dbName, string companyCode)
        {
            // if (USPRunTimeVariables.GetConnectionSource() == "Config")
            //{
            //    return GetConnectionStringByName(dbName);
            //}
            //else //Specific to Exceline 
            //{
            //    return GetConnectionStringByCompanyCode(dbName, companyCode);
            //}
            return GetConnectionStringByName(dbName);
        }


        private static string GetConnectionStringByCompanyCode(EnumDatabase dbName, string companyCode)
        {
            string ConnectionString = string.Empty;
            string domainCode = companyCode + "_" + dbName;

            if (dbName == EnumDatabase.BOX)
            {
                if (AppDomain.CurrentDomain.GetData(domainCode) == null)
                {
                    ConnectionString = ConnectionStringProvider.GetConStringFromDb(companyCode, dbName.ToString());
                    AppDomain.CurrentDomain.SetData(domainCode, ConnectionString);
                }
                else
                {
                    ConnectionString = AppDomain.CurrentDomain.GetData(domainCode).ToString();
                }
            }
            else if (dbName == EnumDatabase.WorkStation)
            {
                ConnectionString = EnvironmentVariables.GetWorkStationConnectionString();
            }

            return ConnectionString;
        }

        private static DbConnection GetConnectionByCompanyCode(EnumDatabase dbName, string companyCode)
        {
            DbConnection Connection = Factory.CreateConnection();
            Connection.ConnectionString = GetConnectionStringByCompanyCode(dbName, companyCode);
            return Connection;
        }
    }
}
