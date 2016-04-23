using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Aide : Form
    {
        #region Public Constructors

        public Aide()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var p = new Process();
            p.StartInfo.FileName = "http://www.ig2i.fr";
            p.Start();
        }

        #endregion Private Methods
    }
}