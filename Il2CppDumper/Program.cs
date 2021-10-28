using System;
using System.Windows.Forms;

namespace Il2CppDumper
{
    static class Program
    {
        public static FrmMain frmMain;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(args.Length > 0 ? frmMain = new FrmMain(args) : frmMain = new FrmMain());
        }
    }
}