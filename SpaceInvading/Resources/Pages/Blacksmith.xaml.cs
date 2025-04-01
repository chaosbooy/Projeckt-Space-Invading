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
    /// Logika interakcji dla klasy Blacksmith.xaml
    /// </summary>
    public partial class Blacksmith : Page
    {
        List<Item> offerSource = new List<Item>() { AllItems.Armor_1, AllItems.Armor_2, AllItems.Armor_3, AllItems.Barrier_1, AllItems.Barrier_2, AllItems.Barrier_3, AllItems.Crossbow, AllItems.Gun, AllItems.EnchantedSword};
        List<Item> itemsDistincs = new List<Item>();
        // dane do listy itemów
        List<Item> ListofItems = new List<Item>();
        public Blacksmith()
        {
            InitializeComponent();
            
            CreateOffers();
            CreateItemList();
            ItemListRefresh();
        }

        private void CreateOffers()
        {
            foreach (Item item in Inventory.PermamentUpgrades)
            {
                offerSource.Remove(item);
            }
            itemsDistincs = Inventory.RemoveSameName(offerSource);
            for(int i = 0; i < itemsDistincs.Count; i++)
            {
                Grid offer = new Grid
                {
                    Name = "offer" + i.ToString(),
                    Background = Brushes.LightGray,
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
                    Margin = new Thickness(5)
                };
                Image item = new Image
                {
                    Source = itemsDistincs[i].Sprite.Source,
                    Width = 43,
                    Height = 43,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(7)
                };

                Label content = new Label
                {
                    Content = itemsDistincs[i].Name + '\n' + "Price: " + itemsDistincs[i].Worth.ToString(),
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FontSize = 30,
                    Margin = new Thickness(65, 0, 0, 0)
                };

                Image priceItem = new Image
                {
                    Source = itemsDistincs[i].WorthItem.Sprite.Source,
                    Width = 30,
                    Height = 30,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(220, 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                };

                TextBox description = new TextBox
                {
                    Text = itemsDistincs[i].Description,
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    FontSize = 20,
                    Margin = new Thickness(300, 0, 0, 0),
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

        private void CreateItemList()
        {
            ListofItems.Clear();
            ListofItems.AddRange(Inventory.GetItemsForShop('n'));
            ListofItems.AddRange(Inventory.GetItemsForShop('m'));
            ListofItems.AddRange(Inventory.GetItemsForShop('b'));
            for (int i = 0; i < ListofItems.Count; i++)
            {
                Grid itemHolder = new Grid
                {
                    Name = "offer" + i.ToString(),
                    Background = Brushes.LightGray,
                    VerticalAlignment = VerticalAlignment.Top,
                    ToolTip = ListofItems[i].Name + ": " + ListofItems[i].Description,
                };
                itemHolder.MouseEnter += Offer_MouseEnter;
                itemHolder.MouseLeave += Offer_MouseLeave;

                Rectangle border = new Rectangle
                {
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                itemHolder.Children.Add(border);

                Image frame = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Item_frame_empty.png")),
                    Width = 48,
                    Height = 48,
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5)
                };
                Image item = new Image();
                if (ListofItems[i].Name == "Barrier Upgrade")
                {
                    item = new Image
                    {
                        Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Images/misc/Upgrade_Barrier_{Inventory.AllItems[ListofItems[i].Name].ToString()}.png")),
                        Width = 43,
                        Height = 43,
                        Stretch = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(7)
                    };
                }
                else
                {
                    item = new Image
                    {
                        Source = ListofItems[i].Sprite.Source,
                        Width = 43,
                        Height = 43,
                        Stretch = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(7)
                    };
                }



                TextBlock itemNumber = new TextBlock
                {
                    Text = Inventory.AllItems[ListofItems[i].Name].ToString(),
                    Margin = new Thickness(55, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 30,
                };
                itemHolder.Children.Add(frame);
                itemHolder.Children.Add(item);
                itemHolder.Children.Add(itemNumber);

                ItemList.Children.Add(itemHolder);
            }
        }

        private void ItemListRefresh()
        {
            int i = 0;
            foreach (var itemNumberInfo in ItemList.Children)
            {
                if (itemNumberInfo.GetType() == typeof(TextBlock))
                {
                    TextBlock itemPos = itemNumberInfo as TextBlock;
                    itemPos.Text = Inventory.ItemCount[ListofItems[i].Name].ToString();
                    i++;
                }

            }
        }
        private void Offer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Grid clickedOffer = (Grid)sender;
            int offerNr = Int32.Parse(clickedOffer.Name.Remove(0, 5));
            Item offerItem = itemsDistincs[offerNr];
            if (offerItem.isUsable)
            {
                if(Inventory.GetItemCount(offerItem.WorthItem.Name) >= offerItem.Worth)
                {
                    Inventory.AddUsableUpgrade(offerItem);
                    Inventory.RemoveItem(offerItem.WorthItem, offerItem.Worth);
                }
            }
            else 
            {
                if (Inventory.GetItemCount(offerItem.WorthItem.Name) >= offerItem.Worth)
                {
                    Inventory.AddPermanentUpgrade(offerItem);
                    Inventory.RemoveItem(offerItem.WorthItem, offerItem.Worth);
                }
            }

            offerList.Children.Clear();
            ItemList.Children.Clear();
            CreateItemList();
            ItemListRefresh();
            CreateOffers();
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
