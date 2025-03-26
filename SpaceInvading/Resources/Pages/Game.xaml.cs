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

        private readonly bool HitBoxShow = false;
        private bool gamePaused;

        public Player Player1;

        // przeciwnicy na planszy
        private List<Enemy> Enemies = new();

        private List<Image> bullets = new();
        private List<Projectile> enemyBullets = new();

        // prędkość co tick
        private double bulletSpeed = 5;
        private double enemySpeed = 0.7;
        // co ktory tick mają ruszyć się wrogowie
        private double TickNumber = 0;

        private int round;
        // kierunek ruchu przeciwników
        private Direction enemiesMoveDirection = Direction.Left;
        private KeyState playerAttack = KeyState.Up;


        //numer klatki animacji pocisku gracza
        private int playerBulletSprite = 1;
        List<Obstacle> obstacles = new List<Obstacle>();
        #endregion


        public Game()
        {
            InitializeComponent();

            Player1 = new Player(HitBoxShow);
            UpdateHealthBar();
            Canvas.SetLeft(Player1.PlayerHitBoxes, (MainCanvas.Width - Player1.PlayerHitBoxes.Width) / 2 + 50);
            Canvas.SetTop(Player1.PlayerHitBoxes, MainCanvas.Height - Player1.PlayerHitBoxes.Height - 10);

            MainCanvas.Children.Add(Player1.PlayerHitBoxes);
            Player1.TurnLeft(true);
            Player1.TurnLeft(false);

            SetupGame(1, 5);

            CompositionTarget.Rendering -= GameLoop;
            CompositionTarget.Rendering += GameLoop;

        }

        private void XamlLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            
            window.KeyDown -= Window_KeyDown;
            window.KeyUp -= Window_KeyUp;

            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;

            MainCanvas.Focus();
        }

        private void PauseGame(object sender, RoutedEventArgs e)
        {
            gamePaused = !gamePaused;
            CompositionTarget.Rendering -= GameLoop;

            if (gamePaused)
            {
                PausePanel.Visibility = Visibility.Visible;

                return;
            }

            PausePanel.Visibility = Visibility.Collapsed;
            CompositionTarget.Rendering += GameLoop;

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

            Random rnd = new Random();

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

                    var block = new Enemy();
                    switch(rnd.Next(2))
                    {
                        case 0:
                            block = (Enemy)AllEnemies.Spider.Clone();
                            break;
                        case 1:
                            block = (Enemy)AllEnemies.Slime.Clone();
                            break;
                    }
                    border.Child = block.EnemyState; // Dodaj obraz jako dziecko ramki

                    Grid.SetColumn(border, j);
                    Grid.SetRow(border, i);
                    EnemyHolder.Children.Add(border);
                    Enemies.Add(block);
                }
            }

            CreateObstacle(150, 400);
            CreateObstacle(350, 400);
            CreateObstacle(550, 400);
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
            if (Player1.PlayerLeft && Canvas.GetLeft(Player1.PlayerHitBoxes) > 0)
            {
                Canvas.SetLeft(Player1.PlayerHitBoxes, Canvas.GetLeft(Player1.PlayerHitBoxes) - Player1.PlayerSpeed);
            } else if (Player1.PlayerRight && Canvas.GetLeft(Player1.PlayerHitBoxes) < MainCanvas.ActualWidth - Player1.PlayerHitBoxes.ActualWidth * 0.7)
            {
                Canvas.SetLeft(Player1.PlayerHitBoxes, Canvas.GetLeft(Player1.PlayerHitBoxes) + Player1.PlayerSpeed);
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

                bullet.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/Player_bullet_" + playerBulletSprite.ToString() + ".png"));
                if (playerBulletSprite < 2) playerBulletSprite++;
                else playerBulletSprite = 1;

                Point bulletPos = bullet.TranslatePoint(new Point(0, 0), this);
                // znikanie pocisku
                if (bulletPos.Y <= 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
            }

            // trafienie pocisku gracza w przeszkode
            foreach (var bullet in bullets.ToArray())
            {
                // okresla czy przycisk jest na ekranie
                bool isBulletAlive = true;
                for (int i = 0; i < obstacles.Count; i++)
                {
                    // sprawdzanie czesci w kazdej przeszkodzie
                    for (int j = 0; j < obstacles[i].Parts.Count(); j++)
                    {
                        if (obstacles[i].Damages[j] != -1)
                        {
                            if (isBulletAlive && IsColliding(obstacles[i].Parts[j], 1, bullet))
                            {
                                bool ifDestroyed = obstacles[i].DamagePart(j);
                                bullets.Remove(bullet);
                                MainCanvas.Children.Remove(bullet);
                                if (ifDestroyed)
                                {
                                    MainCanvas.Children.Remove(obstacles[i].Parts[j]);
                                }
                                isBulletAlive = false;
                            }
                        }
                    }
                }
            }

            // trafienie w przeciwnika
            foreach (var block in Enemies.ToArray())
            {
                foreach (var bullet in bullets.ToArray())
                {
                    if (IsColliding(bullet, 0.9, block.EnemyState))
                    {
                        MainCanvas.Children.Remove(bullet);
                        EnemyHolder.Children.Remove((UIElement)block.EnemyState.Parent);
                        bullets.Remove(bullet);
                        Enemies.Remove(block);

                        while (RemoveEmptyColumn(0))
                            Canvas.SetLeft(EnemyHolder, Canvas.GetLeft(EnemyHolder) + 60);
                        while (RemoveEmptyColumn(EnemyHolder.ColumnDefinitions.Count - 1));
                        break;
                    }
                }
            }

            if (Enemies.Count == 0)
                SetupNewRound();

            // ruch pocisków wrogów
            foreach (var projectile in enemyBullets.ToArray())
            {
                Image bullet = projectile.ProjectileState;

                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + projectile.Speed);
                // znikanie pocisku za swiatem
                if (Canvas.GetTop(bullet) < 0)
                {
                    MainCanvas.Children.Remove(bullet);
                    bullets.Remove(bullet);
                }
                //trafienie gracza
                else if(IsColliding(Player1.PlayerHitBoxes, 0.7, bullet))
                {
                    Player1.Health -= projectile.Damage;
                    UpdateHealthBar();
                    //jezeli nastepne hp bedzie 0 koncz gre
                    if (Player1.Health <= 0) EndGame();
                    else
                    {
                        enemyBullets.Remove(projectile);
                        MainCanvas.Children.Remove(bullet);
                    }
                }
                // trafienie w przeszkodę
                else if (obstacles.Count != 0)
                {
                    // okresla czy przycisk jest na ekranie
                    bool isBulletAlive = true;
                    // sprawdzanie duzych przeszkod
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        // sprawdzanie czesci w kazdej przeszkodzie
                        for (int j = 0; j < obstacles[i].Parts.Count(); j++)
                        {
                            if (obstacles[i].Damages[j] != -1)
                            {
                                if (isBulletAlive && IsColliding(obstacles[i].Parts[j], 1, bullet))
                                {
                                    bool ifDestroyed = obstacles[i].DamagePart(j);
                                    enemyBullets.Remove(projectile);
                                    MainCanvas.Children.Remove(bullet);
                                    if (ifDestroyed)
                                    {
                                        MainCanvas.Children.Remove(obstacles[i].Parts[j]);
                                    }
                                    isBulletAlive = false;
                                }
                            }
                        }
                    }
                }
            }

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

            // strzał wrogów ( powiedzmy co 30 tick )
            if (TickNumber % 30 == 0)
            {
                foreach (var enemy in Enemies.ToArray())
                {
                    Random rnd = new Random();
                    int shootChance = 15;
                    if (rnd.Next(101) <= shootChance)
                    {
                        //var a = enemiesState.IndexOf(enemy);
                        ShootEnemy(enemy);

                    }
                }
            }

        }
        private void UpdateHealthBar()
        {
            if(Player1.Health <0 || Player1.Health > 5) { return; }
            ImageHealthBar.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HealthBar/Health_" + Player1.MaxHealth + "_" + Player1.Health + ".png"));
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

        private bool RemoveEmptyColumn(int columnIndex)
        {
            if (EnemyHolder.ColumnDefinitions.Count <= columnIndex || columnIndex < 0) return false;

            // Check if any child belongs to the given column
            bool columnHasElements = EnemyHolder.Children
                .OfType<UIElement>()
                .Any(child => Grid.GetColumn(child) == columnIndex);

            // If no elements are in the column, remove it
            if (!columnHasElements)
            {
                EnemyHolder.ColumnDefinitions.RemoveAt(columnIndex);

                // Adjust column indices for elements after the removed column
                foreach (UIElement child in EnemyHolder.Children)
                {
                    int currentColumn = Grid.GetColumn(child);
                    if (currentColumn > columnIndex)
                    {
                        Grid.SetColumn(child, currentColumn - 1);
                    }
                }
            }
            return !columnHasElements;
        }


        #region Player Input
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    if (Player1.PlayerLeft)
                        return;
                    Player1.TurnLeft(true);
                    break;
                case Key.D:
                    if (Player1.PlayerRight)
                        return;
                    Player1.TurnRight(true);
                    break;
                case Key.Space:
                    if (playerAttack == KeyState.Pressed || playerAttack == KeyState.Down)
                        playerAttack = KeyState.Down;
                    else
                        playerAttack = KeyState.Pressed;
                    return;
                case Key.Escape:
                    PauseGame(sender, e);
                    return;
                case Key.F:
                //// for debugging
                //CompositionTarget.Rendering -= GameLoop;

                default:
                    return;
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
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/Player_bullet_1.png"))
            };

            double x = Canvas.GetLeft(Player1.PlayerHitBoxes) + (Player1.PlayerHitBoxes.ActualWidth / 2 - bullet.ActualWidth / 2);
            double y = Canvas.GetTop(Player1.PlayerHitBoxes) - bullet.Height;
            Canvas.SetLeft(bullet, x);
            Canvas.SetTop(bullet, y);
            MainCanvas.Children.Add(bullet);
            bullets.Add(bullet);
        }

        private void ShootEnemy(Enemy shooter)
        {
            Projectile projectile = (Projectile)shooter.Projectile.Clone();
            Image bullet = projectile.ProjectileState;
            // Get the position of the shooter relative to the canvas
            Point shooterPositionRelativeToCanvas = shooter.EnemyState.TranslatePoint(new Point(0, 0), MainCanvas);

            // Calculate the bullet's position
            double canvasLeft = shooterPositionRelativeToCanvas.X + shooter.EnemyState.Width / 2 - bullet.Width / 2;
            double canvasTop = shooterPositionRelativeToCanvas.Y - bullet.Height + 50;

            // Set the bullet's position relative to the canvas
            Canvas.SetLeft(bullet, canvasLeft);
            Canvas.SetTop(bullet, canvasTop);


            MainCanvas.Children.Add(bullet);
            enemyBullets.Add(projectile);
        }

        private void CreateObstacle(double x, double y)
        {
            // części przeszkody
            Image[] obstacleParts = new Image[11];
            List<int> pozX = new List<int>() { -40, -40, -40, -20, -20, 0, 20, 20, 40, 40, 40 };
            List<int> pozY = new List<int>() { -20,0,20,20,40,40,40,20,20,0,-20 };
            for (int i = 0; i < 11; i++)
            {
                Image obstaclePart = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Obstacles/obstacle_0.png")),
                    Width = 20,
                    Height = 20,
                };
                Canvas.SetLeft(obstaclePart, x - pozX[i]);
                Canvas.SetTop(obstaclePart, y - pozY[i]);
                MainCanvas.Children.Add(obstaclePart);
                obstacleParts[i] = obstaclePart;
            }

            Obstacle obstacle = new Obstacle(obstacleParts);
            obstacles.Add(obstacle);
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