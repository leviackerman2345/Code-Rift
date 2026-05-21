namespace CodeRift
{
    internal static class Program
    {
        /// <summary>
        /// Main entry point. App always starts at splash/loading screen (Form1).
        /// Defense note: mention this as the single startup boundary.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
