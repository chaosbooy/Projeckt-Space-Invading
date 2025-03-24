using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Enemy : ICloneable
    {
        public List<Item> PossibleDrops = new List<Item>();
        public int MaxDropCount { get; set; }

        public string Name { get; set; }
        public int Score { get; set; }
        public int Health { get; set; }
        public Image EnemyState { get; set; }
        public Projectile Projectile { get; set; }

        public Enemy()
        {
            EnemyState = new Image();
            Projectile = new Projectile();
            Name = "";
        }

        public object Clone()
        {
            return new Enemy
            {
                Name = this.Name,
                Score = this.Score,
                Health = this.Health,
                MaxDropCount = this.MaxDropCount,
                PossibleDrops = new List<Item>(this.PossibleDrops), // Głębokie kopiowanie listy
                Projectile = (Projectile)this.Projectile.Clone(), // Jeśli Projectile też implementuje ICloneable
                EnemyState = new Image
                {
                    Source = this.EnemyState.Source, // Kopiowanie źródła obrazu
                    Width = this.EnemyState.Width,
                    Height = this.EnemyState.Height,
                    Stretch = this.EnemyState.Stretch
                }
            };
        }
    }
}
