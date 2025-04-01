 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpaceInvading.Resources.Classes
{
    internal class AllEnemies
    {
        public readonly static Enemy Spider = new Enemy
        {
            Name = "Spider",
            Score = 10,
            Health = 1,
            Tier = 1,
            ShootChance = 15,
            MaxDropCount = 3,
            Projectile = new Projectile
            {
                Damage = 1,
                Speed = 4,
                ProjectileState = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Spider/Spider_bullet_1.png")),
                    Width = 64,
                    Height = 32
                }
            },
            EnemyState = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Spider/Spider.png")),
                Width = 50,
                Height = 50
            },
            PossibleDrops = new List<Item>
            {
                AllItems.SpiderDrop
            }
        };
        public readonly static Enemy Skeleton = new Enemy
        {
            Name = "Skeleton",
            Score = 10,
            Health = 1,
            MaxDropCount = 3,
            Tier = 2,
            ShootChance = 25,
            Projectile = new Projectile
            {
                Damage = 1,
                Speed = 4,
                ProjectileState = new Image
                {
                    Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Images/Skeleton/Skeleton_Bullet.png")),
                    Width = 64,
                    Height = 32
                }
            },
            EnemyState = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Images/Skeleton/Skeleton.png")),
                Width = 50,
                Height = 50
            },
            PossibleDrops = new List<Item>
            {
                AllItems.SkeletonDrop
            }
        };
        public readonly static Enemy Slime = new Enemy
        {
            Name = "Slime",
            Score = 15,
            Health = 1,
            Tier = 1,
            ShootChance = 15,
            MaxDropCount = 3,
            Projectile = new Projectile
            {
                Damage = 1,
                Speed = 2,
                ProjectileState = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime/Slime_bullet_1.png")),
                    Width = 64,
                    Height = 32
                }
            },
            EnemyState = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime/Slime_Still.png")),
                Width = 50,
                Height = 50
            },
            PossibleDrops = new List<Item>
            {
                AllItems.SlimeDrop
            }
        };

        public readonly static List<Enemy> Mobs = new List<Enemy>
        {
            Spider,
            Skeleton,
            Slime
        };

        public readonly static Boss SlimeBoss = new Boss
        {
            BossName = "Slime Boss",
            ProjectileThrownCount = 5,
            BossPhases = new List<Enemy>
            {
                new Enemy
                {
                    Name = "Slime King",
                    Score = 100,
                    Health = 3,
                    MaxDropCount = 15,
                    ShootChance = 200,
                    Projectile = new Projectile
                    {
                        Damage = 1,
                        Speed = 2,
                        ProjectileState = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_Phase_1_bullet.png")),
                            Width = 64,
                            Height = 32
                        }
                    },
                    EnemyState = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_Sprite.png")),
                        Width = 200,
                        Height = 200
                    },
                    PossibleDrops = new List<Item>
                    {
                        AllItems.SlimeDrop,
                        AllItems.Boss_1_drop_1
                    }
                },
                new Enemy
                {
                    Name = "Slime Kings Core",
                    Score = 150,
                    Health = 2,
                    MaxDropCount = 4,
                    ShootChance = 150,
                    Projectile = new Projectile
                    {
                        Damage = 1,
                        Speed = 2,
                        ProjectileState = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_Phase_2_bullet.png")),
                            Width = 64,
                            Height = 32
                        }
                    },
                    EnemyState = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_Sprite_phase_2.png")),
                        Width = 200,
                        Height = 200
                    },
                    PossibleDrops = new List<Item>
                    {
                        AllItems.Boss_1_drop_3,
                        AllItems.Boss_1_drop_2
                    }
                }
            }

        };
    }
}
