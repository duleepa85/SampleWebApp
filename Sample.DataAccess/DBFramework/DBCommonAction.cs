using Sample.DataAccess.Utill;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.DBFramework
{
    public abstract class DBCommonAction<T>
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

        protected DbConnection connection;
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
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public T Execute(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.ConnectionString = connectionString;

            try
            {
                connection.Open();
                return Body(connection);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error on data. " + ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }


        public T Execute(string workstationConnectionString, string domainCode)
        {
            GetConnectionString(EnumDatabase.BOX, workstationConnectionString, domainCode);
            connection = new SqlConnection(_connectionString);
            connection.ConnectionString = _connectionString;

            try
            {
                connection.Open();
                return Body(connection);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error on data. " + ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// This will create a command which is assocoiated with this db Action's
        /// Connection. Make sure calls to this method happens only withing the
        /// overidden Body() method of this action
        /// </summary>
        /// <param name="type">CommandType of the new Command</param>
        /// <param name="commandText">CommandText of the new Command</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected DbCommand CreateCommand(CommandType type, string commandText)
        {
            DbCommand command = GenericDBFactory.Factory.CreateCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = type;
            command.CommandTimeout = int.MaxValue;
            return command;
        }
    }
}
