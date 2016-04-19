using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Suppresion_User : Form
    {
        #region Public Constructors

        public Suppresion_User()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var user = comboBox1.Text;
            var result = Database.Delete(user);
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

        private void Suppresion_User_Load(object sender, EventArgs e)
        {
            foreach (var a in Database.GetListRequest("user", new[] {"Login"}))
            {
                comboBox1.Items.Add(a);
            }
        }

        #endregion Private Methods
    }
}