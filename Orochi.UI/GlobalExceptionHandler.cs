using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orochi.UI
{
    public static class GlobalExceptionHandler
    {
        public static void SynchronizeEvents(bool status = true)
        {
            switch (status)
            {
                case true:
                    Application.ThreadException += Application_ThreadException;
                    break;
                case false:
                    Application.ThreadException -= Application_ThreadException;
                    break;
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Exception.Message + Environment.NewLine + e.Exception.InnerException + Environment.NewLine + Environment.NewLine + e.Exception.ToString() + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
