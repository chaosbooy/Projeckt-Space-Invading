using SpaceInvading.Pages;
using SpaceInvading.Resources.Classes;
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
        // dane do ofert
        List<Item> offerSource = new List<Item>() { AllItems.SlimeDrop, AllItems.SpiderDrop, AllItems.SkeletonDrop, AllItems.Boss_1_drop_1, AllItems.Boss_1_drop_2, AllItems.Boss_1_drop_3 };
        // dane do listy itemów
        List<Item> ListofItems = new List<Item>() { AllItems.Coin, AllItems.SlimeDrop, AllItems.SpiderDrop, AllItems.SkeletonDrop, AllItems.Boss_1_drop_1, AllItems.Boss_1_drop_2, AllItems.Boss_1_drop_3 };

        public Shop()
        {
            InitializeComponent();
            CreateOffers();
            CreateItemList();
        }

        private void CreateOffers()
        {
            for (int i = 0; i < offerSource.Count; i++)
            {
                Grid offer = new Grid
                {
                    Name = "offer" + i.ToString(),
                    Background = Brushes.LightGray,
                    VerticalAlignment = VerticalAlignment.Top
                };
                offer.MouseEnter += Offer_MouseEnter;
                offer.MouseLeave += Offer_MouseLeave;

                offer.MouseLeftButtonDown += Offer_MouseLeftButtonDown;

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
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(5)
                };
                Image item = new Image
                {
                    Source = offerSource[i].Sprite.Source,
                    Width = 43,
                    Height = 43,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(7)
                };

                Label content = new Label
                {
                    Content = offerSource[i].Name + " x" + Inventory.GetItemCount(offerSource[i].Name)+ '\n' + "Price: " + offerSource[i].Worth.ToString(),
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FontSize = 30,
                    Margin = new Thickness(65,0,0,0)
                };

                Image priceItem = new Image
                {
                    Source = offerSource[i].WorthItem.Sprite.Source,
                    Width = 30,
                    Height = 30,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(220, 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                };


                TextBox description = new TextBox
                {
                    Text = offerSource[i].Description,
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    FontSize = 20,
                    Margin = new Thickness(300, 0, 0, 0),
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    Background = Brushes.Transparent,
                    Width = 200,
                    IsEnabled = false,
                };

                Grid.SetColumn(frame, 0);
                Grid.SetColumn(item, 0);
                offer.Children.Add(item);
                offer.Children.Add(frame);
                offer.Children.Add(content);
                offer.Children.Add(priceItem);
                offer.Children.Add(description);
                offerList.Children.Add(offer);
            }
        }

        private void Offer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // sprzedaz dropu

            Grid clickedOffer = (Grid)sender;
            int offerNr = Int32.Parse(clickedOffer.Name.Remove(0, 5));
            Item offerItem = offerSource[offerNr];
            Inventory.RemoveItem(offerItem, 1);

            Inventory.AddItem(AllItems.Coin, offerItem.Worth);
            // pokazanie zmian w ekwipunku gracza na ekranie sklepu
            CreateItemList();
            ItemListRefresh();
        }

        private void CreateItemList()
        {
            // reset listy
            ItemList.Children.Clear();
            for(int i = 0; i < offerSource.Count; i++)
            {
                if (Inventory.ItemCount.ContainsKey(ListofItems[i].Name))
                {
                    Grid itemHolder = new Grid
                    {
                        Name = "offer" + i.ToString(),
                        Background = Brushes.LightGray,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    itemHolder.MouseEnter += Offer_MouseEnter;
                    itemHolder.MouseLeave += Offer_MouseLeave;

                    Rectangle border = new Rectangle
                    {
                        Stroke = Brushes.White,
                        StrokeThickness = 1
                    };
                    itemHolder.Children.Add(border);

                    Image item = new Image
                    {
                        Source = ListofItems[i].Sprite.Source,
                        Width = 43,
                        Height = 43,
                        Stretch = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(7)
                    };

                    TextBlock itemNumber = new TextBlock
                    {
                        Text = Inventory.ItemCount[ListofItems[i].Name].ToString(),
                    };
                    itemHolder.Children.Add(item);
                    itemHolder.Children.Add(itemNumber);

                    ItemList.Children.Add(itemHolder);
                }

            }
        }

        private void ItemListRefresh()
        {
            int i = 0;
            foreach(var itemNumberInfo in ItemList.Children)
            {
                if(itemNumberInfo.GetType() == typeof(TextBlock))
                {
                    TextBlock itemPos = itemNumberInfo as TextBlock;
                    itemPos.Text = Inventory.ItemCount[ListofItems[i].Name].ToString();
                    i++;
                }

            }
        }

        private void Offer_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = Brushes.LightGray;
        }

        private void Offer_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = Brushes.DimGray;
        }


    }
}