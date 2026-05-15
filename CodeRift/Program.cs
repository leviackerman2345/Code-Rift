using System;
using System.Windows.Forms;
using CodeRift.Forms;

namespace CodeRift
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Start with the Splash Form
            Application.Run(new SplashForm());
        }
    }
}
