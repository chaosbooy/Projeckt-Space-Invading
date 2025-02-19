using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    public class Player : Entity
    {
        List<Item> Drops;
        List<Item> Upgrades;

        public Player()
        {
            Drops = new List<Item>();
            Upgrades = new List<Item>();
            Name = "Player";
            Health = 1;

            IdleSprite = new string[]
            {
                "player_still.png"
            };

            MovingSprite = new string[0];
            DestroyedSprite = new string[0];
        }

        private void Attack()
        {

        }
    }
}
