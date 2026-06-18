using Finnldy.BLL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
                WinnerInfoText.Text = "Es wurden keine passenden Filme bewertet.";
                return;
            }

            Result winner = results.First();

            WinnerTitleText.Text = winner.Movie.Name;

            WinnerInfoText.Text =
                "Score: " + winner.Score +
                " | Likes: " + winner.Likes +
                " | Dislikes: " + winner.Dislikes +
                " | Später ansehen: " + winner.WatchLater +
                " | Bewertung: " + winner.Movie.VoteAverage.ToString("0.0") + " / 10";

            ResultListBox.Items.Clear();

            int place = 2;

            foreach (Result result in results.Skip(1))
            {
                string text =
                    place + ". " +
                    result.Movie.Name +
                    " | Score: " + result.Score +
                    " | Likes: " + result.Likes +
                    " | Bewertung: " + result.Movie.VoteAverage.ToString("0.0") + " / 10";

                ResultListBox.Items.Add(text);

                place++;
            }
        }
    }
}