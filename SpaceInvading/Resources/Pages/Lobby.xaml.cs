using SpaceInvading.Resources.Classes;
using SpaceInvading.Resources.Pages;
using System.Windows;
using System.Windows.Controls;

namespace SpaceInvading.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        public Lobby()
        {
            InitializeComponent();
            
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Village());
        }

        private void LeaveGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
