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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            string name;
            if (playerNameTBox.Text.Equals(""))
                name = "Unknown";
            else name = playerNameTBox.Text.ToString();

            if (chooseCategorieBox.SelectedIndex == -1 || chooseCategorieBox.SelectedItem.Equals(animalsItem))
            {
                this.Hide();
                if (chooseDifficultyBox.SelectedIndex == -1 || chooseDifficultyBox.SelectedItem.Equals(easyItem))
                    new GameWindow("Animals", 120, name).Show();
                else if (chooseDifficultyBox.SelectedItem.Equals(mediumItem))
                    new GameWindow("Animals", 90, name).Show();
                else if (chooseDifficultyBox.SelectedItem.Equals(hardItem))
                    new GameWindow("Animals", 60, name).Show();
            }
            else if (chooseCategorieBox.SelectedItem.Equals(fruitsItem))
            {
                this.Hide();
                if (chooseDifficultyBox.SelectedIndex == -1 || chooseDifficultyBox.SelectedItem.Equals(easyItem))
                    new GameWindow("Fruits", 120, name).Show();
                else if (chooseDifficultyBox.SelectedItem.Equals(mediumItem))
                    new GameWindow("Fruits", 90, name).Show();
                else if (chooseDifficultyBox.SelectedItem.Equals(hardItem))
                    new GameWindow("Fruits", 60, name).Show();
            }


        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ScoresClicked(object sender, RoutedEventArgs e)
        {
            new Results().Show();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
