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
    public partial class ChangerMdp : Form
    {
        string login;
        public ChangerMdp(string login)
        {
            InitializeComponent();
            this.login = login;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(textBox3.Text))
            {
                var reponse = Database.ChangerMdp(login, textBox2.Text, textBox1.Text);
                if (reponse == 1)
                    this.Close();
            }
            else
                MessageBox.Show("Confirmation du mot de passe incorrect");
        }
    }
}
