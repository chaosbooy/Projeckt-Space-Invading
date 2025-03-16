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
        #region Variables

        private const bool HitBoxShow = false;

        public Player Player1 = new();
        private Image playerState = new();

        // przeciwnicy na planszy
        private List<Border> enemiesState = new();

        private List<Image> bullets = new();
        private List<Image> enemyBullets = new();

        // prędkość co tick
        private double playerSpeed = 2.5;
        private double bulletSpeed = 5;
        private double enemySpeed = 2.2;
        // co ktory tick mają ruszyć się wrogowie
        private double enemiesMoveTick = 10;
        private double TickNumber = 0;

        private bool playerLeft = false;
        private bool playerRight = false;
        // kierunek ruchu przeciwników
        private Direction enemiesMoveDirection = Direction.Left;
        private KeyState playerAttack = KeyState.Up;
        // numer klatki gracza
        private int playerSpriteNumber = 1;
        // animacja ataku gracza
        DispatcherTimer playerAttackAnimation = new DispatcherTimer();
        // animacja chodzenia gracza
        DispatcherTimer playerWalkAnimation = new DispatcherTimer();
        // numer klatki animacji ataku gracza
        private int playerAttackSprite = 0;
        //numer klatki animacji pocisku gracza
        private int playerBulletSprite = 1;

        #endregion


        public Game()
        {
            InitializeComponent();
            SetupGame(3, 10);
            CompositionTarget.Rendering += GameLoop;

            playerAttackAnimation.Tick += AttackAnimation;
            playerAttackAnimation.Interval = new TimeSpan(0, 0, 0, 0, 80);

            playerWalkAnimation.Tick += WalkAnimation;
            playerWalkAnimation.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void AttackAnimation(object? sender, EventArgs e)
        {
            if (playerAttackSprite < 4) playerAttackSprite++;
            // koniec klatek - koniec animacji, ustaw domyslny wyglad
            else
            {
                playerAttackSprite = 1;

                //jezeli gracz rusza sie w jedna z dwoch stron
                //jezeli gracz rusza sie w jedna z dwoch stron
                if(playerLeft != playerRight) 
                    playerWalkAnimation.Start();
                else 
                    playerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/player_still.png"));

                playerAttackAnimation.Stop();
            }
            playerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player_attack_" + playerAttackSprite.ToString() + ".png"));
        }

        private void WalkAnimation(object? sender, EventArgs e)
        {
            if (playerSpriteNumber < 4) playerSpriteNumber++;
            else playerSpriteNumber = 1;
            playerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player_side_" + playerSpriteNumber.ToString() + ".png"));
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
            // Debug.WriteLine($"Xaml succesfully loaded and key events activated");
        }

        private void SetupGame(int enemyRows, int enemyCols)
        {
            EnemyHolder.ColumnDefinitions.Clear();
            EnemyHolder.RowDefinitions.Clear();

            for (int i = 0; i < enemyCols; ++i)
                EnemyHolder.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 60 });

            for (int i = 0; i < enemyRows; ++i)
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

            for (int i = 0; i < enemyRows; i++)
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

        private void GameLoop(object? sender, EventArgs e)
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

                bullet.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player_bullet_" + playerBulletSprite.ToString() + ".png"));
                if (playerBulletSprite < 2) playerBulletSprite++;
                else playerBulletSprite = 1;

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
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + bulletSpeed/4);
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
                if (Canvas.GetLeft(EnemyHolder) <= 0 && enemiesMoveDirection == Direction.Left ||
                    Canvas.GetLeft(EnemyHolder) > MainCanvas.Width - (EnemyHolder.ColumnDefinitions.Count * EnemyHolder.ColumnDefinitions[0].ActualWidth)
                    && enemiesMoveDirection == Direction.Right)
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

            // strzał wrogów ( powiedzmy co 30 tick )
            if (TickNumber % 30 == 0)
            {
                foreach (var enemy in enemiesState.ToArray())
                {
                    Random rnd = new Random();
                    int shootChance = 15;
                    if (rnd.Next(1001) <= shootChance)
                    {
                        //var a = enemiesState.IndexOf(enemy);
                        ShootEnemy(enemy);

                    }
                }
            }

        }

        private bool IsColliding(FrameworkElement a, FrameworkElement b)
        {
            Point positionA = a.PointToScreen(new Point(0d, 0d));
            Point positionB = b.PointToScreen(new Point(0d, 0d));

            double aX = positionA.X;
            double aY = positionA.Y;
            double bX = positionB.X;
            double bY = positionB.Y;

            return aX < bX + b.ActualWidth && aX + a.ActualWidth > bX && aY < bY + b.ActualHeight && aY + a.ActualHeight > bY;
        }

        #region Player Input
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    playerLeft = true;
                    playerState.RenderTransform = new ScaleTransform(-1, 1);
                    break;
                case Key.D:
                    playerRight = true;
                    playerState.RenderTransform = new ScaleTransform(1, 1);
                    break;
                case Key.Space:
                    playerAttackAnimation.Start();
                    playerWalkAnimation.Stop();
                    if (playerAttack == KeyState.Pressed || playerAttack == KeyState.Down)
                    {
                        playerAttack = KeyState.Down;
                    }
                    else
                        playerAttack = KeyState.Pressed;
                    break;
            }

            //jezeli gracz rusza sie w jedna z dwoch stron
            if (playerLeft != playerRight && !playerWalkAnimation.IsEnabled)
            {
                playerWalkAnimation.Start();

                WalkAnimation(null, new EventArgs());
            }
            else if (playerLeft == playerRight)
            {
                playerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/player_still.png"));
                playerWalkAnimation.Stop();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
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

            //jezeli gracz rusza sie w jedna z dwoch stron
            if (playerLeft != playerRight && !playerWalkAnimation.IsEnabled)
            {
                playerWalkAnimation.Start();

                WalkAnimation(null, new EventArgs());
            }
            else if (playerLeft == playerRight)
            {
                playerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/player_still.png"));
                playerWalkAnimation.Stop();
            }
        }

        #endregion

        private void Shoot()
        {
            Image bullet = new Image
            {
                Width = 32,
                Height = 16,
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player_bullet_1.png"))
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
            Image bullet = new Image
            {
                Width = 32,
                Height = 16,
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime_bullet_1.png"))
            };
            // Get the position of the shooter relative to the canvas
            Point shooterPositionRelativeToCanvas = shooter.TranslatePoint(new Point(0, 0), MainCanvas);

            // Calculate the bullet's position
            double canvasLeft = shooterPositionRelativeToCanvas.X + shooter.Width / 2 - bullet.Width / 2;
            double canvasTop = shooterPositionRelativeToCanvas.Y - bullet.Height + 50;

            // Set the bullet's position relative to the canvas
            Canvas.SetLeft(bullet, canvasLeft);
            Canvas.SetTop(bullet, canvasTop);


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