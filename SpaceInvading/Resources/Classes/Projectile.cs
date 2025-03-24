using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Projectile: ICloneable
    {
        public Image ProjectileState { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }

        public Projectile()
        {
            ProjectileState = new Image();
        }

        public object Clone()
        {
            return new Projectile
            {
                Damage = this.Damage,
                Speed = this.Speed,
                ProjectileState = new Image
                {
                    Source = this.ProjectileState.Source,
                    Width = this.ProjectileState.Width,
                    Height = this.ProjectileState.Height,
                    Stretch = this.ProjectileState.Stretch
                }
            };
        }
    }
    
}
