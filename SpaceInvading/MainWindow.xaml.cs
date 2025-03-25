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