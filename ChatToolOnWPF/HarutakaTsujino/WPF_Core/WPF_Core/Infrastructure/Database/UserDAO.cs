using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class UserDAO
    {
        public static DataTable Get(string mailAddress)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM m_user WHERE mail_address = {MAIL_ADDRESS_PARAM_NAME};";

            var mailAddressParam = cmd.CreateParameter();
            mailAddressParam.ParameterName = MAIL_ADDRESS_PARAM_NAME;
            mailAddressParam.MySqlDbType = MySqlDbType.VarChar;
            mailAddressParam.Direction = ParameterDirection.Input;
            mailAddressParam.Value = mailAddress;
            cmd.Parameters.Add(mailAddressParam);

            var dataAdapter = new MySqlDataAdapter(cmd);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            mySqlConnection.Close();

            return dataSet.Tables[0];
        }

        public static void ChangeOnlineState(int id, bool isOnline)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            var onlineStateCode = isOnline ? 1 : 0;

            var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"UPDATE m_user SET is_online = {onlineStateCode} WHERE(id = {id});";
            var dataAdapter = new MySqlDataAdapter(cmd);

            mySqlConnection.Close();
        }

        private const string MAIL_ADDRESS_PARAM_NAME = "@mail_address";
    }
}
