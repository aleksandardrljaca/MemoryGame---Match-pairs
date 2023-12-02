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
using System.Media;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private volatile bool _stopTimer = false;
        private bool _startClicked = false;
        private SoundPlayer _soundsPlayer = new SoundPlayer();
        public int Counter { get; set; }
        private string _category;
        private int _level;
        private string _playerName;
        public int Result { get; set; }
        private bool _prevMatch = false;
        private List<Button> _tiles;
        private List<BitmapImage> _images;
        private string _memoryGameDir;
        private string _imagesPath;
        private string _soundsPath;

        public Dictionary<Button, int> _tileImageDict;
        private List<int> _foundPairs;
        private int _firstClickedIndex = -1;
        public GameWindow(string category, int level, string name)
        {
            string parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            _memoryGameDir = Directory.GetParent(parentDir).ToString();
            _imagesPath = string.Concat(_memoryGameDir, "/Resources/", category);
            _soundsPath = string.Concat(_memoryGameDir, "/Resources/Sounds");

            this._level = level;
            _playerName = name;
            this.Counter = level;
            this.Result = 0;
            //category -> animals or fruits
            this._category = category;
            InitializeComponent();
            InitListOfButtons();
            InitImages();
            Init_tiles();


        }
        private void StartTimer()
        {
            //start timer
            Thread t = new Thread(() =>
            {
                for (int i = 0; i < _level; i++)
                {
                    if (!_stopTimer)
                    {
                        Thread.Sleep(1000);
                        Dispatcher.Invoke(() =>
                        {
                            timer.Content = Counter.ToString();
                        });
                        Counter -= 1;
                    }
                    else break;

                }
                if (Counter == 0)
                {
                    MessageBox.Show("You lost!");
                    logResult();
                    _soundsPlayer.SoundLocation = _soundsPath + "/gameover.wav";
                    _soundsPlayer.Play();
                    for (int i = 0; i < 36; i++)
                        _tiles.ElementAt(i).IsEnabled = false;
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        private void InitListOfButtons()
        {
            //init list of buttons
            _tiles = new List<Button>();
            _tiles.Add(tile0);
            _tiles.Add(tile1);
            _tiles.Add(tile2);
            _tiles.Add(tile3);
            _tiles.Add(tile4);
            _tiles.Add(tile5);
            _tiles.Add(tile6);
            _tiles.Add(tile7);
            _tiles.Add(tile8);
            _tiles.Add(tile9);
            _tiles.Add(tile10);
            _tiles.Add(tile11);
            _tiles.Add(tile12);
            _tiles.Add(tile13);
            _tiles.Add(tile14);
            _tiles.Add(tile15);
            _tiles.Add(tile16);
            _tiles.Add(tile17);
            _tiles.Add(tile18);
            _tiles.Add(tile19);
            _tiles.Add(tile20);
            _tiles.Add(tile21);
            _tiles.Add(tile22);
            _tiles.Add(tile23);
            _tiles.Add(tile24);
            _tiles.Add(tile25);
            _tiles.Add(tile26);
            _tiles.Add(tile27);
            _tiles.Add(tile28);
            _tiles.Add(tile29);
            _tiles.Add(tile30);
            _tiles.Add(tile31);
            _tiles.Add(tile32);
            _tiles.Add(tile33);
            _tiles.Add(tile34);
            _tiles.Add(tile35);

        }
        private void InitImages()
        {
            //store images from resources into list
            _images = new List<BitmapImage>();

            for (int i = 1; i <= 18; i++)
            {
                _images.Add(new BitmapImage(new Uri(_imagesPath + "/img" + i + ".png")));

            }



            ShuffleList(_images);


        }
        private void Init_tiles()
        {

            //initialize tiles - set background images
            _tileImageDict = new Dictionary<Button, int>();
            int tileIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    int jj = j;
                    if (i == 2)
                        j = 17 - j;
                    ImageBrush image = new ImageBrush();
                    image.ImageSource = _images.ElementAt(j);
                    _tiles.ElementAt(tileIndex).Background = image;
                    _tileImageDict.Add(_tiles.ElementAt(tileIndex), j);
                    tileIndex++;
                    j = jj;
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
            //set _startClicked to true
            _startClicked = true;
            //reset _foundPairs & first clicked tile index
            _foundPairs = new List<int>();
            _firstClickedIndex = -1;
            //reset timer
            Counter = _level;
            _stopTimer = false;

            // after start is clicked paint all tiles gray
            int tileIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j < 18; j++)
                {

                    _tiles.ElementAt(tileIndex).Background = new SolidColorBrush(Colors.Gray);

                    tileIndex++;
                }

            }
            StartTimer();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _stopTimer = true;
            this.Close();
            new MainWindow().Show();

        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

            if (_startClicked)
            {
                Button clickedTile = (Button)sender;
                int tileIndex = _tiles.IndexOf(clickedTile);

                // check if pair has already been found
                if (_foundPairs.Contains(_tileImageDict[clickedTile]))
                    return;

                // set image for first clicked tile
                ImageBrush image = new ImageBrush();
                image.ImageSource = _images.ElementAt(_tileImageDict[clickedTile]);
                clickedTile.Background = image;

                if (_firstClickedIndex == -1)
                {
                    // first tile clicked
                    _firstClickedIndex = tileIndex;
                }
                else
                {

                    // second tile clicked
                    //checking if image indexes match
                    if (_tileImageDict[_tiles[_firstClickedIndex]] == _tileImageDict[clickedTile])
                    {
                        // same images - update _foundPairs
                        _foundPairs.Add(_tileImageDict[clickedTile]);
                        _foundPairs.Add(_tileImageDict[_tiles[_firstClickedIndex]]);
                        _soundsPlayer.SoundLocation = _soundsPath + "/match.wav";
                        _soundsPlayer.Play();
                        if (!_prevMatch)
                        {
                            Result += 15;
                            Dispatcher.Invoke(() =>
                            {
                                resultLbl.Content = Result.ToString();
                            });
                        }
                        else
                        {
                            Result += 25;
                            Dispatcher.Invoke(() =>
                            {
                                resultLbl.Content = Result.ToString();
                            });
                        }
                        _prevMatch = true;


                        // checking if it is the last pair
                        CheckForWin();
                    }
                    else
                    {

                        _soundsPlayer.SoundLocation = _soundsPath + "/miss.wav";
                        _soundsPlayer.Play();
                        _prevMatch = false;
                        Thread.Sleep(200); // wait animation - turn over tiles
                        _tiles[_firstClickedIndex].Background = new SolidColorBrush(Colors.Gray);
                        clickedTile.Background = new SolidColorBrush(Colors.Gray);
                    }

                    _firstClickedIndex = -1; // reset index of first clicked button
                }
            }
        }
        private void CheckForWin()
        {
            // Check if all pairs are matched
            if (_foundPairs.Count == _images.Count * 2)
            {
                _stopTimer = true;
                _soundsPlayer.SoundLocation = _soundsPath + "/win.wav";
                _soundsPlayer.Play();
                MessageBox.Show("Congrats! You win!");
                logResult();

            }
        }
        private void logResult()
        {
            using (StreamWriter outputFile = new StreamWriter(_memoryGameDir + "/Results.txt", true))
            {
                outputFile.WriteLine(_playerName + "," + Result + " pts");
            }
        }


    }
}
