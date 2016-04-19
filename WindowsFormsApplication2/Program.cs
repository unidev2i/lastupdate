using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Database.Connect();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AssistantConnexion());
        }

        #endregion Private Methods
    }
}