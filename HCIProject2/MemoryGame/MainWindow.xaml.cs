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
            if (chooseCategorieBox.SelectedItem != null)
            {
                if (chooseCategorieBox.SelectedItem.Equals(animalsItem))
                {
                    this.Hide();
                    new GameWindow("Animals").Show();
                }
                else if(chooseCategorieBox.SelectedItem.Equals(fruitsItem))
                {
                    this.Hide();
                    new GameWindow("Fruits").Show();
                }
            }
        }
    }
}
