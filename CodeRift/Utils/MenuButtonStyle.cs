using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Utils
{
    public static class MenuButtonStyle
    {
        private static readonly Color MatrixGreen = Color.FromArgb(0, 255, 65);
        private static readonly Color HoverBlack = Color.Black;
        private static readonly HashSet<Button> ButtonsWithHoverEvents = new HashSet<Button>();
        private static readonly Dictionary<string, Image> HoverImagesBySize = new Dictionary<string, Image>();
        private static readonly Dictionary<Button, bool> HoverImageEnabledByButton = new Dictionary<Button, bool>();

        public static void Apply(Button button, string text, bool useMenuSize = false, bool playClickSound = false, bool useHoverImage = true)
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
            HoverImageEnabledByButton[button] = useHoverImage;

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
                    SetHoverVisual(button, hovered: true);
                };

                button.MouseLeave += (_, _) =>
                {
                    SetHoverVisual(button, hovered: false);
                };
            }

            if (playClickSound)
            {
                button.Click += (_, _) => AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            }

            SetHoverVisual(button, hovered: false);
        }

        public static void SyncHoverVisualState(Button button)
        {
            if (button.IsDisposed || !button.Visible)
            {
                return;
            }

            Point pointer = button.PointToClient(Cursor.Position);
            bool hovered = button.ClientRectangle.Contains(pointer);
            SetHoverVisual(button, hovered);
        }

        private static void SetHoverVisual(Button button, bool hovered)
        {
            if (!button.Enabled)
            {
                button.ForeColor = MatrixGreen;
                button.FlatAppearance.BorderColor = MatrixGreen;
                button.BackgroundImage = null;
                return;
            }

            if (!hovered)
            {
                button.ForeColor = MatrixGreen;
                button.FlatAppearance.BorderColor = MatrixGreen;
                button.BackgroundImage = null;
                return;
            }

            button.ForeColor = HoverBlack;
            button.FlatAppearance.BorderColor = HoverBlack;

            bool useHoverImage = HoverImageEnabledByButton.TryGetValue(button, out bool isEnabled) && isEnabled;
            if (!useHoverImage)
            {
                button.BackgroundImage = null;
                return;
            }

            Image? hoverImage = GetSizedHoverImage(button.Size);
            if (hoverImage != null)
            {
                button.BackgroundImage = hoverImage;
                button.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private static Image? GetSizedHoverImage(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
            {
                return null;
            }

            string key = $"{size.Width}x{size.Height}";
            if (HoverImagesBySize.TryGetValue(key, out Image? cached))
            {
                return cached;
            }

            Image? source = ImageManager.Instance.GetImage(Constants.IMG_UI_BUTTON);
            if (source == null)
            {
                return null;
            }

            Bitmap hover = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(hover))
            {
                g.DrawImage(source, new Rectangle(0, 0, size.Width, size.Height));
            }

            HoverImagesBySize[key] = hover;
            return hover;
        }
    }
}
