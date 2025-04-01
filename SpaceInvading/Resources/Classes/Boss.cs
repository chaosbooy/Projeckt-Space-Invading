using System.Windows.Controls;

namespace SpaceInvading.Resources.Classes
{
    public class Boss
    {
        public string BossName { get; set; }
        public int ProjectileThrownCount { get; set; }
        public List<Enemy> BossPhases { get; set; }
        public List<string> PhaseAnimation { get; set; }

        public Boss() 
        {
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

            return new Boss
            {
                ProjectileThrownCount = this.ProjectileThrownCount,
                BossPhases = phasesCloned
            };
        }
    }
}
