using System.Windows.Forms;

namespace CodeRift.Levels
{
    public partial class Level4Form : BaseBattleForm
    {
        public Level4Form()
        {
            InitializeComponent();
            LevelName = "Level 4: ArrayWorm";
            UpdateUI();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(4);
            this.Close();
        }
    }
}
