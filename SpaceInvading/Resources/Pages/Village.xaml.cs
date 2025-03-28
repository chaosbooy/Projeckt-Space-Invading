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
using static System.Net.Mime.MediaTypeNames;

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
            System.Windows.Controls.Image img = (System.Windows.Controls.Image)sender;
            if (FrameShop.Visibility == Visibility.Visible) FrameShop.Visibility = Visibility.Collapsed;
            else FrameShop.Visibility = Visibility.Visible;
            closeFrame.Visibility = Visibility.Visible;
            switch (img.Name)
            {
                case "Blacksmith":
                    {
                        FrameShop.Navigate(new Blacksmith());
                        break;
                    }
                case "Shop":
                    {
                        FrameShop.Navigate(new Shop());
                        break;
                    }
                case "Witch":
                    {
                        FrameShop.Navigate(new Witch());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image img = (System.Windows.Controls.Image)sender;

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
            System.Windows.Controls.Image img = (System.Windows.Controls.Image)sender;

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
        private void CloseFrame(object sender, MouseButtonEventArgs e)
        {
            FrameShop.Visibility = Visibility.Collapsed;
            closeFrame.Visibility = Visibility.Collapsed;
        }
    }
}
