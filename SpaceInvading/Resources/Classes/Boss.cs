using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Boss : Enemy
    {
        public int ProjectileThrownCount { get; set; }
        public int AttackSpeed { get; set; }

        new public object Clone()
        {
            return new Boss
            {
                ProjectileThrownCount = this.ProjectileThrownCount,
                AttackSpeed = this.AttackSpeed,
                Name = this.Name,
                Score = this.Score,
                Health = this.Health,
                MaxDropCount = this.MaxDropCount,
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
