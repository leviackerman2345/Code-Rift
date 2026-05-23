using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeRift.Forms
{
    public enum TerminalMessageType
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Terminal-styled replacement for generic MessageBox UI.
    /// </summary>
    public static class TerminalMessageBox
    {
        public static DialogResult Show(IWin32Window? owner, string message, string title, TerminalMessageType type = TerminalMessageType.Info)
        {
            using TerminalMessageDialog dialog = new TerminalMessageDialog(message, title, type);
            return owner != null ? dialog.ShowDialog(owner) : dialog.ShowDialog();
        }
    }

    internal sealed class TerminalMessageDialog : Form
    {
        private readonly Color _terminalGreen = Color.FromArgb(0, 255, 65);
        private readonly Color _terminalRed = Color.FromArgb(255, 72, 72);
        private readonly Color _terminalYellow = Color.FromArgb(255, 210, 80);
        private readonly Color _darkBackground = Color.FromArgb(8, 13, 8);
        private readonly Color _mutedGreen = Color.FromArgb(26, 74, 26);
        private readonly Color _mutedRed = Color.FromArgb(90, 32, 32);
        private readonly Color _mutedYellow = Color.FromArgb(84, 68, 30);
        private readonly Color _accentColor;
        private readonly Color _mutedAccentColor;

        public TerminalMessageDialog(string message, string title, TerminalMessageType type)
        {
            bool isBattleOutcomeMessage =
                string.Equals(message, "You win", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(message, "You lose", StringComparison.OrdinalIgnoreCase);

            (_accentColor, _mutedAccentColor) = type switch
            {
                TerminalMessageType.Error => (_terminalRed, _mutedRed),
                TerminalMessageType.Warning => (_terminalYellow, _mutedYellow),
                _ => (_terminalGreen, _mutedGreen)
            };

            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = _darkBackground;
            Width = 640;
            Height = 300;

            Panel frame = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                BackColor = _darkBackground
            };
            frame.Paint += Frame_Paint;

            Label lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Font = new Font("Courier New", 13f, FontStyle.Bold),
                ForeColor = _accentColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = $"// {title.ToUpperInvariant()} //"
            };

            string statusPrefix = type switch
            {
                TerminalMessageType.Warning => "[WARN]",
                TerminalMessageType.Error => "[ERR ]",
                _ => "[INFO]"
            };
            string displayMessage = isBattleOutcomeMessage ? message : $"{statusPrefix} {message}";

            Label lblMessage = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Courier New", isBattleOutcomeMessage ? 26f : 12f, FontStyle.Bold),
                ForeColor = _accentColor,
                TextAlign = isBattleOutcomeMessage ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 14, 6, 14),
                Text = displayMessage
            };

            Panel bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 58,
                Padding = new Padding(0, 10, 0, 0)
            };

            Button btnOk = new Button
            {
                Text = "CONFIRM",
                DialogResult = DialogResult.OK,
                Font = new Font("Courier New", 11f, FontStyle.Bold),
                ForeColor = _accentColor,
                BackColor = _darkBackground,
                FlatStyle = FlatStyle.Flat,
                Width = 160,
                Height = 36,
                Anchor = AnchorStyles.None
            };
            btnOk.FlatAppearance.BorderColor = _accentColor;
            btnOk.FlatAppearance.BorderSize = 1;
            btnOk.Click += (s, e) => {
                Managers.AudioManager.Instance.PlaySFX(Utils.Constants.SFX_CLICK);
            };
            btnOk.MouseEnter += (_, _) =>
            {
                btnOk.BackColor = _accentColor;
                btnOk.ForeColor = Color.Black;
            };
            btnOk.MouseLeave += (_, _) =>
            {
                btnOk.BackColor = _darkBackground;
                btnOk.ForeColor = _accentColor;
            };

            bottomBar.Controls.Add(btnOk);
            bottomBar.Resize += (_, _) =>
            {
                btnOk.Left = (bottomBar.Width - btnOk.Width) / 2;
                btnOk.Top = 10;
            };

            frame.Controls.Add(lblMessage);
            frame.Controls.Add(bottomBar);
            frame.Controls.Add(lblTitle);
            Controls.Add(frame);

            AcceptButton = btnOk;
            CancelButton = btnOk;
        }

        private void Frame_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
            {
                return;
            }

            using Pen outer = new Pen(_accentColor, 1);
            using Pen inner = new Pen(_mutedAccentColor, 1);

            e.Graphics.DrawRectangle(outer, 0, 0, panel.Width - 1, panel.Height - 1);
            e.Graphics.DrawRectangle(inner, 3, 3, panel.Width - 7, panel.Height - 7);
        }
    }
}
