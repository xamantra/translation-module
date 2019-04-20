using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TranslationModule_v2.Database
{
    internal static class Query
    {
        public static DataTable ExecuteMySql(string query)
        {
            var dataTable = new DataTable();

            if (Connection.CanConnectToMySql())
            {
                Connection.OpenMySql();
                try
                {
                    var command = Connection.mySqlConnection.CreateCommand();
                    command.CommandText = query;
                    var adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Connection.CloseMySql();
                }
            }
            else
            {
                Console.WriteLine("Can't connect to MySql.");
            }

            return dataTable;
        }

        public static DataTable SqlServerQuery(string query)
        {
            DataTable dataTable = null;

            if (Connection.CanConnectToSqlServer())
            {
                Connection.OpenSqlServer();
                try
                {
                    var command = Connection.sqlConnection.CreateCommand();
                    command.CommandText = query;
                    var adapter = new SqlDataAdapter(command);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                }
                finally
                {
                    Connection.CloseSqlServer();
                }
            }
            else
            {
                Console.WriteLine("Can't connect to SqlServer.");
            }

            return dataTable;
        }
    }
}