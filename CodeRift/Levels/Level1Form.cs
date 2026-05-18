using System.Windows.Forms;

namespace CodeRift.Levels
{
    public partial class Level1Form : BaseBattleForm
    {
        public Level1Form()
        {
            InitializeComponent();
            CurrentLevel = 1;
            LevelName = "Level 1: LoopBug";
            SetupBaseForm();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(1);
            this.Close();
        }
    }
}
