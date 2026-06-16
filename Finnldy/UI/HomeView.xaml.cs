using Finnldy.BLL;
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
    public partial class HomeView : Window
    {
        private User currentUser;

        public HomeView()
        {
            InitializeComponent();
        }

        private bool CreateCurrentUser()
        {
            string username = UsernameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Bitte gib zuerst einen Namen ein.");
                return false;
            }

            currentUser = new User(username);
            return true;
        }

        private void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateCurrentUser())
            {
                return;
            }

            GenreSelectionWindow genreWindow = new GenreSelectionWindow();

            bool? result = genreWindow.ShowDialog();

            int maxSwipes = genreWindow.MaxSwipes;

            if (result == true)
            {
                LobbyView lobbyView = new LobbyView(
                    currentUser,
                    genreWindow.WantedGenreIds,
                    genreWindow.WantedLanguages,
                    genreWindow.HideAdultMovies,
                    genreWindow.MaxSwipes

                );

                lobbyView.Show();
                this.Close();
            }
        }

        private void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateCurrentUser())
            {
                return;
            }

            JoinLobbyWindow joinLobbyWindow = new JoinLobbyWindow(currentUser);
            joinLobbyWindow.Show();

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}