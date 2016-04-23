using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ChangerMdp : Form
    {
        #region Private Fields

        private readonly string login;

        #endregion Private Fields

        #region Public Constructors

        public ChangerMdp(string login)
        {
            InitializeComponent();
            this.login = login;
        }

        #endregion Public Constructors

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(textBox3.Text))
            {
                var reponse = Database.ChangerMdp(login, textBox2.Text, textBox1.Text);
                if (reponse == 1)
                    Close();
            }
            else
                MessageBox.Show("Confirmation du mot de passe incorrect");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Private Methods
    }
}