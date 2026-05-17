using System.Windows.Forms;
using CodeRift.Forms;

namespace CodeRift.Levels
{
    public partial class Level5Form : BaseBattleForm
    {
        public Level5Form()
        {
            InitializeComponent();
            LevelName = "Level 5: The Null King";
            UpdateUI();
        }

        protected override void TransitionToNext()
        {
            CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(5);
            this.Hide();
            var epilogue = new EpilogueForm();
            epilogue.FormClosed += (s, args) => this.Close();
            epilogue.Show();
        }
    }
}
