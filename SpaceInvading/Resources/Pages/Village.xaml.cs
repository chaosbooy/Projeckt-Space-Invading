using SpaceInvading.Pages;
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

namespace SpaceInvading.Resources.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Village.xaml
    /// </summary>
    public partial class Village : Page
    {
        public Village()
        {
            InitializeComponent();
        }


        private void StartGame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Game());
        }
    }
}
