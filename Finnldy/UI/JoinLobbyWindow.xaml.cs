using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for JoinLobbyWindow.xaml
    /// </summary>
    public partial class JoinLobbyWindow : Window
    {
        private User currentUser;

        public JoinLobbyWindow(User user)
        {
            InitializeComponent();

            currentUser = user;
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            string code = LobbyCodeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Bitte gib einen Lobby-Code ein.");
                return;
            }

            SwipeView swipeView = new SwipeView(
                currentUser,
                new List<int>(),
                new List<string>(),
                true
            );

            swipeView.Show();
            this.Close();


        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeView homeView = new HomeView();
            homeView.Show();

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
