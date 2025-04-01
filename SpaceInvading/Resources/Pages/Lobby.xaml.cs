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


            AddScore("maks", 233330, 1, 0, (random.NextDouble() * 12) - 6);
            AddScore("rafal", 3, 1, 1, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 2, 0, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 2, 1, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 3, 0, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 3, 1, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 4, 0, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 4, 1, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 5, 0, (random.NextDouble() * 12) - 6);
            AddScore("leon", 3, 5, 1, (random.NextDouble() * 12) - 6);
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
            Grid grid = (Grid)Scoreboard.Content;
            Grid poster = new Grid
            {
                RowDefinitions =
                {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
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
                Foreground = Brushes.Brown,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 30,
                Margin = new Thickness(0, 5, 0, 0),
                RenderTransform = new RotateTransform(tilt),
            };
            Label name = new Label
            {
                Content = n,
                Foreground = Brushes.Brown,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 30,
                Margin = new Thickness(0, 5, 0, 0),
                RenderTransform = new RotateTransform(tilt),
            };
            Label score = new Label
            {
                Content = s.ToString(),
                Foreground = Brushes.Brown,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                FontSize = 30,
                RenderTransform = new RotateTransform(tilt),
            };

            RenderOptions.SetBitmapScalingMode(back, BitmapScalingMode.NearestNeighbor);
            Grid.SetRowSpan(back, 3);
            poster.Children.Add(back);

            Grid.SetRow(wanted, 0);
            poster.Children.Add(wanted);

            Grid.SetRow(name, 1);
            poster.Children.Add(name);

            Grid.SetRow(score, 2);
            poster.Children.Add(score);


            Grid.SetRow(poster, row);
            Grid.SetColumn(poster, col);
            grid.Children.Add(poster);
        }
    }
}
