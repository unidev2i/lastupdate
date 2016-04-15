using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApplication2
{
    public partial class Inscription : Form
    {
        private AssistantConnexion form1;
        public Inscription()
        {
            InitializeComponent();
        }

        public Inscription(AssistantConnexion form1)
        {
            InitializeComponent();
            this.form1 = form1;
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string mdp = textBox2.Text;
            int statut = 0;
            if (checkBox1.Checked)
                statut = 1;
            if (login != "" && mdp != "")
            {
                int result = Database.AddUser(login, mdp, statut);

                if (result == 1)
                {
                    MessageBox.Show(" * Inscription réussi * ");
                    this.Close();
                }
                else
                    MessageBox.Show(" * Erreur de la fonction AddUser de Database.cs * ");
            }
            else
                MessageBox.Show(" * Vous ne pouvez pas vous inscrire avec un champ vide * ");

        }

    }
}
