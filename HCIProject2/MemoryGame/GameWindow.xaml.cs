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
using System.Threading;
using System.Drawing;
using System.Windows.Threading;
using System.IO;
namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private volatile bool stopTimer = false;
        private bool startClicked = false;
        public int Counter { get; set; }
        public string category;
        public List<Button> Tiles { get; set; }
        public List<BitmapImage> Images { get; set; }
        
        private string ANIMAL_IMAGES_PATH;
        public Dictionary<Button,int> TileImageDict { get; set; }
        private List<int> foundPairs;
        private int firstClickedIndex = -1;
        public GameWindow(string category)
        {
            string parentDir= Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            string memoryGameDir=Directory.GetParent(parentDir).ToString();
            ANIMAL_IMAGES_PATH = string.Concat(memoryGameDir, "/Resources/");
            this.Counter = 120;
            //category -> animals or fruits
            this.category = category;
            InitializeComponent();
            InitListOfButtons();
            InitImages();
            InitTiles();
            

        }
        private void StartTimer()
        {
            //start timer
            Thread t = new Thread(() =>
            {
                for (int i = 0; i < 120; i++)
                {
                    if (!stopTimer)
                    {
                        Thread.Sleep(1000);
                        Dispatcher.Invoke(() =>
                        {
                            timer.Content = Counter.ToString();
                        });
                        Counter -= 1;
                    }
                }
                if (Counter == 0)
                {
                    MessageBox.Show("You lost!");
                    for (int i = 0; i < 36; i++)
                        Tiles.ElementAt(i).IsEnabled = false;
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        private void InitListOfButtons()
        {
            //init list of buttons
            Tiles = new List<Button>();
            Tiles.Add(tile0);
            Tiles.Add(tile1);
            Tiles.Add(tile2);
            Tiles.Add(tile3);
            Tiles.Add(tile4);
            Tiles.Add(tile5);
            Tiles.Add(tile6);
            Tiles.Add(tile7);
            Tiles.Add(tile8);
            Tiles.Add(tile9);
            Tiles.Add(tile10);
            Tiles.Add(tile11);
            Tiles.Add(tile12);
            Tiles.Add(tile13);
            Tiles.Add(tile14);
            Tiles.Add(tile15);
            Tiles.Add(tile16);
            Tiles.Add(tile17);
            Tiles.Add(tile18);
            Tiles.Add(tile19);
            Tiles.Add(tile20);
            Tiles.Add(tile21);
            Tiles.Add(tile22);
            Tiles.Add(tile23);
            Tiles.Add(tile24);
            Tiles.Add(tile25);
            Tiles.Add(tile26);
            Tiles.Add(tile27);
            Tiles.Add(tile28);
            Tiles.Add(tile29);
            Tiles.Add(tile30);
            Tiles.Add(tile31);
            Tiles.Add(tile32);
            Tiles.Add(tile33);
            Tiles.Add(tile34);
            Tiles.Add(tile35);

        }
        private void InitImages()
        {
            //store images from resources into list
            Images = new List<BitmapImage>();
            if ("animals".Equals(category))
            {
               for(int i = 1; i <= 18; i++)
                {
                    Images.Add(new BitmapImage(new Uri(ANIMAL_IMAGES_PATH + "/img" + i+".png")));
                   
                }
            }

            ShuffleList(Images);
            
           
        }
        private void InitTiles()
        {
            //initialize tiles - set background images
            TileImageDict = new Dictionary<Button, int>();
            int tileIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    ImageBrush image = new ImageBrush();
                    image.ImageSource = Images.ElementAt(j);
                    Tiles.ElementAt(tileIndex).Background = image;
                    TileImageDict.Add(Tiles.ElementAt(tileIndex), j);
                    tileIndex++;
                }

            }
        }
        private static void ShuffleList<T>(List<T> list)
        {
            //shuffle images so they do not repeat ( example: 18 images-> image at index 0 and 18 are same, 1 and 19 and so on)
            Random rng = new Random();
            
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
       
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            //set startClicked to true
            startClicked = true;
            //reset foundPairs & first clicked tile index
            foundPairs = new List<int>();
            firstClickedIndex = -1;
            //reset timer
            Counter = 120;
            stopTimer = false;
            
            // after start is clicked paint all tiles gray
            int tileIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    
                    Tiles.ElementAt(tileIndex).Background = new SolidColorBrush(Colors.Gray);
                    
                    tileIndex++;
                }

            }
            StartTimer();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (startClicked)
            {
                Button clickedTile = (Button)sender;
                int tileIndex = Tiles.IndexOf(clickedTile);

                // check if pair has already been found
                if (foundPairs.Contains(TileImageDict[clickedTile]))
                    return;

                // set image for first clicked tile
                ImageBrush image = new ImageBrush();
                image.ImageSource = Images.ElementAt(TileImageDict[clickedTile]);
                clickedTile.Background = image;

                if (firstClickedIndex == -1)
                {
                    // first tile clicked
                    firstClickedIndex = tileIndex;
                }
                else
                {
                    // second tile clicked
                    //checking if image indexes match
                    if (TileImageDict[Tiles[firstClickedIndex]] == TileImageDict[clickedTile])
                    {
                        // same images - update foundPairs
                        foundPairs.Add(TileImageDict[clickedTile]);
                        foundPairs.Add(TileImageDict[Tiles[firstClickedIndex]]);

                        // checking if it is the last pair
                        CheckForWin();
                    }
                    else
                    {

                        Thread.Sleep(200); // wait animation - turn over tiles
                        Tiles[firstClickedIndex].Background = new SolidColorBrush(Colors.Gray);
                        clickedTile.Background = new SolidColorBrush(Colors.Gray);
                    }

                    firstClickedIndex = -1; // reset index of first clicked button
                }
            }
        }
        private void CheckForWin()
        {
            // Check if all pairs are matched
            if (foundPairs.Count == Images.Count * 2)
            {
                stopTimer = true;
                
                MessageBox.Show("Congrats! You win!");
                for (int i = 0; i < 36; i++)
                    Tiles.ElementAt(i).IsEnabled = false;
            }
        }
       
    }
}
