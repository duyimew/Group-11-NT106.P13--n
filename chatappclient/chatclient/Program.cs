using Newtonsoft.Json;

using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace QLUSER
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Token token=new Token();
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Task.Run(async () => await token.CheckTokenAndShowForm());
                Application.Run();
                Environment.Exit(0);
            }
            catch (Exception)
            { }

        }
        
    }
}
