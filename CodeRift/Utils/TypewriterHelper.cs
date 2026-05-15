using System;
using System.Windows.Forms;

namespace CodeRift.Utils
{
    public static class TypewriterHelper
    {
        public static void Start(Label lbl, string text, int delayMs, Action? onComplete = null)
        {
            lbl.Text = "";
            int index = 0;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = delayMs;
            timer.Tick += (s, e) =>
            {
                if (index < text.Length)
                {
                    lbl.Text += text[index++];
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                    onComplete?.Invoke();
                }
            };
            timer.Start();
        }
    }
}
