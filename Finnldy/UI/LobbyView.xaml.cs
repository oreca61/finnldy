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



        public LobbyView(User user, List<int> wantedGenreIds, List<string> wantedLanguages, bool hideAdultMovies)
        {
            InitializeComponent();

            currentUser = user;

            this.wantedGenreIds = wantedGenreIds;
            this.wantedLanguages = wantedLanguages;
            this.hideAdultMovies = hideAdultMovies;
        }

        private void StartSwipeButton_Click(object sender, RoutedEventArgs e)
        {
            SwipeView swipeView = new SwipeView(
                currentUser,
                wantedGenreIds,
                wantedLanguages,
                hideAdultMovies
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
            this.Close();
        }
    }
}
