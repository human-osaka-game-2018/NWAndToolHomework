using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WPF_Core.Infrastructure.Database
{
    public static class Connection
    {
        static Connection()
        {
            FetchConnectionStrings();
        }

        public static MySqlConnection Connect()
        {
            var mySqlConnection = new MySqlConnection(ConnectionStrings);

            return mySqlConnection;
        }

        private static void FetchConnectionStrings()
        {
            var runMode = ConfigurationManager.AppSettings["RunMode"];
            ConnectionStrings = ConfigurationManager.ConnectionStrings["Default."+runMode].ConnectionString;
        }

        private static string ConnectionStrings { get; set; }
    }
}
