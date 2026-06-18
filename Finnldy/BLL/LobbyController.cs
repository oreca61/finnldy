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


        Database database = new Database();

        Lobby lobby = new Lobby();
        MovieReposotory movies = new MovieReposotory();
        GetMovies GetMovies = new GetMovies();

        private ResultController resultController = new ResultController();
        public event Action<NetworkPacket>? NetworkPacketReceived;
        public event Action<string>? NetworkStatusChanged;

        private readonly Dictionary<int, HashSet<string>> likedUsersByMovie = new Dictionary<int, HashSet<string>>();

        // schaus dir ncohmal an könnte proboleme machen(lass hoffen nciht)
        private bool networkEventsRegistered = false;



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

        }
        public void Moviesfüllen(List<Movies> movieList)
        {
            movies.movies = movieList;
        }

        public void FilterMovies(List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies)
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

        }
        public async Task<ResponseToAPI> HandleRequest(GetDataFromAPI Data)
        {
            if (Data.Status == "POST")
            {
                if (lobby.IsAtive == false)
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
            if (Data.Status == "GET")
            {

                User? user = await database.GetUser(Data.Username);

                if (user == null)
                {
                    User? newUser = await database.CreateUser(Data.Username);
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

        public async Task<bool> Userkontrolle(string username)
        {
            User? user =await database.GetUser(username);

            if(user !=null)
            {
                lobby.AddUser(user);
                return true ;

            }

            user = await database.CreateUser(username);
            if(user ==null)
            {
                return false;

            }

            lobby.AddUser(user);

            return true;


        }



        public async void AddUser(string Name)
        {
            lobby.AddUser(await database.GetUser(Name));
        }

        public void CreateLobby(string Name)
        {
            if (lobby != null)
            {
                return;
            }


        }

        private GetDataFromAPI? ConvertPacketToGetDataFromAPI(NetworkPacket packet)
        {
            GetDataFromAPI.action? action = ConvertSwipeType(packet.SwipeType);

            if (action == null)
            {
                return null;
            }

            return new GetDataFromAPI(
                "GET",
                packet.MovieId,
                packet.Username,
                action
            );
        }

        private GetDataFromAPI.action? ConvertSwipeType(SwipeType swipeType)
        {
            switch (swipeType)
            {
                case SwipeType.Like:
                    return GetDataFromAPI.action.Liked;

                case SwipeType.Dislike:
                    return GetDataFromAPI.action.Disliked;

                case SwipeType.WatchLater:
                    return GetDataFromAPI.action.Watchlater;

                case SwipeType.Watched:
                    return GetDataFromAPI.action.AlreadyWatched;

                default:
                    return null;
            }
        }


        // KI Anfang
        // Chat
        // kannst du mir diese Methode verbessern


        private async Task ProcessNetworkPacket(NetworkPacket packet)
        {
            if (packet.Type == "lobbySettings")
            {
                NetworkPacketReceived?.Invoke(packet);
                return;
            }

            if (packet.Type == "match")
            {
                NetworkPacketReceived?.Invoke(packet);
                return;
            }

            if (packet.Type != "swipe")
            {
                return;
            }

            Movies movieForResult = movies.FindMovieById(packet.MovieId);

            if (movieForResult != null)
            {
                AddSwipeToResult(movieForResult, packet.SwipeType, packet.Username);
            }



            GetDataFromAPI? data = ConvertPacketToGetDataFromAPI(packet);

            if (data == null)
            {
                NetworkStatusChanged?.Invoke("NetworkPacket konnte nicht umgewandelt werden.");
                return;
            }

            if (NetworkSession.IsHost)
            {
                ResponseToAPI response = await HandleRequest(data);

                if (response.Status)
                {
                    NetworkStatusChanged?.Invoke("Swipe wurde mit HandleRequest verarbeitet.");
                }
                else
                {
                    NetworkStatusChanged?.Invoke("HandleRequest konnte den Swipe nicht verarbeiten.");
                }

                if (packet.SwipeType == SwipeType.Like)
                {
                    await CheckForMatch(packet);
                }
            }

            NetworkPacketReceived?.Invoke(packet);
        }

        
        // Ki Ende
        //KI ANFAG
        // Chat
        // bitte hilf mir empfangene NetworkPackets im LobbyController zu unterscheiden

        public void RegisterNetworkEvents()
        {
            if (networkEventsRegistered)
            {
                return;
            }

            NetworkSession.Host.PacketReceived += async packet =>
            {
                await ProcessNetworkPacket(packet);
            };

            NetworkSession.Client.PacketReceived += async packet =>
            {
                await ProcessNetworkPacket(packet);
            };

            NetworkSession.Host.StatusChanged += message =>
            {
                NetworkStatusChanged?.Invoke(message);
            };

            NetworkSession.Client.StatusChanged += message =>
            {
                NetworkStatusChanged?.Invoke(message);
            };

            networkEventsRegistered = true;
        }

        public async Task SendLobbySettingsAsync(
         List<int> wantedGenreIds,
         List<string> wantedLanguages,
         bool hideAdultMovies)
        {
            NetworkPacket packet = new NetworkPacket
            {
                Type = "lobbySettings",
                Username = NetworkSession.Username,
                WantedGenreIds = wantedGenreIds,
                WantedLanguages = wantedLanguages,
                HideAdultMovies = hideAdultMovies,
                Time = DateTime.Now
            };

            await NetworkSession.Host.SendToAllAsync(packet);
        }


        // KI Ende

        public async Task SendSwipeOverNetwork(Movies movie, SwipeType swipeType)
        {
            NetworkPacket packet = new NetworkPacket
            {
                Type = "swipe",
                Username = NetworkSession.Username,
                MovieId = movie.ApiMovieId,
                MovieTitle = movie.Name,
                SwipeType = swipeType,
                Time = DateTime.Now
            };

            if (NetworkSession.IsHost)
            {
                if (swipeType == SwipeType.Like)
                {
                    await CheckForMatch(packet);
                }

                await NetworkSession.Host.SendToAllAsync(packet);
            }
            else if (NetworkSession.IsClient)
            {
                await NetworkSession.Client.SendAsync(packet);
            }
            else
            {
                NetworkStatusChanged?.Invoke("Nicht verbunden. Swipe wurde nicht gesendet.");
            }
        }




        private async Task CheckForMatch(NetworkPacket packet)
        {
            if (packet.SwipeType != SwipeType.Like)
            {
                return;
            }

            if (!likedUsersByMovie.ContainsKey(packet.MovieId))
            {
                likedUsersByMovie[packet.MovieId] = new HashSet<string>();
            }

            likedUsersByMovie[packet.MovieId].Add(packet.Username);

            if (likedUsersByMovie[packet.MovieId].Count >= 2)
            {
                NetworkPacket matchPacket = new NetworkPacket
                {
                    Type = "match",
                    Username = "Host",
                    MovieId = packet.MovieId,
                    MovieTitle = packet.MovieTitle,
                    MatchFound = true,
                    Time = DateTime.Now
                };

                NetworkPacketReceived?.Invoke(matchPacket);

                if (NetworkSession.IsHost)
                {
                    await NetworkSession.Host.SendToAllAsync(matchPacket);
                }
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

        public void AddSwipeToResult(Movies movie, SwipeType swipeType, string username)
        {
            resultController.AddSwipeToResult(movie, swipeType, username);
        }

        public List<Result> GetFinalResults()
        {
            return resultController.GetBestResults();
        }




        

    }
}
