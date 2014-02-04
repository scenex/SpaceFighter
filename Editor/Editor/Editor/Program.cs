using System;

namespace Editor
{
    using System.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var editor = new MainForm())
            {
                Application.Run(editor);
            }
        }
    }
}

