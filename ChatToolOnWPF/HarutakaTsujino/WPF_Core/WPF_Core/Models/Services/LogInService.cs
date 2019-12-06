using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class LogInService
    {
        public static User LogInUser { get; private set; }

        public static bool LogIn(string mailAddress, string password)
        {
            var userDataTable = UserDAO.Get(mailAddress);

            if (ExtractLogInUser(mailAddress, password, userDataTable))
            {
                UserDAO.ChangeOnlineState(LogInUser.Id, true);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LogOut()
        {
            if (LogInUser != null)
            {
                UserDAO.ChangeOnlineState(LogInUser.Id, false);

                LogInUser = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ExtractLogInUser(string mailAddress, string password, DataTable userDataTable)
        {
            foreach (DataRow userData in userDataTable.Rows)
            {
                var thisUserMailAddress = (string)userData["mail_address"];
                var thisUserPassword = (string)userData["password"];

                if (mailAddress == thisUserMailAddress)
                {
                    if (password == thisUserPassword)
                    {
                        LogInUser = new User(
                            (int)userData["id"],
                            thisUserMailAddress,
                            thisUserPassword,
                            (string)userData["name"]);

                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            return false;
        }
    }
}
