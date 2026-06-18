using Finnldy.BLL;
using Finnldy.DAL;
using Finnldy.DAL.Database;
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
        public ClientNetworkService ClientNetworkService = new ClientNetworkService();

        public JoinLobbyWindow(User user)
        {
            InitializeComponent();

            currentUser = user;
        }



        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            string code = LobbyCodeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Bitte gib einen Lobby-Code ein.");
                return;
            }

            NetworkSession.Username = currentUser.Name;
            NetworkSession.IsHost = false;


            NetworkSession.IsClient = true;

            NetworkSession.LobbyController.RegisterNetworkEvents();

            // KI Anfang
            // Chat
            // Kannst du mir hier den Fehler finden und auch ausverbessen?
            // Methdoe bei Zeile 85( OnNetworkPacketReceived ) war auch von ihn und meinte dass ich es trennen sollte
            NetworkSession.LobbyController.NetworkPacketReceived += OnNetworkPacketReceived;
            NetworkSession.LobbyController.NetworkStatusChanged += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine(message);
                });
            };
            // KI-Ende

            await NetworkSession.Client.ConnectAsync(code, 5000);


            MessageBox.Show("Verbunden. Warte, bis der Host die Lobby startet.");





        }

        private void OnNetworkPacketReceived(NetworkPacket packet)
        {
            if (packet.Type != "lobbySettings")
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                SwipeView swipeView = new SwipeView(
                    currentUser,
                    packet.WantedGenreIds,
                    packet.WantedLanguages,
                    packet.HideAdultMovies
                );

                swipeView.Show();
                Close();
            });
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
