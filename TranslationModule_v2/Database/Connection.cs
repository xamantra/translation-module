using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

namespace TranslationModule_v2.Database
{
    internal static class Connection
    {
        #region MySql Methods

        public static MySqlConnection mySqlConnection;

        public static void OpenMySql()
        {
            if (mySqlConnection == null)
            {
                var connString = Config.MySqlConnString;
                mySqlConnection = new MySqlConnection
                {
                    ConnectionString = connString
                };
                mySqlConnection.Open();
            }
        }

        public static void CloseMySql()
        {
            if (mySqlConnection != null)
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
                mySqlConnection = null;
            }
        }

        public static bool CanConnectToMySql()
        {
            var canConnect = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection())
                {
                    connection.ConnectionString = Config.MySqlConnString;
                    connection.Open();
                    connection.Close();
                    canConnect = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                canConnect = false;
            }

            return canConnect;
        }

        #endregion

        #region SqlServer Methods

        public static SqlConnection sqlConnection;

        public static void OpenSqlServer()
        {
            if (sqlConnection == null)
            {
                sqlConnection = new SqlConnection
                {
                    ConnectionString = Config.SqlServerConnString
                };
                sqlConnection.Open();
            }
        }

        public static void CloseSqlServer()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
                sqlConnection = null;
            }
        }

        public static bool CanConnectToSqlServer()
        {
            var canConnect = false;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Config.MySqlConnString;
                try
                {
                    connection.Open();
                    connection.Close();
                    canConnect = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    canConnect = false;
                }

                connection.Dispose();
            }

            return canConnect;
        }

        #endregion
    }
}