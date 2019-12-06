using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_Core.Models.DomainObjects
{
    class User
    {
        public User(int id, string mailAddress, string password, string name)
        {
            Id = id;

            Mail_address = mailAddress;

            Password = password;
            
            Name = name;
        }

        public int Id { get; private set; }

        public string Mail_address { get; private set; }

        public string Password { get; private set; }
        
        public string Name { get; private set; }
    }
}
