using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    public class Item : ICloneable
    {
        public int Worth { get; set; }
        public bool isUsable { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
