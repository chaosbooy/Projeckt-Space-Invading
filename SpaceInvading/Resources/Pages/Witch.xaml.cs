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
    /// Logika interakcji dla klasy Witch.xaml
    /// </summary>
    public partial class Witch : Page
    {
        public Witch()
        {
            InitializeComponent();
            CreateOffers();
        }
        List<Grid> offers = new List<Grid>();
        List<string> offersNames = new List<string>() { "health_potion", "luck_potion", "rage_potion", "shadow_potion", "shield_potion", "sword_enchant" };
        private void CreateOffers()
        {
            for (int i = 0; i < offersNames.Count; i++)
            {
                Grid offer = new Grid
                {
                    Name = "offer" + i.ToString(),
                    Background = Brushes.DimGray,
                };

                Rectangle border = new Rectangle
                {
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                offer.Children.Add(border);

                // -- tworzenie części składowych oferty --~\\

                Image frame = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Item_frame_empty.png")),
                    Width = 48,
                    Height = 48,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5)
                };
                Image item = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_" + offersNames[i] + ".png")),
                    Width = 43,
                    Height = 43,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(7)
                };
                Grid.SetColumn(frame, 0);
                Grid.SetColumn(item, 0);
                offer.Children.Add(item);
                offer.Children.Add(frame);




                offers.Add(offer);
                offerList.Children.Add(offer);
            }
        }
    }
}
