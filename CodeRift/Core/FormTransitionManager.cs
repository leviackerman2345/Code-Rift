using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeRift.Core
{
    public static class FormTransitionManager
    {
        private static bool _isTransitioning;

        public static bool IsTransitioning => _isTransitioning;

        public static bool ShowChild(Form owner, Form child, Func<bool>? onChildClosed = null)
        {
            if (!CanStartTransition(owner, child))
            {
                return false;
            }

            BeginTransition(owner);
            FormWindowState ownerState = owner.WindowState;
            Rectangle ownerBounds = owner.Bounds;
            PrepareChildForm(owner, child, ownerState, ownerBounds);

            child.Shown += (_, _) =>
            {
                // Child is ready, fade out owner and fade in child.
                _ = FadeOutOwnerAsync(owner);
                ShowNextForm(owner, child);
            };

            child.FormClosed += (_, _) =>
            {
                bool shouldShowOwner = onChildClosed?.Invoke() ?? true;
                if (!shouldShowOwner || owner.IsDisposed)
                {
                    return;
                }

                RestoreOwnerForm(owner, ownerState, ownerBounds);
            };

            try
            {
                child.Show();
                return true;
            }
            catch
            {
                EndTransition(owner);
                return false;
            }
        }

        private static bool CanStartTransition(Form owner, Form child)
        {
            return !_isTransitioning && !owner.IsDisposed && !child.IsDisposed;
        }

        private static void BeginTransition(Form owner)
        {
            _isTransitioning = true;
            owner.Enabled = false;
        }

        private static void PrepareChildForm(Form owner, Form child, FormWindowState ownerState, Rectangle ownerBounds)
        {
            child.StartPosition = FormStartPosition.Manual;
            child.Opacity = 0d;

            if (ownerState == FormWindowState.Maximized)
            {
                child.WindowState = FormWindowState.Maximized;
            }
            else
            {
                child.Bounds = ownerBounds;
            }
        }

        private static void ShowNextForm(Form owner, Form child)
        {
            if (!child.IsDisposed)
            {
                child.Opacity = 1d;
                child.Update();
                child.Activate();
            }

            if (!owner.IsDisposed)
            {
                owner.Hide();
            }

            EndTransition(owner);
        }

        private static void RestoreOwnerForm(Form owner, FormWindowState ownerState, Rectangle ownerBounds)
        {
            owner.Opacity = 0d;
            owner.SuspendLayout();

            if (ownerState == FormWindowState.Maximized)
            {
                owner.WindowState = FormWindowState.Maximized;
            }
            else
            {
                owner.WindowState = FormWindowState.Normal;
                owner.Bounds = ownerBounds;
            }

            owner.Show();
            owner.ResumeLayout(performLayout: true);
            owner.Activate();
            _ = FadeInOwnerAsync(owner);
        }

        private static void EndTransition(Form owner)
        {
            if (!owner.IsDisposed)
            {
                owner.Enabled = true;
            }

            _isTransitioning = false;
        }

        private static async Task FadeInOwnerAsync(Form owner)
        {
            if (owner.IsDisposed)
            {
                return;
            }

            const int steps = 10;
            const int delayMs = 16;

            for (int i = 0; i < steps; i++)
            {
                if (owner.IsDisposed)
                {
                    return;
                }

                owner.Opacity = Math.Min(1d, (i + 1) / (double)steps);
                await Task.Delay(delayMs);
            }
        }

        private static async Task FadeOutOwnerAsync(Form owner)
        {
            if (owner.IsDisposed)
            {
                return;
            }

            const int steps = 10;
            const int delayMs = 16;

            for (int i = 0; i < steps; i++)
            {
                if (owner.IsDisposed)
                {
                    return;
                }

                owner.Opacity = Math.Max(0d, 1d - (i + 1) / (double)steps);
                await Task.Delay(delayMs);
            }
        }
    }
}
