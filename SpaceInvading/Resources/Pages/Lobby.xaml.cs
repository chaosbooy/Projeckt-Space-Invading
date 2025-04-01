using SpaceInvading.Resources.Classes;
using SpaceInvading.Resources.Pages;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;

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
    }
}
