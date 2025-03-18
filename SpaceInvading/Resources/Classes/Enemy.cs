using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    internal class Enemy
    {
        public List<Item> PossibleDrops = new List<Item>();
        public int MoneyDropUpper { get; set; }
        public int MoneyDropLower { get; set; }

        public string Name { get; set; }
        public int Score { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

    }
}
