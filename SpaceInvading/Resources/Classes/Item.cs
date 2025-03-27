using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Item : ICloneable
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public Item WorthItem { get; set; }
        public int Worth { get; set; }
        public char Shop { get; set; }//w - witch, m - merchant, b - blacksmith, n - none
        public bool isUsable { get; set; }
        public Image Sprite { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
