using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace SpaceInvading.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private Rectangle player;
        private List<Rectangle> blocks = new();
        private List<Rectangle> bullets = new();
        private DispatcherTimer gameTimer = new();
        private double playerSpeed = 10;
        private double bulletSpeed = 5;

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
            player = new Rectangle { Width = 50, Height = 20, Fill = Brushes.Blue };
            Canvas.SetLeft(player, (MainCanvas.Width - player.Width) / 2);
            Canvas.SetTop(player, MainCanvas.Height - player.Height - 10);
            MainCanvas.Children.Add(player);

            for (int i = 0; i < 5; i++)
            {
                Rectangle block = new() { Width = 60, Height = 30, Fill = Brushes.Red };
                Canvas.SetLeft(block, i * 70 + 20);
                Canvas.SetTop(block, 20);
                MainCanvas.Children.Add(block);
                blocks.Add(block);
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (playerLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (playerRight && Canvas.GetLeft(player) < MainCanvas.Width - player.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
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

            foreach (var block in blocks.ToArray())
            {
                foreach (var bullet in bullets.ToArray())
                {
                    if (IsColliding(bullet, block))
                    {
                        MainCanvas.Children.Remove(bullet);
                        MainCanvas.Children.Remove(block);
                        bullets.Remove(bullet);
                        blocks.Remove(block);
                        break;
                    }
                }
            }
        }

        private bool IsColliding(Rectangle a, Rectangle b)
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
            Rectangle bullet = new Rectangle { Width = 5, Height = 15, Fill = Brushes.Black };
            double x = Canvas.GetLeft(player) + player.Width / 2 - bullet.Width / 2;
            double y = Canvas.GetTop(player) - bullet.Height;
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
