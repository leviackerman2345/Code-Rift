using System.Windows.Forms;

namespace CodeRift.Levels
{
    public partial class Level4Form : BaseBattleForm
    {
        public Level4Form()
        {
            InitializeComponent();
            CurrentLevel = 4;
            LevelName = "Level 4: ArrayWorm";
            SetupBaseForm();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(4);
            this.Close();
        }
    }
}
