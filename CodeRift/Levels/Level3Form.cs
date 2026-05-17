using System.Windows.Forms;

namespace CodeRift.Levels
{
    public partial class Level3Form : BaseBattleForm
    {
        public Level3Form()
        {
            InitializeComponent();
            LevelName = "Level 3: StringCorruptor";
            UpdateUI();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(3);
            this.Close();
        }
    }
}
