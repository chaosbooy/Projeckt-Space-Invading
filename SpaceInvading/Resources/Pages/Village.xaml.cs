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

        private void GoShop(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Shop());
        }
        private void MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            string currentUri = img.Source.ToString();

            int extensionIndex = currentUri.LastIndexOf(".png");

            if (extensionIndex > 0)
            {
                string newUri = currentUri.Substring(0, extensionIndex) + "_selected.png" ;

                img.Source = new BitmapImage(new Uri(newUri));
            }
        }
        private void MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            string currentUri = img.Source.ToString();

            int extensionIndex = currentUri.LastIndexOf("_selected.png");

            if (extensionIndex > 0)
            {
                string newUri = currentUri.Substring(0, extensionIndex) + ".png";

                img.Source = new BitmapImage(new Uri(newUri));
            }
        }    
        private void Button_GotMouseCapture(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
