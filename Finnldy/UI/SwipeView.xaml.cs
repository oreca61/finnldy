using Finnldy.BLL;
using Finnldy.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Finnldy.UI
{
    /// <summary>
    /// Interaction logic for SwipeView.xaml
    /// </summary>
    public partial class SwipeView : Window
    {
        private User currentUser;
        private SwiperContoller swiperContoller;
        private LobbyController lobbyController;
        private ISwipeApiService fastApiService;


        private List<Movies> movies;
        private int currentMovieIndex;

        private List<int> wantedGenreIds;
        private List<string> wantedLanguages;
        private bool hideAdultMovies;


        private int maxSwipes;
        private int swipeCounter;

        public SwipeView(User user, List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies, int maxSwipes = 50)
        {
            InitializeComponent();

            currentUser = user;

            this.wantedGenreIds = wantedGenreIds;
            this.wantedLanguages = wantedLanguages;
            this.hideAdultMovies = hideAdultMovies;
            this.maxSwipes = maxSwipes;
            
            
            swipeCounter = 0;

            swiperContoller = new SwiperContoller();

            fastApiService = new FastApiService();

            lobbyController = NetworkSession.LobbyController;

            movies = new List<Movies>();

            currentMovieIndex = 0;

            NetworkSession.LobbyController.NetworkPacketReceived += OnNetworkPacketReceived;
        }

        private void OnNetworkPacketReceived(NetworkPacket packet)
        {
            if (packet.Type == "match")
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        "Match! Ihr wollt beide \"" + packet.MovieTitle + "\" schauen.",
                        "Gemeinsamer Film gefunden"
                    );
                });

                return;
            }

            if (packet.Type != "swipe")
            {
                return;
            }

            if (packet.SwipeType == SwipeType.Watched)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    packet.Username + " hat \"" + packet.MovieTitle + "\" " + packet.SwipeType,
                    "Swipe empfangen"
                );
            });
        }



        private async void SwipeView_Loaded(object sender, RoutedEventArgs e)
        {
            MovieTitleText.Text = "Filme werden geladen...";
            MovieReleaseDateText.Text = "";
            MovieDescriptionText.Text = "";
            MovieCoverImage.Source = null;

            await LoadMoviesFromApi();

            ShowCurrentMovie();
        }

        private async Task LoadMoviesFromApi()
        {
            try
            {
                GetMovies getMovies = new GetMovies();

                movies = await getMovies.GetPopularMoviesAsync();

                if (wantedGenreIds != null && wantedGenreIds.Count > 0)
                {
                    movies = movies
                        .Where(movie => movie.GenreIds.Any(genreId => wantedGenreIds.Contains(genreId)))
                        .ToList();
                }

                if (wantedLanguages != null && wantedLanguages.Count > 0)
                {
                    movies = movies
                        .Where(movie => wantedLanguages.Contains(movie.OriginalLanguage))
                        .ToList();
                }

                if (hideAdultMovies)
                {
                    movies = movies
                        .Where(movie => movie.Adult == false)
                        .ToList();
                }


                //KI ANfang
                // Chat
                // Kannst du meine Sortier methode verbessern?
                // ich hatte davor eine ganze methode die leider nciht funktoniert hat deswegen habe ich chat gefragt und hat sich hereausgestellt dass an nur 4 Zeilen braucht GGs
                movies = movies
                .OrderByDescending(movie =>  movie.Popularity)
                .ThenBy(movie => movie.ApiMovieId)

                .ToList();

                NetworkSession.LobbyController.Moviesfüllen(movies);

                currentMovieIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Filme: " + ex.Message);
                movies = new List<Movies>();
            }
            // KI ende
        }

        private void ShowCurrentMovie()
        {
            if (movies == null || movies.Count == 0)
            {
                MovieTitleText.Text = "Keine Filme gefunden";
                MovieReleaseDateText.Text = "";
                MovieDescriptionText.Text = "Es konnten keine Filme geladen werden oder alle Filme wurden durch deine Genre-Auswahl herausgefiltert.";
                MovieCoverImage.Source = null;

                DisableButtons();
                return;
            }

            if (currentMovieIndex >= movies.Count)
            {
                MovieTitleText.Text = "Fertig";
                MovieReleaseDateText.Text = "";
                MovieDescriptionText.Text = "Du hast alle Filme geswiped.";
                MovieCoverImage.Source = null;

                DisableButtons();
                return;
            }

            Movies currentMovie = movies[currentMovieIndex];


            MovieTitleText.Text = currentMovie.Name;
            MovieReleaseDateText.Text = "📅 " + currentMovie.ReleaseDate;
            MovieDescriptionText.Text = currentMovie.Description;

            MovieRatingText.Text = "⭐ " + currentMovie.VoteAverage.ToString("0.0") + " / 10";
            MovieLanguageText.Text = "🌍 " + currentMovie.OriginalLanguage.ToUpper();

            if (currentMovie.Adult)
            {
                MovieAdultText.Text = "🔞 18+";
            }
            else
            {
                MovieAdultText.Text = "👨‍👩‍👧 Nicht 18+";
            }


            try
            {
                if (!string.IsNullOrEmpty(currentMovie.Cover))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(currentMovie.Cover, UriKind.Absolute);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    MovieCoverImage.Source = image;
                }
                else
                {
                    MovieCoverImage.Source = null;
                }
            }
            catch
            {
                MovieCoverImage.Source = null;
            }
        }

        private Movies GetCurrentMovie()
        {
            if (movies == null || currentMovieIndex >= movies.Count)
            {
                return null;
            }

            return movies[currentMovieIndex];
        }

        private void GoToNextMovie()
        {
            currentMovieIndex++;
            ShowCurrentMovie();
        }

        private void DisableButtons()
        {
            WatchedButton.IsEnabled = false;
            DislikeButton.IsEnabled = false;
            LikeButton.IsEnabled = false;
            WatchLaterButton.IsEnabled = false;
        }








        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                AppLogger.Error("Like geklickt, aber currentMovie war null.");
                return;
            }

            AppLogger.Info(currentUser.Name + " liked Film: " + currentMovie.Name);

            Swipe swipe = swiperContoller.LikeMovie(currentUser, currentMovie);

            SaveSwipeInBackground(currentMovie, SwipeType.Like);

            await NetworkSession.LobbyController.SendSwipeOverNetwork(currentMovie, SwipeType.Like);

            FinishSwipeAndCheckResult(currentMovie, SwipeType.Like);
        }



        private async void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.DislikeMovie(currentUser, currentMovie);

            SaveSwipeInBackground(currentMovie, SwipeType.Dislike);

            await NetworkSession.LobbyController.SendSwipeOverNetwork(currentMovie, SwipeType.Dislike);

            FinishSwipeAndCheckResult(currentMovie, SwipeType.Dislike);
        }



        private async void WatchedButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.AddWatchedMovie(currentUser, currentMovie);

            SaveSwipeInBackground(currentMovie, SwipeType.Watched);

            await NetworkSession.LobbyController.SendSwipeOverNetwork(currentMovie, SwipeType.Watched);

            FinishSwipeAndCheckResult(currentMovie, SwipeType.Watched);
        }



        private async void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.AddWatchLaterMovie(currentUser, currentMovie);

            SaveSwipeInBackground(currentMovie, SwipeType.WatchLater);

            await NetworkSession.LobbyController.SendSwipeOverNetwork(currentMovie, SwipeType.WatchLater);

            FinishSwipeAndCheckResult(currentMovie, SwipeType.WatchLater);
        }




        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Ki Hilfe
        // mach mir eine Methode um nach jedem Swipe zu überprüfen ob das Ergebnis angezeigt werden soll oder zum nächsten Film gegangen wird

        private void FinishSwipeAndCheckResult(Movies movie, SwipeType swipeType)
        {
            NetworkSession.LobbyController.AddSwipeToResult(movie, swipeType, currentUser.Name);

            swipeCounter++;

            AppLogger.Info("Swipe " + swipeCounter + " von " + maxSwipes + ": "
                           + currentUser.Name + " -> " + swipeType + " -> " + movie.Name);

            if (swipeCounter >= maxSwipes)
            {
                DisableButtons();

                AppLogger.Info("Maximale Swipe-Anzahl erreicht. Ergebnis wird angezeigt.");

                List<Result> results = NetworkSession.LobbyController.GetFinalResults();

                ResultView resultView = new ResultView(results);
                resultView.Show();

                this.Close();
                return;
            }

            GoToNextMovie();
        }

        // Chatgbt Hilfsmethode um den Swipe im Hintergrund zu speichern damit die UI schneller reagiert

        private async void SaveSwipeInBackground(Movies movie, SwipeType swipeType)
        {
            await fastApiService.SaveSwipeAsync(currentUser, movie, swipeType);
        }
    }
}
