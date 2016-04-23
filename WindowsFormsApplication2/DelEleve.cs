using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class DelEleve : Form
    {
        #region Public Constructors

        public DelEleve()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            var eleve = comboBox1.Text;
            var result = Database.DeleteElv(eleve);
            if (result == 1)
            {
                MessageBox.Show(" * Suppression Réussie * ");
                Close();
            }
            else
            {
                MessageBox.Show(" * Erreur lors de la suppression * ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var promo = comboBox2.Text;
            comboBox1.Items.Clear();
            var eleve = new string[100];
            var i = 0;
            eleve = Database.PromoEleve(promo);
            if (eleve != null)
            {
                for (i = 0; (i < eleve.Length) && (eleve[i] != null); i++)
                {
                    //MessageBox.Show(eleve[i]);
                    comboBox1.Items.Add(eleve[i]);
                }
            }
        }

        private void DelEleve_Load(object sender, EventArgs e)
        {
            foreach (var a in Database.GetListRequest("eleve", new[] {"Prenom", "Nom"}))
            {
                comboBox1.Items.Add(a);
            }
            foreach (var a in Database.GetListRequest("classe", new[] {"Promotion"}))
            {
                comboBox2.Items.Add(a);
            }
        }

        #endregion Private Methods
    }
}