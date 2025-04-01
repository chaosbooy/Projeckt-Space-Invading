using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Boss
    {
        public string BossName { get; set; }
        public int ProjectileThrownCount { get; set; }
        public List<Enemy> BossPhases { get; set; }
        public List<Image> PhaseAnimation { get; set; }

        public Boss() 
        {
            PhaseAnimation = new List<Image>();
            BossPhases = new List<Enemy>();
            BossName = "";
        }

        public object Clone()
        {
            List<Enemy> phasesCloned = new List<Enemy>();

            foreach (Enemy phase in this.BossPhases)
            {
                phasesCloned.Add((Enemy)phase.Clone());
            }

            List<Image> animationsCloned = new List<Image>();

            foreach (Image animation in this.PhaseAnimation)
            {
                animationsCloned.Add(new Image
                {
                    Source = animation.Source, // Kopiowanie źródła obrazu
                    Width = animation.Width,
                    Height = animation.Height,
                    Stretch = animation.Stretch
                });
            }

            return new Boss
            {
                BossName = this.BossName,
                ProjectileThrownCount = this.ProjectileThrownCount,
                BossPhases = phasesCloned,
                PhaseAnimation = animationsCloned,
            };
        }
    }
}
