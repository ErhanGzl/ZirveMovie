using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace ZirveMovie.ORM.PureAdo
{
    public static class SqlClientUtility
    {
        public static string connectionStringName=ConnectionString.MovieConnectionString;
        public static DataTable ExecuteDataTable( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
           string cnstr =connectionStringName;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(cnstr))
                {
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        return SqlClientUtility.CreateDataTable(command);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionStringName), commandType, commandText, parameters))
                    return SqlClientUtility.CreateDataTable(command);
            }
        }

        public static DataSet ExecuteDataSet( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
           string cnstr =connectionStringName;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(cnstr))
                {
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        return SqlClientUtility.CreateDataSet(command);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionStringName), commandType, commandText, parameters))
                    return SqlClientUtility.CreateDataSet(command);
            }
        }

        public static string ExecuteJson( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            SqlDataReader sqlDataReader = SqlClientUtility.ExecuteReader( commandType, commandText, parameters);
            while (sqlDataReader.Read())
            {
                stringBuilder.Append("{");
                for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
                {
                    if (sqlDataReader.GetFieldType(ordinal) == typeof(DateTime))
                        stringBuilder.AppendFormat("\"{0}\":\"{1:u}\"", (object)sqlDataReader.GetName(ordinal), sqlDataReader.GetValue(ordinal));
                    else
                        stringBuilder.AppendFormat("\"{0}\":\"{1}\"", (object)sqlDataReader.GetName(ordinal), sqlDataReader.GetValue(ordinal));
                    if (ordinal < sqlDataReader.FieldCount - 1)
                        stringBuilder.Append(",");
                }
                stringBuilder.Append("},");
                ++num;
            }
            if (stringBuilder.Length > 1)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            if (num == 1)
                return stringBuilder.ToString();
            if (num > 1)
                return "[" + stringBuilder.ToString() + "]";
            return (string)null;
        }

        public static int ExecuteNonQuery( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
           string cnstr =connectionStringName;
            int res = 0;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(cnstr))
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionStringName), commandType, commandText, parameters))
                    res = command.ExecuteNonQuery();
            }
            return res;
        }

        public static SqlDataReader ExecuteReader( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
           string cnstr =connectionStringName;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(new SqlConnection(cnstr), commandType, commandText, parameters))
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionStringName), commandType, commandText, parameters))
                    return command.ExecuteReader();
            }
        }

        public static int ExecuteScalar( CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
           string cnstr =connectionStringName;
            object res;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(new SqlConnection(cnstr), commandType, commandText, parameters))
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionStringName), commandType, commandText, parameters))
                    res = command.ExecuteScalar();
            }
            return Convert.ToInt32(res);
        }

        private static object CheckValue(object value)
        {
            if (value == null)
                return (object)DBNull.Value;
            return value;
        }

        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = connection.ConnectionTimeout;
            return sqlCommand;
        }

        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            //if (connection != null && connection.State == ConnectionState.Closed)
            //connection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = connection.ConnectionTimeout;
            if (parameters != null)
            {
                foreach (SqlParameter sqlParameter in parameters)
                {
                    sqlParameter.Value = SqlClientUtility.CheckValue(sqlParameter.Value);
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            //connection.Close();
            return sqlCommand;
        }

        private static DataSet CreateDataSet(SqlCommand command)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        private static DataTable CreateDataTable(SqlCommand command)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        private static SqlConnection GetTransactedSqlConnection(string connectionStringName)
        {
            string cnstr = connectionStringName;
            LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("ConnectionDictionary");
            Dictionary<string, SqlConnection> dictionary = (Dictionary<string, SqlConnection>)Thread.GetData(namedDataSlot);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, SqlConnection>();
                Thread.SetData(namedDataSlot, (object)dictionary);
            }
            SqlConnection sqlConnection;
            if (dictionary.ContainsKey(connectionStringName))
            {
                sqlConnection = dictionary[connectionStringName];
            }
            else
            {
                sqlConnection = new SqlConnection(cnstr);
                dictionary.Add(connectionStringName, sqlConnection);
                Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler(SqlClientUtility.Current_TransactionCompleted);
            }
            return sqlConnection;
        }

        private static void Current_TransactionCompleted(object sender, TransactionEventArgs e)
        {
            Dictionary<string, SqlConnection> dictionary = (Dictionary<string, SqlConnection>)Thread.GetData(Thread.GetNamedDataSlot("ConnectionDictionary"));
            if (dictionary != null)
            {
                foreach (SqlConnection sqlConnection in dictionary.Values)
                {
                    if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                        sqlConnection.Close();
                }
                dictionary.Clear();
            }
            Thread.FreeNamedDataSlot("ConnectionDictionary");
        }
    }
}
