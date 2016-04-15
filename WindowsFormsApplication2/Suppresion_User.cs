using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication2
{
    public partial class Suppresion_User : Form
    {
        public Suppresion_User()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = comboBox1.Text;
            int result = Database.Delete(user);
            if (result == 1)
            {
                MessageBox.Show(" * Suppression Réussie * ");
                this.Close();
            }
            else
            {
                MessageBox.Show(" * Erreur lors de la suppression * ");

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Suppresion_User_Load(object sender, EventArgs e)
        {
            foreach (var a in Database.GetListRequest("user", new[] { "Login" }))
            {
                comboBox1.Items.Add(a);
            }
        }

    }
}
