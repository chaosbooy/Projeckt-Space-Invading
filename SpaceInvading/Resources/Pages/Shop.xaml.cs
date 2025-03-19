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
    /// Logika interakcji dla klasy Shop.xaml
    /// </summary>
    public partial class Shop : Page
    {
        public Shop()
        {
            InitializeComponent();
            CreateOffers();
        }

        private void BackToVillage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Village());
            
        }

        private void CreateOffers()
        {
            for(int i = 0; i < 10; i++)
            {

                Image offerBackground = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Offers/offerDisabled.png")),
                    Stretch = Stretch.Fill
                };
                Grid.SetRow(offerBackground, i + 1);
                Offers.Children.Add(offerBackground);

                Image offer = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Offers/armor1.png")),
                    Stretch = Stretch.Fill
                };
                Grid.SetRow(offer, i + 1);
                Offers.Children.Add(offer);

                Button offerButton = new Button()
                {
                    Background = Brushes.White,
                    Opacity = 0.1
                };
                Grid.SetRow(offerButton, i + 1);
                Offers.Children.Add(offerButton);
            }
        }
    }
}
