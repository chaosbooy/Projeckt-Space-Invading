using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpaceInvading.Resources.Classes
{
    internal class Obstacle
    {
        // części przeszkody - obrazki
        public Image[] Parts { get; set; }
        // od 0 do 5 - przeszkody widoczne, -1 -> przeszkoda usunięta
        public int[] Health { get; set; }
        public int tier { get; set; }
            
        public Obstacle(Image[] ObstacleParts)
        {
            this.Parts = ObstacleParts;
            tier = Inventory.GetPermamentUpgradeCount("Barrier Upgrade");
            Health = new int[ObstacleParts.Length];
            for (int i = 0; i < Parts.Length; i++)
            {
                Health[i] = tier + 2;
            }
        }

        // zwrocono true - zniszczono całkowicie część, false - tylko uszkodzenie, jesli true - usun obrazek z planszy
        public bool DamagePart(int PartNr)
        {

            if (Health[PartNr] > 1 ) Health[PartNr]--;
            else
            {
                Health[PartNr] = -1;
                return true;
            }
             Parts[PartNr].Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Obstacles/obstacle_"+ tier + "_" + Health[PartNr].ToString() + ".png"));
            return false;
        }

    }
}
