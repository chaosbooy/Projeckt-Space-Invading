using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Boss : Enemy
    {
        public int ProjectileThrownCount;

        new public object Clone()
        {
            return new Boss
            {
                Name = this.Name,
                Score = this.Score,
                Health = this.Health,
                MaxDropCount = this.MaxDropCount,
                ProjectileThrownCount = this.ProjectileThrownCount,
                PossibleDrops = new List<Item>(this.PossibleDrops), // Głębokie kopiowanie listy
                Projectile = (Projectile)this.Projectile.Clone(), // Jeśli Projectile też implementuje ICloneable
                EnemyState = new Image
                {
                    Source = this.EnemyState.Source, // Kopiowanie źródła obrazu
                    Width = this.EnemyState.Width,
                    Height = this.EnemyState.Height,
                    Stretch = this.EnemyState.Stretch
                }
            };
        }
    }
}
