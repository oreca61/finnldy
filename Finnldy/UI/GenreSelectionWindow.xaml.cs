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
    /// Interaction logic for GenreSelectionWindow.xaml
    /// </summary>
    public partial class GenreSelectionWindow : Window
    {
        public List<int> UnwantedGenreIds { get; private set; } = new List<int>();

        public GenreSelectionWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            UnwantedGenreIds.Clear();

            AddGenreIfChecked(ActionCheckBox);
            AddGenreIfChecked(AdventureCheckBox);
            AddGenreIfChecked(AnimationCheckBox);
            AddGenreIfChecked(ComedyCheckBox);
            AddGenreIfChecked(CrimeCheckBox);
            AddGenreIfChecked(DocumentaryCheckBox);
            AddGenreIfChecked(DramaCheckBox);
            AddGenreIfChecked(FamilyCheckBox);
            AddGenreIfChecked(FantasyCheckBox);
            AddGenreIfChecked(HorrorCheckBox);
            AddGenreIfChecked(RomanceCheckBox);
            AddGenreIfChecked(SciFiCheckBox);
            AddGenreIfChecked(ThrillerCheckBox);
            AddGenreIfChecked(WarCheckBox);
            AddGenreIfChecked(WesternCheckBox);

            DialogResult = true;
            Close();
        }

        private void AddGenreIfChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked == true)
            {
                int genreId = int.Parse(checkBox.Tag.ToString());
                UnwantedGenreIds.Add(genreId);
            }
        }
    }
}
