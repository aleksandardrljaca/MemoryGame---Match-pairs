using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public class Result
        {
            public string PlayerName { get; set; }
            public string Pesult { get; set; }
        }
        private string _gamePath;
       
        public Results()
        {
            InitializeComponent();
            string parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            _gamePath = Directory.GetParent(parentDir).ToString();
            readResults();
        }
        private void readResults()
        {
            List<Result> lines = new List<Result>();
            using (StreamReader inputFile = new StreamReader(_gamePath + "/Results.txt", true))
            {
                while (!inputFile.EndOfStream)
                {
                    Result r = new Result();
                    string[] namePoints = inputFile.ReadLine().Split(',');
                    r.PlayerName = namePoints[0];
                    r.Pesult = namePoints[1];
                    lines.Add(r);
                    
                }
            }
            dataGrid.ItemsSource = lines;
            
            
          
            
        }
    }
}
