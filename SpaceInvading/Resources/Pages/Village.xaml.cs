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

        private void Image_GotMouseCapture(object sender, MouseEventArgs e)
        {
            // Cast the sender to an Image
            Image img = (Image)sender;

            // Get the current image source as a string
            string currentUri = img.Source.ToString();

            // Find the position of the .png extension
            int extensionIndex = currentUri.LastIndexOf(".png");

            // If the .png extension is found, modify the URI by adding "_selected" before the extension
            if (extensionIndex > 0)
            {
                // Create the new URI by inserting "_selected" before the .png extension
                string newUri = currentUri.Substring(0, extensionIndex) + "_selected.png";

                // Set the new image source
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
