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

        private List<Movies> movies;
        private int currentMovieIndex;

        public SwipeView()
        {
            InitializeComponent();

            currentUser = new User("Mero ist cool");
            swiperContoller = new SwiperContoller();

            movies = new List<Movies>();
            currentMovieIndex = 0;
        }

        private async void SwipeView_Loaded(object sender, RoutedEventArgs e)
        {
            MovieTitleText.Text = "Filme werden geladen...";
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
                MovieDescriptionText.Text = "Es konnten keine Filme geladen werden.";
                MovieCoverImage.Source = null;

                DisableButtons();
                return;
            }

            if (currentMovieIndex >= movies.Count)
            {
                MovieDescriptionText.Text = "Du hast alle Filme geswiped.";
                MovieCoverImage.Source = null;

                DisableButtons();
                return;
            }





            Movies currentMovie = movies[currentMovieIndex];

            MovieTitleText.Text = currentMovie.Name;
            MovieReleaseDateText.Text = currentMovie.ReleaseDate;
            MovieDescriptionText.Text = currentMovie.Description;

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
         
            if (currentMovieIndex >= movies.Count)
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

            Swipe swipe = swiperContoller.DislikeMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void WatchedButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

 

            Swipe swipe = swiperContoller.AddWatchedMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();


            Swipe swipe = swiperContoller.AddWatchLaterMovie(currentUser, currentMovie);

            GoToNextMovie();
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            Movies currentMovie = GetCurrentMovie();

            Swipe swipe = swiperContoller.LikeMovie(currentUser, currentMovie);

            GoToNextMovie();
        }
    }
}
