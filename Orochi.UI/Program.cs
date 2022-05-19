using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Orochi.UI
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        internal static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AllocConsole();

        [STAThread]
        static void Main()
        {
            GlobalExceptionHandler.SynchronizeEvents(true);
            AllocConsole();
            Console.Title = "Project Orochi";
            Console.WriteLine("Project Orochi: https://github.com/MrReverse/Project-Orochi");
            Console.WriteLine("Do not forget to fork & star!");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine();
            System.Threading.Thread.Sleep(300);
            Console.WriteLine("Credits: ");
            System.Threading.Thread.Sleep(550);
            Console.WriteLine("Thanks to dnlib for modifying modules.");
            System.Threading.Thread.Sleep(550);
            Console.WriteLine("Thanks to user76 for helping me about some problems.");
            System.Threading.Thread.Sleep(550);
            Console.WriteLine("Thanks to ConfuserEx source for runtime injecting (i hope it works).");
            System.Threading.Thread.Sleep(550);
            Console.WriteLine("Thanks to Stackoverflow users for helping me about some problems.");
            System.Threading.Thread.Sleep(550);
            Console.WriteLine("Made by Excepti0n aka MrReverse.");
            System.Threading.Thread.Sleep(2550);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Application.Exit();
        }
    }
}
