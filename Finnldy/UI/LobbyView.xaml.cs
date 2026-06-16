using Finnldy.BLL;
using Finnldy.DAL;
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
    /// Interaction logic for LobbyView.xaml
    /// </summary>
    public partial class LobbyView : Window
    {
        private User currentUser;

        private List<int> wantedGenreIds;
        private List<string> wantedLanguages;
        private bool hideAdultMovies;
        private int maxSwipes;




        public LobbyView(User user, List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies, int maxSwipes)
        {
            InitializeComponent();

            currentUser = user;

            this.wantedGenreIds = wantedGenreIds;
            this.wantedLanguages = wantedLanguages;
            this.hideAdultMovies = hideAdultMovies;
            this.maxSwipes = maxSwipes;

            StartHost();
        }

        private void StartHost()
        {
            NetworkSession.Username = currentUser.Name;
            NetworkSession.IsHost = true;
            NetworkSession.IsClient = false;

            NetworkSession.LobbyController.RegisterNetworkEvents();

            NetworkSession.LobbyController.NetworkStatusChanged += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine(message);

                    if (message.Contains("Client verbunden"))
                    {
                        MessageBox.Show("Ein Client ist der Lobby beigetreten.");
                    }
                });
            };

            _ = Task.Run(async () =>
            {
                await NetworkSession.Host.StartAsync(5000);
            });

            LobbyCodeText.Text = HostNetworkService.GetLocalIpAddress();
        }

        private async void StartSwipeButton_Click(object sender, RoutedEventArgs e)
        {
            await NetworkSession.LobbyController.SendLobbySettingsAsync( 
                wantedGenreIds, 
                wantedLanguages , 
                hideAdultMovies );



            SwipeView swipeView = new SwipeView(
                currentUser,
                wantedGenreIds,
                wantedLanguages,
                hideAdultMovies,
                maxSwipes
                );



            swipeView.Show();
            Close();
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeView homeView = new HomeView();
            homeView.Show();

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
