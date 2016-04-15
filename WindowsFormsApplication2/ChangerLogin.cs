using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ChangerLogin : Form
    {
        string login;
        PagePrincipal Pp;

        public ChangerLogin(string login,PagePrincipal p1)
        {
            InitializeComponent();
            this.login = login;
            this.Pp = p1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string reponse = Database.ChangerLogin(textBox1.Text, textBox2.Text,login);
            if (!reponse.Equals(""))
            {
                this.Close();
                Pp.Majlog(reponse);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangerLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
