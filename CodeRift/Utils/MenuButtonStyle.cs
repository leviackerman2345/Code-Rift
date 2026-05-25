using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Utils
{
    public static class MenuButtonStyle
    {
        private static readonly Color MatrixGreen = Color.FromArgb(0, 255, 65);
        private static readonly HashSet<Button> ButtonsWithHoverEvents = new HashSet<Button>();

        public static void Apply(Button button, string text, bool useMenuSize = false, bool playClickSound = false)
        {
            button.Text = text;
            button.BackColor = Color.Black;
            button.ForeColor = MatrixGreen;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = MatrixGreen;
            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            button.FlatAppearance.MouseOverBackColor = Color.Black;
            button.Font = new Font("Courier New", 18F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.TabStop = false;
            button.UseVisualStyleBackColor = false;

            if (useMenuSize)
            {
                button.Size = new Size(400, 60);
            }

            if (ButtonsWithHoverEvents.Add(button))
            {
                button.MouseEnter += (_, _) =>
                {
                    if (!button.Enabled)
                    {
                        return;
                    }

                    AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                    button.ForeColor = Color.Black;
                    button.FlatAppearance.BorderColor = Color.Black;

                    Image? hoverImage = ImageManager.Instance.GetImage(Constants.IMG_UI_BUTTON);
                    if (hoverImage != null)
                    {
                        button.BackgroundImage = hoverImage;
                        button.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                };

                button.MouseLeave += (_, _) =>
                {
                    button.ForeColor = MatrixGreen;
                    button.FlatAppearance.BorderColor = MatrixGreen;
                    button.BackgroundImage = null;
                };
            }

            if (playClickSound)
            {
                button.Click += (_, _) => AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            }
        }
    }
}
