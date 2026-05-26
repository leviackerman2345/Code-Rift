using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Entities;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    /// <summary>
    /// BattleArenaQuestionForm: Terminal-inspired question UI.
    /// </summary>
    public partial class BattleArenaQuestionForm : Form
    {
        private static readonly Color DefaultAccent = Color.FromArgb(0, 255, 65);
        private static readonly Color DefaultMuted = Color.FromArgb(26, 74, 26);
        private static readonly Color FailureAccent = Color.FromArgb(255, 65, 65);
        private static readonly Color FailureMuted = Color.FromArgb(80, 24, 24);
        private static readonly Color DarkBackground = Color.FromArgb(8, 13, 8);
        private static readonly Color ButtonBackground = Color.FromArgb(5, 10, 5);
        private const string CodeInputPlaceholder = "type here..";

        private Color _accentColor = DefaultAccent;
        private Color _mutedColor = DefaultMuted;
        private string? _selectedOption;
        private string _typedCommandBuffer = string.Empty;
        private bool _isSubmitting;
        private bool _isShowingCodeInputPlaceholder;
        private bool _updatingCodeInputPlaceholder;

        public Question? CurrentQuestion { get; private set; }
        public bool WasAnswerCorrect { get; private set; }
        public QuestionSkipCommandType SkipCommand { get; private set; }

        public BattleArenaQuestionForm()
        {
            InitializeComponent();
            SetupInitialUI();
            WireMultipleChoiceEvents();
        }

        private void SetupInitialUI()
        {
            ConfigureFormLayout();
            ConfigureInputEvents();
            ConfigureDefaultLabels();
            ApplyAccentPalette(DefaultAccent, DefaultMuted);
            SetQuestionMode(true);
            ShowCodeInputPlaceholder();
        }

        private void ConfigureFormLayout()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = DarkBackground;
            KeyPreview = true;
        }

        private void ConfigureInputEvents()
        {
            KeyDown += BattleArenaQuestionForm_KeyDown;
            KeyPress += BattleArenaQuestionForm_KeyPress;
            txtCodeInput.KeyPress += TxtCodeInput_KeyPress;
            txtCodeInput.TextChanged += TxtCodeInput_TextChanged;
        }

        private void ConfigureDefaultLabels()
        {
            lblSystemId.Text = $"SYS_ID: CR-{Random.Shared.Next(100, 999)}-X | MODE: CMD_AUTH";
            btnSubmit.Text = "CONFIRM_RUN [ENTER]";
            btnBack.Text = "[ESC] BACK";
            txtCodeInput.AcceptsTab = true;
        }

        /// <summary>
        /// Populates question content and restores standard terminal palette.
        /// </summary>
        public void Populate(Question data, int current, int total)
        {
            CurrentQuestion = data;
            WasAnswerCorrect = false;
            SkipCommand = QuestionSkipCommandType.None;
            _selectedOption = null;
            _typedCommandBuffer = string.Empty;
            _isSubmitting = false;
            EnableInputs(true);
            ApplyAccentPalette(DefaultAccent, DefaultMuted);
            ClearOptionHighlights();

            lblQuestion.Text = data.ProblemStatement;
            lblQuestionCounter.Text = $"/// LEVEL_{data.Level:D2}: {data.LevelTitle} | TASK: {current:D2}_OF_{total:D2} ///";
            lblHint.Text = $"> HINT: {data.Hint}";

            bool isCodeMode = data.Type == QuestionType.CodeInput;
            SetQuestionMode(isCodeMode);

            if (!isCodeMode && data.Options != null && data.Options.Count >= 4)
            {
                SetOptions(data.Options[0], data.Options[1], data.Options[2], data.Options[3]);
            }

            if (isCodeMode)
            {
                ShowCodeInputPlaceholder();
            }
            else
            {
                HideCodeInputPlaceholder(clearText: true);
            }
        }

        public void SetQuestionMode(bool isCodeMode)
        {
            pnlCodeInput.Visible = isCodeMode;
            pnlMultiChoice.Visible = !isCodeMode;

            if (isCodeMode)
            {
                lblLineNumbers.Text = "01\n02\n03\n04\n05\n06\n07\n08\n09\n10";
                lblHint.Text = "> STATUS: READY | Enter code, then press ENTER.";
            }
            else
            {
                lblLineNumbers.Text = "A)\nB)\nC)\nD)\n--\nRUN";
                lblHint.Text = "> STATUS: READY | Pick A/B/C/D, then press ENTER.";
            }
        }

        public void UpdateTimer(string timeString)
        {
            lblTimer.Text = timeString;
        }

        private void SetOptions(string a, string b, string c, string d)
        {
            btnOptionA.Text = $"[ A ]  {a}";
            btnOptionB.Text = $"[ B ]  {b}";
            btnOptionC.Text = $"[ C ]  {c}";
            btnOptionD.Text = $"[ D ]  {d}";

            btnOptionA.Tag = a;
            btnOptionB.Tag = b;
            btnOptionC.Tag = c;
            btnOptionD.Tag = d;
        }

        #region Custom Border Painting (Unified Frame)

        private void DoubleLine_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel pnl)
            {
                using Pen pen = new Pen(_accentColor, 1);

                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                e.Graphics.DrawRectangle(pen, 2, 2, pnl.Width - 5, pnl.Height - 5);

                int dividerY = 170;
                e.Graphics.DrawLine(pen, 0, dividerY, pnl.Width, dividerY);
                e.Graphics.DrawLine(pen, 0, dividerY + 2, pnl.Width, dividerY + 2);
            }
        }

        private void TopBar_Paint(object sender, PaintEventArgs e)
        {
            using Pen p = new Pen(_mutedColor, 2);
            e.Graphics.DrawLine(p, 0, pnlTopBar.Height - 1, pnlTopBar.Width, pnlTopBar.Height - 1);
        }

        #endregion

        #region Action Logic & Shortcuts

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            if (!CanSubmitQuestion())
            {
                return;
            }

            _isSubmitting = true;
            if (TryHandleSkipCommand(GetSubmittedCommandText()))
            {
                return;
            }

            bool isCorrect = EvaluateSubmittedAnswer();
            WasAnswerCorrect = isCorrect;
            if (isCorrect)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            await ShowIncorrectAnswerFeedbackAsync();
        }

        private bool CanSubmitQuestion()
        {
            return !_isSubmitting && CurrentQuestion != null;
        }

        private bool TryHandleSkipCommand(string commandText)
        {
            QuestionSkipCommandType skipCommand = QuestionSkipCommand.Parse(commandText);
            if (skipCommand == QuestionSkipCommandType.None)
            {
                return false;
            }

            SkipCommand = skipCommand;
            WasAnswerCorrect = false;
            DialogResult = DialogResult.OK;
            Close();
            return true;
        }

        private bool EvaluateSubmittedAnswer()
        {
            if (CurrentQuestion == null)
            {
                return false;
            }

            string playerAnswer = GetCurrentPlayerAnswer();
            return IsAnswerMatch(playerAnswer, CurrentQuestion.CorrectAnswer);
        }

        private string GetCurrentPlayerAnswer()
        {
            if (CurrentQuestion != null && CurrentQuestion.Type == QuestionType.CodeInput)
            {
                return _isShowingCodeInputPlaceholder ? string.Empty : txtCodeInput.Text.Trim();
            }

            return _selectedOption ?? string.Empty;
        }

        private string GetSubmittedCommandText()
        {
            if (CurrentQuestion != null && CurrentQuestion.Type == QuestionType.CodeInput)
            {
                return _isShowingCodeInputPlaceholder ? string.Empty : txtCodeInput.Text;
            }

            return _typedCommandBuffer;
        }

        private async Task ShowIncorrectAnswerFeedbackAsync()
        {
            EnableInputs(false);
            lblHint.Text = "> STATUS: INVALID ANSWER DETECTED. RETRY REQUIRED.";

            // Keep the original green state visible first.
            await Task.Delay(1000);
            if (IsDisposed) return;

            // Then shift to red failure palette.
            ApplyAccentPalette(FailureAccent, FailureMuted);
            lblHint.Text = "> ACCESS DENIED. LOCKED CARD MUST BE RETRIED.";

            await Task.Delay(700);
            if (IsDisposed) return;

            DialogResult = DialogResult.No;
            Close();
        }

        private void EnableInputs(bool enabled)
        {
            btnSubmit.Enabled = enabled;
            btnBack.Enabled = enabled;
            btnOptionA.Enabled = enabled;
            btnOptionB.Enabled = enabled;
            btnOptionC.Enabled = enabled;
            btnOptionD.Enabled = enabled;
            txtCodeInput.ReadOnly = !enabled;
        }

        private bool IsAnswerMatch(string? playerAnswer, string? correctAnswer)
        {
            if (string.IsNullOrWhiteSpace(playerAnswer) || string.IsNullOrWhiteSpace(correctAnswer))
            {
                return false;
            }

            string normalizedPlayer = playerAnswer.Trim().Replace("\r\n", "\n");
            string normalizedCorrect = correctAnswer.Trim().Replace("\r\n", "\n");
            return string.Equals(normalizedPlayer, normalizedCorrect, StringComparison.OrdinalIgnoreCase);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            CancelQuestion();
        }

        private void CancelQuestion()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void WireMultipleChoiceEvents()
        {
            btnOptionA.Click += MultipleChoiceOption_Click;
            btnOptionB.Click += MultipleChoiceOption_Click;
            btnOptionC.Click += MultipleChoiceOption_Click;
            btnOptionD.Click += MultipleChoiceOption_Click;
        }

        private void MultipleChoiceOption_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            if (sender is not Button selectedButton || _isSubmitting)
            {
                return;
            }

            _selectedOption = selectedButton.Tag?.ToString() ?? string.Empty;
            ClearOptionHighlights();
            HighlightOption(selectedButton);
        }

        private void BattleArenaQuestionForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_isSubmitting)
            {
                return;
            }

            if (TryHandleTypedBufferBackspace(e))
            {
                return;
            }

            if (TryHandleGlobalShortcut(e))
            {
                return;
            }

            HandleMultiChoiceShortcut(e);
        }

        private bool TryHandleTypedBufferBackspace(KeyEventArgs e)
        {
            if (!pnlCodeInput.Visible && e.KeyCode == Keys.Back && _typedCommandBuffer.Length > 0)
            {
                _typedCommandBuffer = _typedCommandBuffer.Substring(0, _typedCommandBuffer.Length - 1);
                e.Handled = true;
                return true;
            }

            return false;
        }

        private bool TryHandleGlobalShortcut(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnBack.PerformClick();
                e.Handled = true;
                return true;
            }

            if (e.KeyCode == Keys.Enter)
            {
                btnSubmit.PerformClick();
                e.Handled = true;
                return true;
            }

            return false;
        }

        private void HandleMultiChoiceShortcut(KeyEventArgs e)
        {
            if (!pnlMultiChoice.Visible)
            {
                return;
            }

            Button? shortcutButton = GetShortcutButton(e.KeyCode);
            if (shortcutButton == null)
            {
                return;
            }

            shortcutButton.PerformClick();
            e.Handled = true;
        }

        private Button? GetShortcutButton(Keys keyCode)
        {
            return keyCode switch
            {
                Keys.A => btnOptionA,
                Keys.B => btnOptionB,
                Keys.C => btnOptionC,
                Keys.D => btnOptionD,
                _ => null
            };
        }

        private void BattleArenaQuestionForm_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (_isSubmitting || pnlCodeInput.Visible || char.IsControl(e.KeyChar))
            {
                return;
            }

            AppendTypedCommandChar(e.KeyChar);
        }

        private void AppendTypedCommandChar(char keyChar)
        {
            _typedCommandBuffer += keyChar;
            if (_typedCommandBuffer.Length > 32)
            {
                _typedCommandBuffer = _typedCommandBuffer.Substring(_typedCommandBuffer.Length - 32);
            }
        }

        private void ClearOptionHighlights()
        {
            ResetOptionButton(btnOptionA);
            ResetOptionButton(btnOptionB);
            ResetOptionButton(btnOptionC);
            ResetOptionButton(btnOptionD);
        }

        private void HighlightOption(Button button)
        {
            button.BackColor = _accentColor;
            button.ForeColor = Color.Black;
            button.FlatAppearance.BorderColor = Color.Black;
        }

        private void ResetOptionButton(Button button)
        {
            button.BackColor = ButtonBackground;
            button.ForeColor = _accentColor;
            button.FlatAppearance.BorderColor = _accentColor;
        }

        private void ApplyAccentPalette(Color accent, Color muted)
        {
            _accentColor = accent;
            _mutedColor = muted;
            ApplyPaletteToLabels(accent, muted);
            ApplyPaletteToActionButtons(accent);
            ApplyPaletteToInputControls(accent);
            ClearOptionHighlights();
            InvalidatePalettePanels();

            if (_isShowingCodeInputPlaceholder)
            {
                txtCodeInput.ForeColor = _mutedColor;
            }
        }

        private void ApplyPaletteToLabels(Color accent, Color muted)
        {
            lblQuestionCounter.ForeColor = accent;
            lblQuestionTag.ForeColor = accent;
            lblQuestion.ForeColor = accent;
            lblHint.ForeColor = accent;
            lblCodeTag.ForeColor = accent;
            lblMCTag.ForeColor = accent;
            lblTimer.ForeColor = accent;
            lblSystemId.ForeColor = muted;
            lblLineNumbers.ForeColor = muted;
            lblQuestion.BackColor = DarkBackground;
        }

        private void ApplyPaletteToActionButtons(Color accent)
        {
            btnBack.ForeColor = accent;
            btnSubmit.ForeColor = accent;
            btnSubmit.FlatAppearance.BorderColor = accent;
            btnBack.FlatAppearance.BorderColor = accent;
        }

        private void ApplyPaletteToInputControls(Color accent)
        {
            txtCodeInput.ForeColor = accent;
            txtCodeInput.BackColor = DarkBackground;
        }

        private void InvalidatePalettePanels()
        {
            pnlTopBar.Invalidate();
            pnlContentFrame.Invalidate();
            pnlMainLayout.Invalidate();
        }

        private void TxtCodeInput_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (_isShowingCodeInputPlaceholder && !char.IsControl(e.KeyChar))
            {
                HideCodeInputPlaceholder(clearText: true);
            }
        }

        private void TxtCodeInput_TextChanged(object? sender, EventArgs e)
        {
            if (_updatingCodeInputPlaceholder)
            {
                return;
            }

            if (!_isShowingCodeInputPlaceholder && txtCodeInput.TextLength == 0)
            {
                ShowCodeInputPlaceholder();
            }
        }

        private void ShowCodeInputPlaceholder()
        {
            _updatingCodeInputPlaceholder = true;
            _isShowingCodeInputPlaceholder = true;
            txtCodeInput.Text = CodeInputPlaceholder;
            txtCodeInput.ForeColor = _mutedColor;
            txtCodeInput.SelectionStart = 0;
            txtCodeInput.SelectionLength = 0;
            _updatingCodeInputPlaceholder = false;
        }

        private void HideCodeInputPlaceholder(bool clearText)
        {
            _updatingCodeInputPlaceholder = true;
            bool wasShowing = _isShowingCodeInputPlaceholder;
            _isShowingCodeInputPlaceholder = false;
            txtCodeInput.ForeColor = _accentColor;

            if (clearText && (wasShowing || string.Equals(txtCodeInput.Text, CodeInputPlaceholder, StringComparison.Ordinal)))
            {
                txtCodeInput.Clear();
            }

            _updatingCodeInputPlaceholder = false;
        }

        #endregion

        #region Button Hover Effects

        private void InvertedButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Enabled)
            {
                AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                btn.BackColor = _accentColor;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
            }
        }

        private void InvertedButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn == btnOptionA || btn == btnOptionB || btn == btnOptionC || btn == btnOptionD)
                {
                    // Keep selected option highlighted.
                    if ((_selectedOption == (btn.Tag?.ToString() ?? string.Empty)) && !_isSubmitting)
                    {
                        HighlightOption(btn);
                        return;
                    }

                    ResetOptionButton(btn);
                    return;
                }

                btn.BackColor = DarkBackground;
                btn.ForeColor = _accentColor;
                btn.FlatAppearance.BorderColor = _accentColor;
            }
        }

        #endregion
    }
}
