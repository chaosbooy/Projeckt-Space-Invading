using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    public class Data
    {
        public string PlayerName { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public List<string> Drops { get; set; }
        public List<string> Upgrades { get; set; }

        public int Money { get; set; }
        public int Score { get; set; }
        public Dictionary<string, int> PlayerScores { get; set; }
    }
}
