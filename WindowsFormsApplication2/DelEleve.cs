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
    public partial class DelEleve : Form
    {
        public DelEleve()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eleve = comboBox1.Text;
            int result = Database.DeleteElv(eleve);
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

        private void DelEleve_Load(object sender, EventArgs e)
        {
            foreach (var a in Database.GetListRequest("eleve", new[] { "Prenom", "Nom" }))
            {
                comboBox1.Items.Add(a);
            }
            foreach (var a in Database.GetListRequest("classe", new[] { "Promotion"}))
            {
                comboBox2.Items.Add(a);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String promo = comboBox2.Text;
            comboBox1.Items.Clear();
            string[] eleve = new String[100];
            var i=0;
            eleve = Database.PromoEleve(promo);
            if (eleve != null)
            {
                for (i = 0; i < eleve.Length && eleve[i] != null; i++)
                {
                    //MessageBox.Show(eleve[i]);
                    comboBox1.Items.Add(eleve[i]);
                }
            }
        }

    }
}
