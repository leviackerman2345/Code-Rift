using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeRift.Core
{
    public static class FormTransitionManager
    {
        private const int FadeDurationMs = 180;
        private const int FadeIntervalMs = 15;
        private static bool _isTransitioning;

        public static bool IsTransitioning => _isTransitioning;

        public static bool ShowChild(Form owner, Form child, Func<bool>? onChildClosed = null)
        {
            if (_isTransitioning || owner.IsDisposed || child.IsDisposed)
            {
                return false;
            }

            _isTransitioning = true;
            child.Opacity = 0;

            child.Shown += async (_, _) =>
            {
                await FadeAsync(child, 1.0);

                if (!owner.IsDisposed)
                {
                    owner.Hide();
                    owner.Opacity = 1.0;
                }

                _isTransitioning = false;
            };

            child.FormClosed += async (_, _) =>
            {
                bool shouldShowOwner = onChildClosed?.Invoke() ?? true;
                if (!shouldShowOwner || owner.IsDisposed)
                {
                    return;
                }

                owner.Opacity = 0;
                owner.Show();
                await FadeAsync(owner, 1.0);
            };

            child.Show();
            return true;
        }

        private static async Task FadeAsync(Form form, double targetOpacity)
        {
            if (form.IsDisposed)
            {
                return;
            }

            double startOpacity = form.Opacity;
            int elapsed = 0;

            while (elapsed < FadeDurationMs && !form.IsDisposed)
            {
                elapsed += FadeIntervalMs;
                double amount = Math.Min(1.0, elapsed / (double)FadeDurationMs);
                form.Opacity = startOpacity + ((targetOpacity - startOpacity) * amount);
                await Task.Delay(FadeIntervalMs);
            }

            if (!form.IsDisposed)
            {
                form.Opacity = targetOpacity;
            }
        }
    }
}
