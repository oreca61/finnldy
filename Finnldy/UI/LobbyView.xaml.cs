using Finnldy.BLL;
using Finnldy.DAL.Database;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Finnldy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
        


        }

        private void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            GenreSelectionWindow genreWindow = new GenreSelectionWindow();

            bool? result = genreWindow.ShowDialog();

            if (result == true)
            {
                List<int> unwantedGenreIds = genreWindow.UnwantedGenreIds;

                SwipeView swipeView = new SwipeView(unwantedGenreIds);
                swipeView.Show();

                this.Close();
            }
        }

        private void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}