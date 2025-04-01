using SpaceInvading.Resources.Classes;
using SpaceInvading.Resources.Pages;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvading.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        private Dictionary<string, int> previousSaves = new Dictionary<string, int>();

        public Lobby()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(Image_Title, BitmapScalingMode.NearestNeighbor);
              
            Image_Title.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Background/Title_Card.png"));
            Random random = new Random();

            SavingSystem.Initialize();
            var scores = SavingSystem.Scores;

            for (int i = 0; i < scores.Count; i++)
            {
                AddScore(scores[i].Item1, scores[i].Item2, i / 2 + 1, i % 2, (random.NextDouble() * 12) - 6);
            }
        }
        
        private void StartGame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Village());
            Inventory.AddItem(AllItems.Coin, 300);
            Inventory.AddItem(AllItems.SlimeDrop, 10);
            Inventory.AddUsableUpgrade(AllItems.ShieldPotion);
        }

        private void LeaveGame(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void AddScore(string n, int s, int row, int col, double tilt)
        {
            SolidColorBrush TextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6E260E"));
            Grid grid = (Grid)Scoreboard.Content;
            Grid poster = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
            };
            System.Windows.Controls.Image back = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Poster.png")),
                Stretch = Stretch.UniformToFill,
                RenderTransform = new RotateTransform(tilt),
            };
            Label wanted = new Label
            {
                Content = "Wanted",
                Foreground = TextBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 30,
                Margin = new Thickness(0, 15, 0, 0),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Resources/Fonts/Nashville.ttf#Nashville"),
                RenderTransform = new RotateTransform(tilt),
            };
            Label name = new Label
            {
                Content = n,
                Foreground = TextBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 30,
                Margin = new Thickness(0, 20, 0, 0),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Resources/Fonts/Nashville.ttf#Nashville"),
                RenderTransform = new RotateTransform(tilt),
            };
            Label score = new Label
            {
                Content = $"{s}$",
                Foreground = TextBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 30),
                FontSize = 30,
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Resources/Fonts/Helldorado.ttf#Helldorado"),
                RenderTransform = new RotateTransform(tilt),
            };

            RenderOptions.SetBitmapScalingMode(back, BitmapScalingMode.NearestNeighbor);
            Grid.SetRowSpan(back, 3);
            poster.Children.Add(back);

            //Grid.SetRow(wanted, 0);
            poster.Children.Add(wanted);

            //Grid.SetRow(name, 1);
            poster.Children.Add(name);

            //Grid.SetRow(score, 2);
            poster.Children.Add(score);


            Grid.SetRow(poster, row);
            Grid.SetColumn(poster, col);
            grid.Children.Add(poster);
        }
    }
}
