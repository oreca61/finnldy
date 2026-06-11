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

        private List<Movies> movies;
        private int currentMovieIndex;

        private List<int> wantedGenreIds;
        private List<string> wantedLanguages;
        private bool hideAdultMovies;


        public SwipeView(User user, List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies)
        {
            InitializeComponent();

            currentUser = user;

            this.wantedGenreIds = wantedGenreIds;
            this.wantedLanguages = wantedLanguages;
            this.hideAdultMovies = hideAdultMovies;

            swiperContoller = new SwiperContoller();
            lobbyController = new LobbyController();

            movies = new List<Movies>();
            currentMovieIndex = 0;
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
                movies = await lobbyController.LoadAndFilterMovies(
                    wantedGenreIds,
                    wantedLanguages,
                    hideAdultMovies
                );

                currentMovieIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Filme: " + ex.Message);
                movies = new List<Movies>();
            }
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

        private void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.DislikeMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void WatchedButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.AddWatchedMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.AddWatchLaterMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            if (currentMovie == null)
            {
                return;
            }

            Swipe swipe = swiperContoller.LikeMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
