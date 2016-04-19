using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class AjoutEleve : Form
    {
        #region Public Constructors

        public AjoutEleve()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void AjoutEleve_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Bouton s'inscrire appuyer
            var nom = textBox2.Text;
            var prenom = textBox1.Text;
            var promo = textBox3.Text;
            var idClasse = Database.GetidClasse(promo);
            MessageBox.Show(idClasse);

            ("INSERT INTO eleve (idClasse,Nom,Prenom) VALUES ('" + idClasse + "','" + nom + "','" + prenom + "')")
                .SimpleRequest();
            MessageBox.Show("Ajout Réussit");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Private Methods
    }
}