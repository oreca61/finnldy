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
    /// Interaction logic for GenreSelectionWindow.xaml
    /// </summary>
    public partial class GenreSelectionWindow : Window
    {
        public List<int> WantedGenreIds { get; private set; } = new List<int>();
        public List<string> WantedLanguages { get; private set; } = new List<string>();
        public bool HideAdultMovies { get; private set; }

        public int MaxSwipes { get; private set; }

        public HostNetworkService networkService = new HostNetworkService();

        public GenreSelectionWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            WantedGenreIds.Clear();
            WantedLanguages.Clear();

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
            AddGenreIfChecked(MusicCheckBox);
            AddGenreIfChecked(MysteryCheckBox);
            AddGenreIfChecked(RomanceCheckBox);
            AddGenreIfChecked(ScienceFictionCheckBox);
            AddGenreIfChecked(TvMovieCheckBox);
            AddGenreIfChecked(ThrillerCheckBox);
            AddGenreIfChecked(WarCheckBox);
            AddGenreIfChecked(WesternCheckBox);

            AddLanguageIfChecked(GermanLanguageCheckBox);
            AddLanguageIfChecked(EnglishLanguageCheckBox);
            AddLanguageIfChecked(HindiLanguageCheckBox);
            AddLanguageIfChecked(JapaneseLanguageCheckBox);
            AddLanguageIfChecked(KoreanLanguageCheckBox);
            AddLanguageIfChecked(FrenchLanguageCheckBox);
            AddLanguageIfChecked(SpanishLanguageCheckBox);
            AddLanguageIfChecked(ItalianLanguageCheckBox);

            HideAdultMovies = HideAdultMoviesCheckBox.IsChecked == true;

            if (!int.TryParse(MaxSwipesTextBox.Text, out int enteredMaxSwipes))
            {
                MessageBox.Show("Bitte gib eine gültige Swipe-Anzahl ein.");
                return;
            }

            if (enteredMaxSwipes <= 0)
            {
                MessageBox.Show("Die Swipe-Anzahl muss größer als 0 sein.");
                return;
            }

            if (enteredMaxSwipes > 100)
            {
                MessageBox.Show("Bitte gib maximal 100 Swipes ein.");
                return;
            }

            MaxSwipes = enteredMaxSwipes;

            DialogResult = true;
            Close();
        }

        private void AddGenreIfChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked == true)
            {
                int genreId = int.Parse(checkBox.Tag.ToString());
                WantedGenreIds.Add(genreId);
            }
        }

        private void AddLanguageIfChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked == true)
            {
                string language = checkBox.Tag.ToString();
                WantedLanguages.Add(language);
            }
        }
    }
}
