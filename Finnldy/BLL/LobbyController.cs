using Finnldy.DAL;
using Finnldy.DAL.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace Finnldy.BLL
{
    public class LobbyController
    {
        HostNetworkService networkService = new HostNetworkService();

        Database database = new Database();

        Lobby lobby = new Lobby();
        MovieReposotory movies = new MovieReposotory();
        GetMovies GetMovies = new GetMovies();

        public void CreateUser(string Name)
        {
            User user = new User(Name);
            lobby.AddUser(user);
            // Musst es der Datenbank auch geben
        }
        
        public async void LoadAllMovies()
        {
            
            try
            {
                GetMovies getMovies = new GetMovies();

                movies.movies = await GetMovies.GetPopularMoviesAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Filme: " + ex.Message);
            }
            


        }


        public void FilterMovies()
        {
            if (lobby.UnwantedGenreIds == null || lobby.UnwantedGenreIds.Count == 0)
            {
                return;
            }

            movies.movies = movies.movies
                .Where(movie => !movie.GenreIds.Any(genreId => lobby.UnwantedGenreIds.Contains(genreId)))
                .ToList();
        }


        public async Task<ResponseToAPI> HandleRequest(GetDataFromAPI Data)
        {
            if(Data.Status == "POST")
            {
                if(lobby.IsAtive == false)
                {


                    if (await database.IsUserActive(Data.Username))
                    {
                        User? user = await database.GetUser(Data.Username);

                        if (user != null)
                        {
                            lobby.AddUser(user);
                        }
                    }
                    else
                    {
                        CreateUser(Data.Username);
                    }
                    ResponseToAPI response = new ResponseToAPI(true, null, null);
                    return response;

                }
            }
            if(Data.Status == "GET")
            {

                User? user = await database.GetUser(Data.Username);

                if(user == null)
                {
                    return new ResponseToAPI(false, null, null);
                }
                Movies movie = movies.FindMovieById(Data.Movie_id.Value);


                switch (Data.Action)
                {
                    case GetDataFromAPI.action.Liked: 
                        user.LikeMovie(movie); 
                        break;
                    case GetDataFromAPI.action.Disliked:
                        user.DislikeMovie(movie);
                        break;
                    case GetDataFromAPI.action.Watchlater:
                        user.AddWatchLaterMovie(movie);
                        break;
                    case GetDataFromAPI.action.AlreadyWatched:
                        user.AddWatchedMovie(movie);
                        break;
                }


                ResponseToAPI response = new ResponseToAPI(true, GetNextMovie(movie), null);
                return response;


            }
            ResponseToAPI responseend = new ResponseToAPI(false, null, null);
            return responseend;
        }



        public async void AddUser(string Name)
        {
            lobby.AddUser(await database.GetUser(Name));
        }

        public void CreateLobby (string Name)
        {
            if(lobby != null)
            {
                return;
            }

            
            


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

        public Movies GetNextMovie(Movies movie)
        {
            int index = movies.movies.IndexOf(movie);

            if (index == -1)
            {
                return null;
            }

            if (index + 1 < movies.movies.Count)
            {
                return movies.movies[index + 1]; 
            }

            return null; 
        }
    }
}
