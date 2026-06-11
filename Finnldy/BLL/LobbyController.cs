using Finnldy.DAL;
using Finnldy.DAL.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Finnldy.BLL
{
    public class LobbyController
    {


        Database database = new Database();

        Lobby lobby = new Lobby();
        MovieReposotory movies = new MovieReposotory();
        GetMovies GetMovies = new GetMovies();

        public event Action<NetworkPacket>? NetworkPacketReceived;
        public event Action<string>? NetworkStatusChanged;

        private bool networkEventsRegistered = false;

        public void CreateUser(string Name)
        {
            User user = new User(Name);
            lobby.AddUser(user);
            // Musst es der Datenbank auch geben
        }

        public async Task LoadAllMovies()
        {
            try
            {
                movies.movies = await GetMovies.GetPopularMoviesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Filme: " + ex.Message);
                movies.movies = new List<Movies>();
            }
        
        
        }


        public async Task<List<Movies>> LoadAndFilterMovies( List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies)

        {
            try
            {
                movies.movies = await GetMovies.GetPopularMoviesAsync();

                FilterMovies(wantedGenreIds, wantedLanguages, hideAdultMovies);

                return movies.movies;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden und Filtern der Filme: " + ex.Message);
                movies.movies = new List<Movies>();
                return movies.movies;
            }
        }

        public void FilterMovies( List<int> wantedGenreIds, List<string> wantedLanguages,bool hideAdultMovies)
        {
            if (wantedGenreIds != null && wantedGenreIds.Count > 0)
            {
                movies.movies = movies.movies
                    .Where(movie => movie.GenreIds.Any(genreId => wantedGenreIds.Contains(genreId)))
                    .ToList();
            }

            if (wantedLanguages != null && wantedLanguages.Count > 0)
            {
                movies.movies = movies.movies
                    .Where(movie => wantedLanguages.Contains(movie.OriginalLanguage))
                    .ToList();
            }

            if (hideAdultMovies)
            {
                movies.movies = movies.movies
                    .Where(movie => movie.Adult == false)
                    .ToList();
            }
        }

        public List<Movies> GetFilteredMovies()
        {
            return movies.movies;
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

        public async Task asybStartLobby()
        {
            await LoadAllMovies();
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
