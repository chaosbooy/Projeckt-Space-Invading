using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Projectile
    {
        public Image ProjectileState { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }

        public Projectile() 
        {
            ProjectileState = new Image();
        }
    }
}
