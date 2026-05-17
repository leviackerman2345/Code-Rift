using System.Windows.Forms;

namespace CodeRift.Levels
{
    public partial class Level2Form : BaseBattleForm
    {
        public Level2Form()
        {
            InitializeComponent();
            LevelName = "Level 2: VoidCrawler";
            UpdateUI();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(2);
            this.Close();
        }
    }
}
