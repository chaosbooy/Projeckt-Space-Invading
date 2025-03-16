using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    public class Player
    {
        public List<Item> Drops { get; set; }
        public List<Item> Upgrades { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }

        public int MaxHealth { get; set; }

        private string _playerState;
        public string PlayerState { get { return _playerState; } }

        public int AnimationFrame { get; set; }

        public Player()
        {
            Drops = new List<Item>();
            Upgrades = new List<Item>();
            Name = "Player";
            Health = 2;
            MaxHealth = 2;
            _playerState = "";
        }

        private void Attack()
        {

        }
    }
}
