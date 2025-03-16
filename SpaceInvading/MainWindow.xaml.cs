using SpaceInvading.Pages;
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
        }

        private void MainContent_Navigated(object sender, NavigationEventArgs e)
        {
            if(MainContent.Content.GetType() == new Game().GetType())
            {
                this.Height = 650;
            }
            else
            {
                this.Height = 450;
            }
        }
    }
}