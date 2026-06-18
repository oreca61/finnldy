using Finnldy.DAL;
using System.Linq;
using System.Windows;

namespace Finnldy.UI
{
    public partial class WatchLaterView : Window
    {
        private FastApiService fastApiService =new FastApiService() ;

        public WatchLaterView()
        {
            InitializeComponent();




        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WatchLaterListBox.Items.Clear();



            WatchLaterListBox.Items.Add("filme werden geladen...");




            var results = await fastApiService.GetResultsAsync();

            var watchLaterMovies = results

                .Where(result => result.WatchLater > 0)
                .OrderByDescending(result => result.WatchLater)
                .ThenByDescending(result => result.Score)

                .ToList();



            WatchLaterListBox.Items.Clear();

            if (watchLaterMovies.Count ==0)
            {

                WatchLaterListBox.Items.Add("Es gibt keine Filme");
                return;
            }

            foreach (var movie in watchLaterMovies)
            {
                string text = movie.Title;



                WatchLaterListBox.Items.Add(text);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}