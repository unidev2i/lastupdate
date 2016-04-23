using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    static class Program
    {
        //public static string repoPath;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try 
            {
                /*ProcessStartInfo startServ = new ProcessStartInfo("mysql\\start.exe");
                Process.Start(startServ);                                           //A décommenter avant mise en oeuvre
                System.Threading.Thread.Sleep(1000);*/

                Database.Connect();
            
            }
            catch { MessageBox.Show("Impossible de se connecter à la BDD"); }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AssistantConnexion());

          /*ProcessStartInfo stopServ = new ProcessStartInfo("mysql\\stop.exe");
            Process.Start(stopServ);                                            //A décommenter avant mise en oeuvre
            System.Threading.Thread.Sleep(1000); */
           
        }
    }
}
