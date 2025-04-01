using SpaceInvading.Resources.Classes;
using SpaceInvading.Resources.Pages;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace SpaceInvading.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        #region Variables

        private readonly bool HitBoxShow = false;
        private bool gamePaused, roundEnd = false;
        private int minWidth = 0, minHeight = 0;
        public static int _score = 0;
        Random rnd = new Random();

        public Player Player1;

        private Timer _effectTimer;
        private Effects currEffect = Effects.None;

        // przeciwnicy na planszy
        private List<Enemy> Enemies = new();
        private Boss CurrentBoss = new();

        private List<Image> bullets = new();
        private List<Projectile> enemyBullets = new();

        // prędkość co tick
        private double bulletSpeed = 5;
        private double enemySpeed = 0.7;
        // co ktory tick mają ruszyć się wrogowie
        private double TickNumber = 0;

        private static int round;
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

            SetupNewRound();
            round--;

            CreateObstacle(150, 400);
            CreateObstacle(350, 400);
            CreateObstacle(550, 400);
            UpdateAbilityWindow();
        }

        private void XamlLoaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= GameLoop;
            CompositionTarget.Rendering += GameLoop;

            round++;
            var window = Window.GetWindow(this);
            
            window.KeyDown -= Window_KeyDown;
            window.KeyUp -= Window_KeyUp;

            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;

            MainCanvas.Focus();
        }

        #region Setups

        private void SetupNewRound()
        {
            minWidth = 0;
            minHeight = 0;

            EnemyHolder.ColumnDefinitions.Clear();
            EnemyHolder.RowDefinitions.Clear();

            round++;

            //Debug.WriteLine(round);
            if (round % 5 == 0)
            {
                CurrentBoss = (Boss)AllEnemies.SlimeBoss.Clone();
                SetupBoss();
            }
            else if (round == 1)
            {
                CurrentBoss = (Boss)AllEnemies.SlimeBoss.Clone();
                SetupBoss();
            }
            else if (round == 2)
                SetupGame(1, 10);
            else if (round < 5)
                SetupGame(2, 10);
            else
                SetupGame(3, 10);
            return;
        }
        
        private void SetupBoss()
        {
            var phaseOne = CurrentBoss.BossPhases[0];

            Border border = new Border
            {
                Background = HitBoxShow ? Brushes.Black : Brushes.Transparent,
                Width = phaseOne.EnemyState.Width,
                Height = phaseOne.EnemyState.Height
            };
            minWidth = (int)phaseOne.EnemyState.Width;
            minHeight = (int)phaseOne.EnemyState.Height;

            if(CurrentBoss.BossPhases.Count == 2)
            {
                Canvas.SetTop(EnemyHolder, -40);
                Canvas.SetLeft(EnemyHolder, (MainCanvas.Width - minWidth) / 2);
                border.Child = phaseOne.EnemyState; // Dodaj obraz jako dziecko ramki
            }
            else
            {
                while(bullets.Count > 0)
                {
                    MainCanvas.Children.Remove(bullets[0]);
                    bullets.RemoveAt(0);
                }
                while(enemyBullets.Count > 0)
                {
                    MainCanvas.Children.Remove(enemyBullets[0].ProjectileState);
                    enemyBullets.RemoveAt(0);
                }

                CompositionTarget.Rendering -= GameLoop;

                border.Child = CurrentBoss.PhaseAnimation[0];

                // Apply the animated GIF behavior
                ImageBehavior.SetAnimatedSource(CurrentBoss.PhaseAnimation[0], CurrentBoss.PhaseAnimation[0].Source);
                ImageBehavior.SetRepeatBehavior(CurrentBoss.PhaseAnimation[0], new System.Windows.Media.Animation.RepeatBehavior(1));
                ImageBehavior.AddAnimationCompletedHandler(CurrentBoss.PhaseAnimation[0], (sender, e) =>
                {
                    CompositionTarget.Rendering += GameLoop;
                    border.Child = phaseOne.EnemyState;
                    CurrentBoss.PhaseAnimation.RemoveAt(0);
                });

                Canvas.SetLeft(EnemyHolder, Canvas.GetLeft(EnemyHolder) - CurrentBoss.BossPhases[0].EnemyState.Width);
            }

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);

            EnemyHolder.Children.Add(border);
            Enemies.Add(phaseOne);
            Debug.WriteLine($"Left: {Canvas.GetLeft(EnemyHolder)} Top: {Canvas.GetTop(EnemyHolder)}");
            
            //Health bar
            progressBar.Maximum = phaseOne.Health;
            progressBar.Visibility = Visibility.Visible;
            UpdateBossHealth(phaseOne.Health);

            minWidth += 10;
            minHeight += 10;

            EnemyHolder.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = minWidth });
            EnemyHolder.RowDefinitions.Add(new RowDefinition { MinHeight = minHeight });
        }
        
        private void SetupNextBossPhase()
        {
            CurrentBoss.BossPhases.RemoveAt(0);
            if (CurrentBoss.BossPhases.Count == 0)
            {
                progressBar.Visibility = Visibility.Collapsed;
                roundEnd = true;
                var window = Window.GetWindow(this);
                CompositionTarget.Rendering -= GameLoop;

                new Timer((sender) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        window.KeyDown -= Window_KeyDown;
                        window.KeyUp -= Window_KeyUp;
                        this.NavigationService.Navigate(new Village());
                    });

                }, null, 1000, 0);

                return;
            }

            SetupBoss();
        }

        private void SetupGame(int enemyRows, int enemyCols)
        {
            Canvas.SetLeft(EnemyHolder, 20);
            Canvas.SetTop(EnemyHolder, 20);

            Random rnd = new Random();

            for (int i = 0; i < enemyRows; i++)
            {
                for (int j = 0; j < enemyCols; j++)
                {
                    var block = (Enemy) AllEnemies.Mobs[rnd.Next(AllEnemies.Mobs.Count)].Clone();

                    Border border = new Border
                    {
                        Background = HitBoxShow ? Brushes.Black : Brushes.Transparent,
                        Width = block.EnemyState.Width,
                        Height = block.EnemyState.Height
                    };
                    border.Child = block.EnemyState; // Dodaj obraz jako dziecko ramki

                    if (minWidth < block.EnemyState.Width)
                        minWidth = (int)block.EnemyState.Width;
                    if (minHeight< block.EnemyState.Height)
                        minHeight = (int)block.EnemyState.Height;

                    Grid.SetColumn(border, j);
                    Grid.SetRow(border, i);
                    EnemyHolder.Children.Add(border);
                    Enemies.Add(block);
                }
            }

            minWidth += 10;
            minHeight += 10;

            for (int i = 0; i < enemyCols; ++i)
                EnemyHolder.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = minWidth});

            for (int i = 0; i < enemyRows; ++i)
                EnemyHolder.RowDefinitions.Add(new RowDefinition { MinHeight = minHeight});
        }

        #endregion

        private void UpdateBossHealth(int hp)
        {
            progressBar.Value = hp;
        }
        private void EndGame()
        {
            var window = Window.GetWindow(this);
            window.KeyDown -= Window_KeyDown;
            window.KeyUp -= Window_KeyUp;
            CompositionTarget.Rendering -= GameLoop;
            Player1.Stay();
            GameOverPanel.Visibility = Visibility.Visible;
            round = 0;
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            
            TickNumber++;
            if (Player1.PlayerLeft && Canvas.GetLeft(Player1.PlayerHitBoxes) > 0)
            {
                Canvas.SetLeft(Player1.PlayerHitBoxes, Canvas.GetLeft(Player1.PlayerHitBoxes) - Player1.PlayerSpeed);
            }
            if (Player1.PlayerRight && Canvas.GetLeft(Player1.PlayerHitBoxes) < MainCanvas.ActualWidth - Player1.PlayerHitBoxes.ActualWidth * 0.7)
            {
                Canvas.SetLeft(Player1.PlayerHitBoxes, Canvas.GetLeft(Player1.PlayerHitBoxes) + Player1.PlayerSpeed);
            }

            if (playerAttack == KeyState.Pressed && !Player1.IsAttacking)
            {
                playerAttack = KeyState.Down;
                Shoot();
                Player1.Attack();
            } else if ((playerAttack == KeyState.Pressed || playerAttack == KeyState.Down) && currEffect == Effects.Rage)
            {
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

            //pociski gracza
            foreach (var bullet in bullets.ToArray())
            {
                // trafienie pocisku gracza w przeszkodę
                bool isBulletAlive = true;      // okresla czy przycisk jest na ekranie
                for (int i = 0; i < obstacles.Count; i++)
                {
                    // sprawdzanie czesci w kazdej przeszkodzie
                    for (int j = 0; j < obstacles[i].Parts.Count(); j++)
                    {
                        if (obstacles[i].Health[j] != -1)
                        {
                            if (isBulletAlive && IsColliding(obstacles[i].Parts[j], 1, bullet))
                            {
                                bool ifDestroyed = obstacles[i].DamagePart(j);
                                if (ifDestroyed)
                                {
                                    MainCanvas.Children.Remove(obstacles[i].Parts[j]);
                                }

                                if(currEffect != Effects.Enchant)
                                {
                                    MainCanvas.Children.Remove(bullet);
                                    bullets.Remove(bullet);
                                    isBulletAlive = false;
                                }
                            }
                        }
                    }
                }
                if (!isBulletAlive) continue;

                // trafienie pocisku gracza w przeciwnika
                foreach (var block in Enemies.ToArray())
                {
                    if (isBulletAlive && IsColliding(bullet, 0.9, block.EnemyState))
                    {
                        if (block.Health-- == 1)
                        {
                            EnemyHolder.Children.Remove((UIElement)block.EnemyState.Parent);

                            Enemies.Remove(block);
                            _score += block.Score;


                            if (CurrentBoss.BossPhases.Contains(block))
                                Inventory.AddItem(block.PossibleDrops[0], block.MaxDropCount);

                            Inventory.AddItem(block.PossibleDrops.Last(), rnd.Next(block.MaxDropCount));
                        }

                        if (CurrentBoss.BossPhases.Contains(block))
                        {
                            _score += block.Score / 10;
                            UpdateBossHealth(block.Health);
                        }
                            
                        ScoreCount.Content = _score.ToString();

                        while (RemoveEmptyColumn(0))
                            Canvas.SetLeft(EnemyHolder, Canvas.GetLeft(EnemyHolder) + minWidth);
                        while (RemoveEmptyColumn(EnemyHolder.ColumnDefinitions.Count - 1)) ;

                        if (currEffect != Effects.Enchant)
                        {
                            MainCanvas.Children.Remove(bullet);
                            bullets.Remove(bullet);
                            isBulletAlive = false;
                            break;
                        }
                    }
                }
            }

            if (Enemies.Count == 0 && CurrentBoss.BossPhases.Count > 0)
                SetupNextBossPhase();
            else if (Enemies.Count == 0)
                SetupNewRound();

            // ruch pocisków wrogów
            foreach (var projectile in enemyBullets.ToArray())
            {
                Image bullet = projectile.ProjectileState;

                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + projectile.Speed);
                // znikanie pocisku za swiatem
                if (Canvas.GetTop(bullet) > this.ActualHeight)
                {
                    MainCanvas.Children.Remove(bullet);
                    enemyBullets.Remove(projectile);
                }
                //trafienie gracza
                else if(IsColliding(Player1.PlayerHitBoxes, 0.4, bullet))
                {
                    Player1.Health -= projectile.Damage;
                    UpdateHealthBar();
                    if(currEffect == Effects.Shield)
                    {
                        currEffect = Effects.None;
                        Player1.PlayerHitBoxes.BorderBrush = Brushes.Transparent;
                        Player1.PlayerHitBoxes.BorderThickness = new Thickness(0);

                        MainCanvas.Children.Remove(bullet);
                        enemyBullets.Remove(projectile);
                        Player1.Health += projectile.Damage;
                        UpdateHealthBar();
                    }
                    //jezeli nastepne hp bedzie 0 koncz gre
                    else if (Player1.Health <= 0)
                    {
                        EndGame();
                        return;
                    }
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
                            if (obstacles[i].Health[j] != -1)
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

            if (EnemyHolder.ColumnDefinitions.Count == 0) return;

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

            // strzał bossa ( wg. predkosci bossa )

            if (CurrentBoss.BossPhases.Count != 0 && TickNumber % CurrentBoss.BossPhases[0].ShootChance == 0)
            {
                ShootBoss(CurrentBoss.ProjectileThrownCount);
            }

            // strzał wrogów ( powiedzmy co 30 tick )
            else if (TickNumber % 30 == 0 && CurrentBoss.BossPhases.Count == 0)
            {
                foreach (var enemy in Enemies.ToArray())
                {
                    int next = rnd.Next(300);
                    if (next <= enemy.ShootChance)
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

        private void Restart(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Village());
        }

        private void ToLobby(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Lobby());
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
                case Key.D1:
                    if (Inventory.UsableUpgrades.Count < 1) return;
                    Ability(Inventory.UsableUpgrades[0]);
                    Inventory.UsableUpgrades.RemoveAt(0);
                    UpdateAbilityWindow();
                    return;
                case Key.D2:
                    if (Inventory.UsableUpgrades.Count < 2) return;
                    Ability(Inventory.UsableUpgrades[1]);
                    Inventory.UsableUpgrades.RemoveAt(1);
                    UpdateAbilityWindow();
                    return;
                case Key.D3:
                    if (Inventory.UsableUpgrades.Count < 3) return;
                    Ability(Inventory.UsableUpgrades[2]);
                    Inventory.UsableUpgrades.RemoveAt(2);
                    UpdateAbilityWindow();
                    return;
                case Key.Escape:
                    if (roundEnd) return;
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

        #region Shooting and Abilities
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
        private void UpdateAbilityWindow()
        {
            if (Inventory.UsableUpgrades.Count <= 0) 
            {
                Ability1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
                Ability2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
                Ability3.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
            }
            if (Inventory.UsableUpgrades.Count >= 1)
            {
                Ability1.Source = Inventory.UsableUpgrades[0].Sprite.Source;
            }
            else
            {
                Ability1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
            }
            if (Inventory.UsableUpgrades.Count >= 2)
            {
                Ability2.Source = Inventory.UsableUpgrades[1].Sprite.Source;
            }
            else
            {
                Ability2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
            }
            if (Inventory.UsableUpgrades.Count >= 3)
            {
                Ability3.Source = Inventory.UsableUpgrades[2].Sprite.Source;
            }
            else
            {
                Ability3.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/item_frame_empty.png"));
            }
        }
        private void Ability(Item item)
        {

            int lastLength = Inventory.UsableUpgrades.Count;
            
            if (item == AllItems.HealthPotion)
            {
                if (Player1.Health + 1 >= Player1.MaxHealth) Player1.Health = Player1.MaxHealth;
                else Player1.Health += 1;
                UpdateHealthBar();
            }
            else if (item == AllItems.ShieldPotion)
            {
                currEffect = Effects.Shield;
                Player1.PlayerHitBoxes.BorderBrush = Brushes.Blue;
                Player1.PlayerHitBoxes.BorderThickness = new Thickness(2);

                EffectTimer(lastLength, 5000);
            }
            else if (item == AllItems.RagePotion)
            {
                currEffect = Effects.Rage;
                Player1.PlayerHitBoxes.BorderBrush = Brushes.Red;
                Player1.PlayerHitBoxes.BorderThickness = new Thickness(2);
                Player1.PlayerSpeed = 4;

                EffectTimer(lastLength, 2000);
            }
            else if (item == AllItems.EnchantedSword)
            {
                currEffect = Effects.Enchant;
                Player1.PlayerHitBoxes.BorderBrush = Brushes.Purple;
                Player1.PlayerHitBoxes.BorderThickness = new Thickness(2);

                EffectTimer(lastLength, 3000);
            }
        }

        private void EffectTimer(int lastLength, int effectTime)
        {
            _effectTimer?.Dispose();

            _effectTimer = new Timer((sender) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {       
                    currEffect = Effects.None;

                    Player1.PlayerHitBoxes.BorderBrush = Brushes.Transparent;
                    Player1.PlayerHitBoxes.BorderThickness = new Thickness(0);
                    Player1.PlayerSpeed = 2.5;
                });

            }, null, effectTime, 0);
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

        private void ShootBoss(int bulletCount)
        {
            var currentPhase = CurrentBoss.BossPhases[0];

            for (int i = 0; i < bulletCount; i++)
            {
                Projectile projectile = (Projectile)currentPhase.Projectile.Clone();
                Image bullet = projectile.ProjectileState;
                // Get the position of the shooter relative to the canvas
                Point shooterPositionRelativeToCanvas = currentPhase.EnemyState.TranslatePoint(new Point(0, 0), MainCanvas);
                // Calculate the bullet's position
                double canvasLeft = shooterPositionRelativeToCanvas.X + (currentPhase.EnemyState.Width / bulletCount) * (i + 1) - bullet.Width / 2;
                double canvasTop = shooterPositionRelativeToCanvas.Y + currentPhase.EnemyState.Height;
                // Set the bullet's position relative to the canvas
                Canvas.SetLeft(bullet, canvasLeft);
                Canvas.SetTop(bullet, canvasTop);
                MainCanvas.Children.Add(bullet);
                enemyBullets.Add(projectile);
            }
        }

        #endregion

        private void CreateObstacle(double x, double y)
        {
            if (Inventory.GetPermamentUpgradeCount("Barrier Upgrade") == 0) return;
            // części przeszkody
            Image[] obstacleParts = new Image[4];
            List<int> pozX = new List<int>() { -31, 0, 31, 62 };
            List<int> pozY = new List<int>() { -0,0,0,0};
            for (int i = 3; i >= 0; i--)
            {
                Image obstaclePart = new Image
                {
                    Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Images/Obstacles/obstacle_{Inventory.GetPermamentUpgradeCount("Barrier Upgrade")}_{Inventory.GetPermamentUpgradeCount("Barrier Upgrade") + 2}.png")),
                    Width = 40,
                    Height = 40,
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
    enum Effects
    {
        None,
        Enchant,
        Rage,
        Shield,
    }
}