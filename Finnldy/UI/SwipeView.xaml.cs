using Finnldy.BLL;
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
        public SwipeView()
        {

            InitializeComponent();


            Lobby lobby = new Lobby();
            User currentUser = new User();


            SwiperContoller swiperContoller = new SwiperContoller();
            MovieReposotory movieReposotory = new MovieReposotory();

        }


        private async void SwipeView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void WatchedButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
