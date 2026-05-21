using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    /// <summary>
    /// BattleArenaForm - A beginner-friendly frontend UI for our fighting game!
    /// 
    /// This form just handles displaying the UI (User Interface) using panels, 
    /// labels, and picture boxes. There is no damage calculation or game logic yet.
    /// It's styled like a retro hacker terminal with a fighting game layout.
    /// </summary>
    public partial class BattleArenaForm : Form
    {
        // Current level context passed from LevelsMenuForm.
        public int Level { get; private set; }

        public BattleArenaForm(int level = 1)
        {
            InitializeComponent();
            this.Level = level;
            SetupLevel();
        }

        private void SetupLevel()
        {
            // UI-only label update; combat mechanics are not implemented here yet.
            lblLevelTitle.Text = $"// LEVEL {Level} : {GetLevelName(Level)} //";
            lblEnemyName.Text = GetLevelName(Level);
        }

        private string GetLevelName(int level)
        {
            return level switch
            {
                1 => "LOOPBUG",
                2 => "VOID_CRAWLER",
                3 => "STRING_CORRUPTOR",
                4 => "ARRAY_CRASHER",
                5 => "NULL_KING",
                _ => "UNKNOWN_BUG"
            };
        }

        private void BattleArenaForm_Load(object sender, EventArgs e)
        {
            // Asset loading currently reads level files directly from output Assets folder.
            string basePath = Application.StartupPath;

            try
            {
                // 1. Load Background based on level
                string bgPath = Path.Combine(basePath, "Assets", "Images", "backgrounds", "level_background", $"level_{Level}.jpeg");
                if (File.Exists(bgPath))
                {
                    this.BackgroundImage = Image.FromFile(bgPath);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }

                // 2. Load Portraits
                string playerPath = Path.Combine(basePath, "Assets", "Images", "portraits", "player.jpeg");
                string enemyPath = Path.Combine(basePath, "Assets", "Images", "portraits", $"enemy_level_{Level}.jpeg");

                if (File.Exists(playerPath))
                {
                    picPlayerPortrait.Image = Image.FromFile(playerPath);
                    picPlayerThumb.Image = Image.FromFile(playerPath);
                }

                if (File.Exists(enemyPath))
                {
                    picEnemyPortrait.Image = Image.FromFile(enemyPath);
                    picEnemyThumb.Image = Image.FromFile(enemyPath);
                }

                // 4. Load Player Cards (Bottom Left)
                LoadCards("player", "player_card", picPlayerCard1, picPlayerCard2, picPlayerCard3, picPlayerCard4, picPlayerCard5);

                // 5. Load Enemy Cards (Bottom Right)
                LoadCards("enemies", "enemy_card", picEnemyCard1, picEnemyCard2, picEnemyCard3, picEnemyCard4, picEnemyCard5);

                // Prototype progression: unlocks next level when this screen is loaded.
                ProgressManager.Instance.UnlockNextLevel(Level);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load some images: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadCards(string folder, string prefix, params PictureBox[] boxes)
        {
            string basePath = Application.StartupPath;
            for (int i = 0; i < boxes.Length; i++)
            {
                string path = Path.Combine(basePath, "Assets", "Images", folder, "cards", $"{prefix}_{i + 1}.jpeg");
                if (File.Exists(path))
                {
                    boxes[i].Image = Image.FromFile(path);
                }
            }
        }

        /// <summary>
        /// This method runs when the user clicks the "< BACK" button.
        /// It simply closes this battle arena form.
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblEnemyHP_Click(object sender, EventArgs e)
        {

        }
    }
}
