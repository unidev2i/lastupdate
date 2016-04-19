using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class AssistantConnexion : Form
    {
        #region Public Constructors

        public AssistantConnexion()
        {
            InitializeComponent();
            Verrouiller();
        }

        #endregion Public Constructors

        #region Private Fields

        private Aide _apropos;
        private PagePrincipal _graphic;
        private Inscription _graphic2;

        #endregion Private Fields

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var mdp = textBox2.Text;

            // lr -> login result
            var lr = Database.Login(login, mdp);

            if ((lr != 0) && (lr != 1))
                return;

            _graphic = new PagePrincipal(this, textBox1.Text, lr == 1);
            _graphic.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _apropos = new Aide();
            _apropos.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Bouton s'inscrire appuyer
            _graphic2 = new Inscription(this);
            _graphic2.Show();
        }

        private void Deverrouiller()
        {
            pictureBox2.Hide();
            pictureBox4.Show();
            textBox2.PasswordChar = (char) 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Deverrouiller();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Verrouiller();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void Verrouiller()
        {
            pictureBox2.Show();
            pictureBox4.Hide();
            textBox2.PasswordChar = '*';
        }

        #endregion Private Methods
    }
}