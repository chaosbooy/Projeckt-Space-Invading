using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Enemy
    {
        public List<Item> PossibleDrops = new List<Item>();
        public int MaxDropCount { get; set; }

        public string Name { get; set; }
        public int Score { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Image EnemyState { get; set; }
        public Projectile Projectile { get; set; }

        public Enemy()
        {
            EnemyState = new Image();
            Projectile = new Projectile();
            Name = "";
        }
    }
}
