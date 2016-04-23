using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApplication2.Properties;

using WL;

namespace WindowsFormsApplication2
{
    public partial class ImportTp : Form
    {
        private string _rootFolder;

        public ImportTp(string rootFolder)
        {
            _rootFolder = rootFolder;
            //textBox1.Text = WindowsFormsApplication2.Properties.Settings.Default.repoPath;
            //textBox2.Text = _rootFolder;
            InitializeComponent();
        }

        private void ImportTP_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Checks if some files in the promo directory have changed
        /// </summary>
        /// <param name="name">name of the promo</param>
        /// <returns>true if changed, false if not</returns>
        private bool CheckPromo(string name)
        {
            var request0 = string.Empty;
            foreach (var a in Directory.GetDirectories(_rootFolder))
            {
                request0 += a + " ";
            }
            request0 += "";

            //TODO: REQUEST
            return false;
        }

        private void ReadTp(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                GetValue(file);
            }
        }

        private static void GetValue(string file)
        {
            if (!file.Contains(".pdf"))
                return;

            var b = file;
            var a = new pdfHandler(ref b);
            string c = (string)a.readPDF();


            MessageBox.Show(c);

            const string strRegex = @"C[0-9].[0-9]";
            var myRegex = new Regex(strRegex, RegexOptions.None);
            const string strRegex2 = @"[0-9]{1,2}\.{0,1}[0-9]{0,3}\s{0,2}\/[0-9]{1,2}\.{0,1}[0-9]{0,1}\s";
            var myRegex2 = new Regex(strRegex2, RegexOptions.None);

            var maxMark = new List<string>();

            var sortie = string.Empty;

            var i = 0;

            var skills = (from Match k in myRegex.Matches(sortie) select k.Value).ToList();

            var mark = (from Match l in myRegex2.Matches(sortie) select l.Value).ToList();

            for (var index = 0; index < mark.Count; index++)
            {
                var m = mark[index];

                mark[index] = mark[index].Split('/')[0].RemoveChar('\n').RemoveChar('\r');
                maxMark.Add(m.Split('/')[1]);
            }


            for (var index = 0; index < skills.Count; index++)
            {
                var z = mark[index] + "->" + maxMark[index];
                MessageBox.Show(z);
            }

            // Ceci est tellement horrible MDR
            a = null;
            GC.Collect();
        }

        private bool NeedSend(string filename)
        {
            return true;
        }

        private bool NeedDelete(string filename)
        {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadTp(_rootFolder);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public static class MyString
    {
        public static string RemoveChar(this string x, char b)
        {
            var a = x;
            while (a.Contains(b.ToString()))
            {
                if (a.Length > a.IndexOf(b.ToString(), StringComparison.Ordinal))
                    a.Remove(a.IndexOf(b.ToString(), StringComparison.Ordinal));
            }
            return a;
        }
    }
}
