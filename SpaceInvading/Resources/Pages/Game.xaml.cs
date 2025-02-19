using SpaceInvading.Resources.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpaceInvading.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private const bool TEST = true;

        public Player Player1 = new();
        private Image playerState = new();

        private List<Entity> enemies = new();
        private List<Border> enemiesState = new();

        private List<Rectangle> bullets = new();

        private DispatcherTimer gameTimer = new();
        private double playerSpeed = 10;
        private double bulletSpeed = 5;
        private double enemySpeed = 1;

        private bool playerLeft = false;
        private bool playerRight = false;
        private KeyState playerAttack = KeyState.Up;

        public Game()
        {
            InitializeComponent();
            SetupGame();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void XamlLoaded(object sender, RoutedEventArgs e) 
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;
            MainCanvas.Focus();
            // Debug.WriteLine($"Xaml succesfully loaded and key events activated");
        }

        private void SetupGame()
        {
            playerState = new Image 
            { 
                Width = 100, 
                Height = 100, 
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/player_still.png"))
            };
            Canvas.SetLeft(playerState, (MainCanvas.Width - playerState.Width) / 2);
            Canvas.SetTop(playerState, MainCanvas.Height - playerState.Height - 10);
            MainCanvas.Children.Add(playerState);

            var enemyWidth = 100;
            var enemyHeight = 100;
            var enemySpacing = 20;

            for (int i = 0; i < 5; i++)
            {
                Border border = new Border
                {
                    Width = enemyWidth,
                    Height = enemyHeight,
                    Background = TEST ? Brushes.Black : Brushes.Transparent,
                };

                Image block = new Image
                {
                    Width = enemyWidth,
                    Height = enemyHeight,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime_Still.png"))
                };

                border.Child = block; // Dodaj obraz jako dziecko ramki

                Canvas.SetLeft(border, i * (enemyWidth + enemySpacing) + 20);
                Canvas.SetTop(border, 20);
                MainCanvas.Children.Add(border);
                enemiesState.Add(border);
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (playerLeft && Canvas.GetLeft(playerState) > 0)
            {
                Canvas.SetLeft(playerState, Canvas.GetLeft(playerState) - playerSpeed);
            }
            if (playerRight && Canvas.GetLeft(playerState) < MainCanvas.Width - playerState.Width)
            {
                Canvas.SetLeft(playerState, Canvas.GetLeft(playerState) + playerSpeed);
            }
            if (playerAttack == KeyState.Pressed)
            {
                playerAttack = KeyState.Down;
                Shoot();
            }

            foreach (var bullet in bullets.ToArray())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) - bulletSpeed);
                if (Canvas.GetTop(bullet) < 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
            }

            foreach (var block in enemiesState.ToArray())
            {
                foreach (var bullet in bullets.ToArray())
                {
                    if (IsColliding(bullet, block))
                    {
                        MainCanvas.Children.Remove(bullet);
                        MainCanvas.Children.Remove(block);
                        bullets.Remove(bullet);
                        enemiesState.Remove(block);
                        break;
                    }
                }
            }
        }

        private bool IsColliding(Rectangle a, Border b)
        {
            double aX = Canvas.GetLeft(a);
            double aY = Canvas.GetTop(a);
            double bX = Canvas.GetLeft(b);
            double bY = Canvas.GetTop(b);
            return aX < bX + b.Width && aX + a.Width > bX && aY < bY + b.Height && aY + a.Height > bY;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.A:
                    playerLeft = true;
                    break;
                case Key.D:
                    playerRight = true;
                    break;
                case Key.Space:
                    if (playerAttack == KeyState.Pressed || playerAttack == KeyState.Down)
                        playerAttack = KeyState.Down;
                    else
                        playerAttack = KeyState.Pressed;
                    break;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.A:
                    playerLeft = false;
                    break;
                case Key.D:
                    playerRight = false;
                    break;
                case Key.Space:
                    playerAttack = KeyState.Released;
                    break;
            }
        }

        private void Shoot()
        {
            Rectangle bullet = new Rectangle 
            { 
                Width = 5, 
                Height = 15, 
                Fill = Brushes.Black 
            };
            double x = Canvas.GetLeft(playerState) + playerState.Width / 2 - bullet.Width / 2;
            double y = Canvas.GetTop(playerState) - bullet.Height;
            Canvas.SetLeft(bullet, x);
            Canvas.SetTop(bullet, y);
            MainCanvas.Children.Add(bullet);
            bullets.Add(bullet);
        }
    }

    enum KeyState
    {
        Down,
        Pressed,
        Up,
        Released
    }
}
