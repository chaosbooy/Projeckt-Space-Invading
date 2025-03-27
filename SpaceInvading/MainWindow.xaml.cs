using SpaceInvading.Pages;
using SpaceInvading.Resources.Classes;
using System.Windows;
using System.Windows.Navigation;

namespace SpaceInvading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Inventory.AddUpgrade((Item)AllItems.Barrier_1.Clone()); // debug line to delete later
            Inventory.AddUpgrade((Item)AllItems.Barrier_2.Clone());
            Inventory.AddUpgrade((Item)AllItems.Barrier_3.Clone());
            Inventory.AddUpgrade((Item)AllItems.Armor_1.Clone()); 
            Inventory.AddUpgrade((Item)AllItems.Armor_2.Clone());
            Inventory.AddUpgrade((Item)AllItems.Armor_3.Clone());
        }

        private void MainContent_Navigated(object sender, NavigationEventArgs e)
        {
            if(MainContent.Content.GetType() == new Game().GetType())
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowStyle = WindowStyle.None;
            }
            else
            {
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }
}