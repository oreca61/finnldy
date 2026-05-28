using Finnldy.DAL;
using Finnldy.DAL.Database;
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
            // Musst es der Datenbank auch geben
        }
        
        public void LoadAllMovies()
        {
            
        }

        

        public ResponseToAPI HandleRequest(string status, int? movie_id, string? username)
        {
            if(status == "POST")
            {
                if(lobby.IsAtive == false)
                {
                    bool check = database.IsUserActive(username);

                    if(check == true)
                    {
                        User user = database.GetUser(username);
                        lobby.AddUser(user);
                    }
                    else
                    {
                        CreateUser(username);
                    }
                    ResponseToAPI response = new ResponseToAPI(true, null, null);
                    return response;

                }
            }
            if(status == "GET")
            {

            }
        }

        public string Lobbycode()
        {
            
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
