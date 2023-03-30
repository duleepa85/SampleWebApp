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
    public abstract class AbstractDataAction<T>
    {
        /// <summary>
        /// This connection string will be used through out the all application 
        /// and It will be maintain under Runtime Constant.
        /// </summary>
        private string _connectionString;  // Sample "Data Source=192.168.100.2;Initial Catalog=Procasso4;uid=sa;pwd=Sql2005";
        /// <summary>
        /// This Method should implement by the sub class, which will be creating on purpose 
        /// of data action.
        /// </summary>
        /// <param name="conection"></param>
        /// <returns></returns>
        protected abstract T Body(DbConnection conection);

        [Obsolete("Do not use this method for newer implementations")]
        protected void OverrideConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void GetConnectionString(EnumDatabase dbType, string connectionString, string companyCode)
        {
            _connectionString = new GetConnectionStringByDomainCode(dbType, companyCode, connectionString).Execute();
        }

        /// <summary>
        /// This methos will return result of execution of the Data action provided 
        /// by the Sub class which implement this class.
        /// </summary>
        /// <returns></returns>
        //public T Execute(string connectionString)
        //{
        //    DbConnection connection = new SqlConnection(connectionString);
        //    connection.ConnectionString = connectionString;

        //    try
        //    {
        //        connection.Open();
        //        return Body(connection);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        public T Execute(string workstationConnectionString, string domainCode)
        {
            GetConnectionString(EnumDatabase.BOX, workstationConnectionString, domainCode);
            DbConnection connection = new SqlConnection(_connectionString);
            connection.ConnectionString = _connectionString;

            try
            {
                connection.Open();
                return Body(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public T Execute(string domainCode)
        {
            string workstationConnectionString = EnvironmentVariables.GetWorkStationConnectionString();
            GetConnectionString(EnumDatabase.BOX, workstationConnectionString, domainCode);
            DbConnection connection = new SqlConnection(_connectionString);
            connection.ConnectionString = _connectionString;

            try
            {
                connection.Open();
                return Body(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public T Execute(EnumDatabase dbType, string workstationConnectionString, string domainCode)
        {
            GetConnectionString(dbType, workstationConnectionString, domainCode);
            DbConnection connection = new SqlConnection(_connectionString);
            connection.ConnectionString = _connectionString;

            try
            {
                connection.Open();
                return Body(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
