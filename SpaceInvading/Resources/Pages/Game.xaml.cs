using SpaceInvading.Resources.Classes;
using SpaceInvading.Resources.Pages;
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

        // przeciwnicy na planszy
        private List<Border> enemiesState = new();

        private List<Image> bullets = new();
        private List<Image> enemyBullets = new();

        // prędkość co tick
        private double bulletSpeed = 5;
        private double enemySpeed = 2.2;
        // co ktory tick mają ruszyć się wrogowie
        private double enemiesMoveTick = 10;
        private double TickNumber = 0;

        private int round;
        // kierunek ruchu przeciwników
        private Direction enemiesMoveDirection = Direction.Left;
        private KeyState playerAttack = KeyState.Up;


        //numer klatki animacji pocisku gracza
        private int playerBulletSprite = 1;

        #endregion

        public Game()
        {
            InitializeComponent();
            SetupGame(1, 5);

            CompositionTarget.Rendering += GameLoop;
        }

        private void XamlLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;
            MainCanvas.Focus();

            // Debug.WriteLine($"Xaml succesfully loaded and key events activated");
        }

        private void SetupGame(int enemyRows, int enemyCols)
        {
            Canvas.SetLeft(EnemyHolder, 20);
            Canvas.SetTop(EnemyHolder, 20);

            EnemyHolder.ColumnDefinitions.Clear();
            EnemyHolder.RowDefinitions.Clear();
            

            for (int i = 0; i < enemyCols; ++i)
                EnemyHolder.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 60 });

            for (int i = 0; i < enemyRows; ++i)
                EnemyHolder.RowDefinitions.Add(new RowDefinition { MinHeight = 60 });

            if (round == 0)
            {
                Player1 = new Player();
                Canvas.SetLeft(Player1.PlayerState, (MainCanvas.Width - Player1.PlayerState.Width) / 2);
                Canvas.SetTop(Player1.PlayerState, MainCanvas.Height - Player1.PlayerState.Height - 10);

                MainCanvas.Children.Add(Player1.PlayerState);
            }

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

        private void EndGame()
        {
            this.NavigationService.Navigate(new Lobby());
            CompositionTarget.Rendering -= GameLoop;
        }

        private void SetupBoss()
        {

        }

        private void SetupNewRound()
        {
            round++;
            if (round == 5)
                SetupBoss();
            else if (round % 5 == 0)
                SetupBoss();
            if (round < 2)
                SetupGame(1, 10);
            else if (round < 5)
                SetupGame(2, 10);
            else
                SetupGame(3, 10);
            return;
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            TickNumber++;
            if (Player1.PlayerLeft && Canvas.GetLeft(Player1.PlayerState) > Player1.PlayerState.ActualWidth * 0.7)
            {
                Canvas.SetLeft(Player1.PlayerState, Canvas.GetLeft(Player1.PlayerState) - Player1.PlayerSpeed);
            }
            if (Player1.PlayerRight && Canvas.GetLeft(Player1.PlayerState) < MainCanvas.ActualWidth - Player1.PlayerState.ActualWidth * 2)
            {
                Canvas.SetLeft(Player1.PlayerState, Canvas.GetLeft(Player1.PlayerState) + Player1.PlayerSpeed);
            }
            if (playerAttack == KeyState.Pressed && !Player1.IsAttacking)
            {
                playerAttack = KeyState.Down;
                Shoot();
                Player1.Attack();
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

            // trafienie w przeciwnika
            foreach (var block in enemiesState.ToArray())
            {
                foreach (var bullet in bullets.ToArray())
                {
                    if (IsColliding(bullet, 0.9, block))
                    {
                        MainCanvas.Children.Remove(bullet);
                        EnemyHolder.Children.Remove(block);
                        bullets.Remove(bullet);
                        enemiesState.Remove(block);
                        break;
                    }
                }
            }

            if (enemiesState.Count == 0)
                SetupNewRound();

            // ruch pocisków wrogów
            foreach (var bullet in enemyBullets.ToArray())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + bulletSpeed/4);
                // znikanie pocisku za swiatem
                if (Canvas.GetTop(bullet) < 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
                //trafienie gracza
                else if(IsColliding(Player1.PlayerState, 0.7, bullet))
                {
                    //jezeli nastepne hp bedzie 0 koncz gre
                    if (Player1.Health-- == 1) EndGame();
                    else
                    {
                        enemyBullets.Remove(bullet);
                        MainCanvas.Children.Remove(bullet);
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

        private bool IsColliding(FrameworkElement a, double hitboxMultiplier, FrameworkElement b)
        {
            GeneralTransform transformA = a.TransformToAncestor(MainCanvas);
            Point positionA = transformA.Transform(new Point(0, 0));

            GeneralTransform transformB = b.TransformToAncestor(MainCanvas);
            Point positionB = transformB.Transform(new Point(0, 0));

            double newWidth = a.ActualWidth * hitboxMultiplier;
            double newHeight = a.ActualHeight * hitboxMultiplier;

            double aX = positionA.X - (newWidth - a.ActualWidth) / 2;
            double aY = positionA.Y - (newHeight - a.ActualHeight) / 2;

            double bX = positionB.X;
            double bY = positionB.Y;

            return aX < bX + b.ActualWidth && aX + newWidth > bX &&
                   aY < bY + b.ActualHeight && aY + newHeight > bY;
        }



        #region Player Input
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    Player1.TurnLeft(true);
                    break;
                case Key.D:
                    Player1.TurnRight(true);
                    break;
                case Key.Space:
                    if (playerAttack == KeyState.Pressed || playerAttack == KeyState.Down)
                        playerAttack = KeyState.Down;
                    else
                        playerAttack = KeyState.Pressed;
                    break;
                case Key.F:
                    //// for debugging
                    //CompositionTarget.Rendering -= GameLoop;

                    break;
            }

            if (Player1.IsAttacking) return;
            //jezeli gracz rusza sie w jedna z dwoch stron
            if (Player1.PlayerLeft != Player1.PlayerRight)
                Player1.Walk();
            else
                Player1.Stay();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    Player1.TurnLeft(false);
                    break;
                case Key.D:
                    Player1.TurnRight(false);
                    break;
                case Key.Space:
                    playerAttack = KeyState.Released;
                    break;
            }

            if (Player1.IsAttacking) return;
            //jezeli gracz rusza sie w jedna z dwoch stron
            if (Player1.PlayerLeft != Player1.PlayerRight)
                Player1.Walk();
            else
                Player1.Stay();
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
            double x = Canvas.GetLeft(Player1.PlayerState) + Player1.PlayerState.Width / 2 - bullet.Width / 2;
            double y = Canvas.GetTop(Player1.PlayerState) - bullet.Height;
            Canvas.SetLeft(bullet, x);
            Canvas.SetTop(bullet, y);
            MainCanvas.Children.Add(bullet);
            bullets.Add(bullet);
        }

        private void ShootEnemy(FrameworkElement shooter)
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