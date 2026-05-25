using Finnldy.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class LobbyController
    {
        HostNetworkService networkService = new HostNetworkService();

        Database database = new Database();

        Lobby lobby = new Lobby();
        MovieReposotory movies = new MovieReposotory();

        public void CreateUser(string Name)
        {
            User user = new User(Name);
            lobby.AddUser(user);
        }
        
        public void LoadAllMovies()
        {
            //TODO
        }

        public string Lobbycode()
        {
            // TODO
            return "unfinished";
        }

        public void AddUser(string Name)
        {
            lobby.AddUser(database.GetUser(Name));
        }

        public void CreateLobby (string Name)
        {
            if(lobby == null)
            {
                return;
            }

            lobby.Lobbycode = Lobbycode();
            


        }

        public void StartLobby()
        {
            LoadAllMovies();
        }


        public void RefreshLobby(string Name)
        {
            Lobby Lobbynew = new Lobby();

            lobby = Lobbynew;
        }
    }
}
