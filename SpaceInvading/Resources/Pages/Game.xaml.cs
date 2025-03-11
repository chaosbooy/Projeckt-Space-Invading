using SpaceInvading.Resources.Classes;
using System.Diagnostics;
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
        private const bool HitBoxShow = true;

        public Player Player1 = new();
        private Image playerState = new();

        // przeciwnicy na planszy
        private List<Entity> enemies = new();
        private List<Border> enemiesState = new();

        private List<Rectangle> bullets = new();
        private List<Rectangle> enemyBullets = new();
    
        private DispatcherTimer gameTimer = new();
        // prędkość co tick
        private double playerSpeed = 10;
        private double bulletSpeed = 5;
        private double enemySpeed = 10;
        // co ktory tick mają ruszyć się wrogowie
        private double enemiesMoveTick = 10;
        private double TickNumber = 0;

        private bool playerLeft = false;
        private bool playerRight = false;
        // kierunek ruchu przeciwników
        private Direction enemiesMoveDirection = Direction.Left;
        private KeyState playerAttack = KeyState.Up;

        public Game()
        {
            InitializeComponent();
            SetupGame(3, 10);
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

            if (EnemyHolder.Parent is Canvas)
            {
                Canvas.SetLeft(EnemyHolder, 20);
                Canvas.SetTop(EnemyHolder, 20);
            }
        }

        private void SetupGame(int enemyRows, int enemyCols)
        {
            EnemyHolder.ColumnDefinitions.Clear();
            EnemyHolder.RowDefinitions.Clear();

            for (int i = 0; i < enemyCols; ++i)
                EnemyHolder.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 60 });

            for(int i = 0; i < enemyRows; ++i)
                EnemyHolder.RowDefinitions.Add(new RowDefinition { MinHeight = 60 });


            playerState = new Image 
            { 
                Width = 100, 
                Height = 100, 
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/player_still.png"))
            };
            Canvas.SetLeft(playerState, (MainCanvas.Width - playerState.Width) / 2);
            Canvas.SetTop(playerState, MainCanvas.Height - playerState.Height - 10);
            MainCanvas.Children.Add(playerState);


            for(int i = 0; i < enemyRows; i++)
            {
                for (int j = 0; j < enemyCols; j++)
                {
                    Border border = new Border
                    {
                        Background = HitBoxShow ? Brushes.Black : Brushes.Transparent,
                        Width = 50,
                        Height = 50
                    };

                    Image block = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime_Still.png"))
                    };

                    border.Child = block; // Dodaj obraz jako dziecko ramki

                    Grid.SetColumn(border, j);
                    Grid.SetRow(border, i);
                    EnemyHolder.Children.Add(border);
                    enemiesState.Add(border);
                }
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            TickNumber++;

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

            // ruch pocisków
            foreach (var bullet in bullets.ToArray())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) - bulletSpeed);
                // znikanie pocisku
                if (Canvas.GetTop(bullet) < 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
            }

            // ruch pocisków wrogów
            foreach (var bullet in enemyBullets.ToArray())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + bulletSpeed);
                // znikanie pocisku
                if (Canvas.GetTop(bullet) < 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
            }

            // trafienie w przeciwnika
            foreach (var block in enemiesState.ToArray())
            {
                foreach (var bullet in bullets.ToArray())
                {
                    if (IsColliding(bullet, block))
                    {
                        MainCanvas.Children.Remove(bullet);
                        EnemyHolder.Children.Remove(block);
                        bullets.Remove(bullet);
                        enemiesState.Remove(block);
                        break;
                    }
                }
            }


            // ruch wrogów
            if (TickNumber % enemiesMoveTick == 0)
            {
                Debug.WriteLine($"The Position: { Canvas.GetLeft(EnemyHolder) }");
                Debug.WriteLine($"The Maximum Position: {EnemyHolder.ColumnDefinitions.Count * EnemyHolder.ColumnDefinitions[0].Width.Value}");
                if (Canvas.GetLeft(EnemyHolder) <= 0 && enemiesMoveDirection == Direction.Left ||
                    Canvas.GetLeft(EnemyHolder) > MainCanvas.Width - (EnemyHolder.ColumnDefinitions.Count * EnemyHolder.ColumnDefinitions[0].Width.Value) && enemiesMoveDirection == Direction.Right)
                {
                    enemiesMoveDirection = enemiesMoveDirection == Direction.Left ? Direction.Right : Direction.Left;
                    Canvas.SetTop(EnemyHolder, Canvas.GetTop(EnemyHolder) + 20);
                }

                if (enemiesMoveDirection == Direction.Left)
                    Canvas.SetLeft(EnemyHolder, Canvas.GetLeft(EnemyHolder) - enemySpeed);
                else
                    Canvas.SetLeft(EnemyHolder, Canvas.GetLeft(EnemyHolder) + enemySpeed);

                //foreach (var enemy in enemiesState.ToArray())
                //{
                //    // jeśli krańcowy wróg z rzędu dotyka ścian - obniż cały rząd
                //    if (Canvas.GetLeft(enemy) <= 0 && enemiesMoveDirection == Direction.Left ||
                //        Canvas.GetLeft(enemy) > MainCanvas.Width - enemy.Width && enemiesMoveDirection == Direction.Right)
                //    {
                //        // wszyscy wrogowie w rzędzie idą niżej
                //        foreach (var enemyInRow in enemiesState.ToArray())
                //        {
                //            Canvas.SetTop(enemyInRow, Canvas.GetTop(enemyInRow) + 20);
                //        }
                //        // zmiana kierunku
                //        if (enemiesMoveDirection == Direction.Left) enemiesMoveDirection = Direction.Right;
                //        else enemiesMoveDirection = Direction.Left;
                //    }
                //    // ruch w lewo lub prawo
                //    if (enemiesMoveDirection == Direction.Left) Canvas.SetLeft(enemy, Canvas.GetLeft(enemy) - enemySpeed);
                //    else Canvas.SetLeft(enemy, Canvas.GetLeft(enemy) + enemySpeed);
                //}
            }

            // strzał wrogów ( powiedzmy co 20 tick )
            if( TickNumber % 20 == 0)
            {
                foreach (var enemy in enemiesState.ToArray())
                {
                    Random rnd = new Random();
                    int shootChance = 2;
                    if (rnd.Next(101) <= shootChance)
                    {
                        ShootEnemy(enemy);
                    }
                }
            }

        }

        private bool IsColliding(Rectangle a, Border b)
        {
            Point positionA = a.PointToScreen(new Point(0d, 0d));
            Point positionB = b.PointToScreen(new Point(0d, 0d));

            double aX = positionA.X;
            double aY = positionA.Y;
            double bX = positionB.X;
            double bY = positionB.Y;

            return aX < bX + b.Width && aX + a.Width > bX && aY < bY + b.Height && aY + a.Height > bY;
        }

        #region Player Input
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

        #endregion

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

        private void ShootEnemy(Border shooter)
        {
            Rectangle bullet = new Rectangle
            {
                Width = 5,
                Height = 15,
                Fill = Brushes.Green
            };
            double x = Canvas.GetLeft(shooter) + shooter.Width / 2 - bullet.Width / 2;
            double y = Canvas.GetTop(shooter) - bullet.Height + 50;
            Canvas.SetLeft(bullet, x);
            Canvas.SetTop(bullet, y);
            MainCanvas.Children.Add(bullet);
            enemyBullets.Add(bullet);
        }
    }

    enum KeyState
    {
        Down,
        Pressed,
        Up,
        Released
    }
    enum Direction
    {
        Left,
        Right
    }
}
