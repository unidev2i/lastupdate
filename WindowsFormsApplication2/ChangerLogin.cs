using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ChangerLogin : Form
    {
        #region Public Constructors

        public ChangerLogin(string login, PagePrincipal p1)
        {
            InitializeComponent();
            this.login = login;
            Pp = p1;
        }

        #endregion Public Constructors

        #region Private Fields

        private readonly string login;
        private readonly PagePrincipal Pp;

        #endregion Private Fields

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            var reponse = Database.ChangerLogin(textBox1.Text, textBox2.Text, login);
            if (!reponse.Equals(""))
            {
                Close();
                Pp.Majlog(reponse);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangerLogin_Load(object sender, EventArgs e)
        {
        }

        #endregion Private Methods
    }
}