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
        public int[] Damages { get; set; }

        public Obstacle(Image[] ObstacleParts)
        {
            this.Parts = ObstacleParts;
            Damages = new int[ObstacleParts.Length];
            for(int i = 0; i < Parts.Length; i++)
            {
                Damages[i] = 0;
            }
        }

        // zwrocono true - zniszczono całkowicie część, false - tylko uszkodzenie, jesli true - usun obrazek z planszy
        public bool DamagePart(int PartNr)
        {
            /*
            int PartNr = 0;
            // szukanie indeksu trafionej części
            for(int i = 0; i < Parts.Length; i++)
            {
                if (Parts[i] == DamagedImage) PartNr = i; break;
            }*/

            if(Damages[PartNr] < 5) Damages[PartNr]++;
            else
            {
                Damages[PartNr] = -1;
                return true;
            }
            Parts[PartNr].Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/obstacle_" + Damages[PartNr].ToString() + ".png"));
            return false;
        }

    }
}
