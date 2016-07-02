using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services
{
    public class UserCredencials
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public UserCredencials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
