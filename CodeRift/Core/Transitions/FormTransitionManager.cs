using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeRift.Core
{
    public static class FormTransitionManager
    {
        private static bool _isTransitioning;
        private static readonly Dictionary<Form, CancellationTokenSource> ActiveFades = new Dictionary<Form, CancellationTokenSource>();

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
                // Force one draw pass before hiding owner to avoid desktop/IDE flash-through.
                child.BeginInvoke(new Action(() =>
                {
                    child.Update();
                    ShowNextForm(owner, child);
                }));
            };

            child.FormClosed += (_, _) =>
            {
                bool shouldShowOwner = onChildClosed?.Invoke() ?? true;
                if (!shouldShowOwner || owner.IsDisposed)
                {
                    CancelActiveFade(child);
                    return;
                }

                CancelActiveFade(child);
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
            // Cancel any active fade-in on the owner since it is now being hidden.
            CancelActiveFade(owner);

            if (!child.IsDisposed)
            {
                child.Update();
                child.Activate();
                _ = FadeInFormAsync(child, () =>
                {
                    EndTransition(owner);
                });
            }
            else
            {
                EndTransition(owner);
            }
        }

        private static void RestoreOwnerForm(Form owner, FormWindowState ownerState, Rectangle ownerBounds)
        {
            CancelActiveFade(owner);

            if (ownerState == FormWindowState.Maximized)
            {
                owner.WindowState = FormWindowState.Maximized;
            }
            else
            {
                owner.WindowState = FormWindowState.Normal;
                owner.Bounds = ownerBounds;
            }

            // Since the owner form was never hidden, we just instantly bring it back into focus!
            // Zero lag, zero reloading, zero opacity fades!
            owner.Activate();
            EndTransition(owner);
        }

        public static void ForceEndTransition(Form owner)
        {
            EndTransition(owner);
        }

        private static void EndTransition(Form owner)
        {
            if (!owner.IsDisposed)
            {
                owner.Enabled = true;
            }

            _isTransitioning = false;
        }

        private static void CancelActiveFade(Form form)
        {
            lock (ActiveFades)
            {
                if (ActiveFades.TryGetValue(form, out var cts))
                {
                    try
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }
                    catch
                    {
                        // Ignore occasional cleanup exceptions
                    }
                    ActiveFades.Remove(form);
                }
            }
        }

        private static async Task FadeInFormAsync(Form form, Action? onComplete = null)
        {
            if (form.IsDisposed)
            {
                onComplete?.Invoke();
                return;
            }

            CancellationTokenSource cts;
            lock (ActiveFades)
            {
                if (ActiveFades.TryGetValue(form, out var existingCts))
                {
                    try
                    {
                        existingCts.Cancel();
                        existingCts.Dispose();
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                cts = new CancellationTokenSource();
                ActiveFades[form] = cts;
            }

            CancellationToken token = cts.Token;

            try
            {
                // Smooth 60fps-like visual fade
                const int steps = 12;
                const int delayMs = 10;

                for (int i = 0; i < steps; i++)
                {
                    if (token.IsCancellationRequested || form.IsDisposed)
                    {
                        return;
                    }

                    form.Opacity = Math.Min(1d, (i + 1) / (double)steps);
                    
                    await Task.Delay(delayMs, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Expected when canceled
            }
            finally
            {
                lock (ActiveFades)
                {
                    if (ActiveFades.TryGetValue(form, out var currentCts) && currentCts == cts)
                    {
                        ActiveFades.Remove(form);
                    }
                }
                cts.Dispose();
                onComplete?.Invoke();
            }
        }
    }
}
