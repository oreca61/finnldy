using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Finnldy.DAL.Database
{
    public class Database
    {

        public User GetUser(string name)
        {
            // TODO
            User user = new User("");
            return user;

            
        }

        public bool IsUserActive(string name)
        {
            // TODO
            User user = new User("");
            return true;


        }

        public void AddToWatched(User user, Movies movie)
        {

            string query = "INSERT INTO Watched (?, ?, ?) VALUES (UserName, Title, WatchedAt);";
        }
        

        private void ConnectToDatabase(String query)
        {

        }
    }
}
