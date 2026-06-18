using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Finnldy.UI
{
    public partial class ResultView : Window
    {
        private List<Result> results;

        public ResultView(List<Result> results)
        {
            InitializeComponent();

            this.results = results;

            ShowResults();
        }

        private void ShowResults()
        {
            if (results == null || results.Count == 0)
            {
                WinnerTitleText.Text = "Kein Ergebnis gefunden";

                WinnerScoreText.Text = "-";
                WinnerLikesText.Text = "-";
                WinnerDislikesText.Text = "-";
                WinnerWatchLaterText.Text = "-";
                WinnerRatingText.Text = "-";
                WinnerCoverImage.Source = null;

                HonorableMentionsPanel.Children.Clear();

                TextBlock emptyText = new TextBlock();
                emptyText.Text = "Keine weiteren Vorschläge vorhanden.";
                emptyText.Foreground = new SolidColorBrush(Color.FromRgb(209, 213, 219));
                emptyText.FontSize = 16;
                emptyText.TextWrapping = TextWrapping.Wrap;

                HonorableMentionsPanel.Children.Add(emptyText);

                return;
            }

            Result winner = results.First();

            WinnerTitleText.Text = winner.Movie.Name;
            WinnerScoreText.Text = winner.Score.ToString();
            WinnerLikesText.Text = winner.Likes.ToString();
            WinnerDislikesText.Text = winner.Dislikes.ToString();
            WinnerWatchLaterText.Text = winner.WatchLater.ToString();
            WinnerRatingText.Text = winner.Movie.VoteAverage.ToString("0.0") + " / 10";

            LoadWinnerCover(winner.Movie);

            ShowHonorableMentions();
        }

        private void LoadWinnerCover(Movies movie)
        {
            try
            {
                if (movie == null || string.IsNullOrEmpty(movie.Cover))
                {
                    WinnerCoverImage.Source = null;
                    return;
                }

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(movie.Cover, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                WinnerCoverImage.Source = image;
            }
            catch
            {
                WinnerCoverImage.Source = null;
            }
        }



        private void ShowHonorableMentions()
        {
            HonorableMentionsPanel.Children.Clear();

            if (results == null || results.Count <= 1)
            {
                TextBlock emptyText = new TextBlock();
                emptyText.Text = "Es gibt keine weiteren Vorschläge.";
                emptyText.Foreground = new SolidColorBrush(Color.FromRgb(209, 213, 219));
                emptyText.FontSize = 16;
                emptyText.TextWrapping = TextWrapping.Wrap;

                HonorableMentionsPanel.Children.Add(emptyText);

                return;
            }

            int place = 2;

            foreach (Result result in results.Skip(1).Take(4))
            {
                Border card = CreateHonorableMentionCard(result, place);

                HonorableMentionsPanel.Children.Add(card);

                place++;
            }
        }

        // Mit Ki hilfe gemacht die Methode macht die Honrable Mentions also Filme die gut abgeschnitten haben aber nicht gewonnen haben


        private Border CreateHonorableMentionCard(Result result, int place)
        {
            Border card = new Border();
            card.Background = new SolidColorBrush(Color.FromRgb(17, 24, 39));
            card.CornerRadius = new CornerRadius(16);
            card.Padding = new Thickness(16);
            card.Margin = new Thickness(0, 0, 0, 10);
            card.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 65, 81));
            card.BorderThickness = new Thickness(1);

            TextBlock text = new TextBlock();
            text.Foreground = Brushes.White;
            text.FontSize = 16;
            text.TextWrapping = TextWrapping.Wrap;

            text.Text =
                place + ". " + result.Movie.Name +
                "   |   Score: " + result.Score +
                "   |   Likes: " + result.Likes +
                "   |   Dislikes: " + result.Dislikes +
                "   |   Später: " + result.WatchLater +
                "   |   Bewertung: " + result.Movie.VoteAverage.ToString("0.0") + " / 10";

            card.Child = text;

            return card;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeView homeView = new HomeView();
            homeView.Show();

            this.Close();
        }
    }
}