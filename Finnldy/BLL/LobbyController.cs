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


        public ResponseToAPI HandleRequest(GetDataFromAPI Data)
        {
            if(Data.Status == "POST")
            {
                if(lobby.IsAtive == false)
                {
                    bool check = database.IsUserActive(Data.Username);

                    if(check == true)
                    {
                        User user = database.GetUser(Data.Username);
                        lobby.AddUser(user);
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

                if(Data.Action == GetDataFromAPI.action.Liked)
                {
                    User user = database.GetUser(Data.Username);
                    Movies movie = movies.FindMovieById(Data.Movie_id.Value);

                    user.LikeMovie(movie);

                    ResponseToAPI response = new ResponseToAPI(true, GetNextMovie(movie), null);
                    return response;
                    
                }
                if (Data.Action == GetDataFromAPI.action.Disliked)
                {
                    User user = database.GetUser(Data.Username);
                    Movies movie = movies.FindMovieById(Data.Movie_id.Value);

                    user.DislikeMovie(movie);

                    ResponseToAPI response = new ResponseToAPI(true, GetNextMovie(movie), null);
                    return response;

                }
                if (Data.Action == GetDataFromAPI.action.Watchlater)
                {
                    User user = database.GetUser(Data.Username);
                    Movies movie = movies.FindMovieById(Data.Movie_id.Value);

                    user.AddWatchLaterMovie(movie);

                    ResponseToAPI response = new ResponseToAPI(true, GetNextMovie(movie), null);
                    return response;

                }
                if (Data.Action == GetDataFromAPI.action.AlreadyWatched)
                {
                    User user = database.GetUser(Data.Username);
                    Movies movie = movies.FindMovieById(Data.Movie_id.Value);

                    user.AddWatchedMovie(movie);

                    movies.movies.Remove(movie);
                    ResponseToAPI response = new ResponseToAPI(true, GetNextMovie(movie), null);
                    return response;

                }

            }
            ResponseToAPI responseend = new ResponseToAPI(false, null, null);
            return responseend;
        }



        public void AddUser(string Name)
        {
            lobby.AddUser(database.GetUser(Name));
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
