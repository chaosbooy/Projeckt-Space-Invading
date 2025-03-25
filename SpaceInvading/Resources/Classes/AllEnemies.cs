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

            }
        };

        public readonly static Enemy Slime = new Enemy
        {
            Name = "Slime",
            Score = 15,
            Health = 1,
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

            }
        };
    }
}
