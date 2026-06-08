using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class Lobby
    {
        private List<User> Users = new List<User>();
        public void AddUser(User user) {
            Users.Add(user);
        }
        public bool IsAtive = false;

        public string Lobbycode;

        public string Unwanted;
    }
}
