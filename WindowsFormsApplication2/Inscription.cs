using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Inscription : Form
    {
        #region Private Fields

        private AssistantConnexion form1;

        #endregion Private Fields

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var mdp = textBox2.Text;
            var statut = 0;
            if (checkBox1.Checked)
                statut = 1;
            if ((login != "") && (mdp != ""))
            {
                var result = Database.AddUser(login, mdp, statut);

                if (result == 1)
                {
                    MessageBox.Show(" * Inscription réussi * ");
                    Close();
                }
                else
                    MessageBox.Show(" * Erreur de la fonction AddUser de Database.cs * ");
            }
            else
                MessageBox.Show(" * Vous ne pouvez pas vous inscrire avec un champ vide * ");
        }

        #endregion Private Methods

        #region Public Constructors

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

        #endregion Public Constructors
    }
}