using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        #region Public Constructors

        public Form1(Chart a)
        {
            InitializeComponent();
            chart1.Series[0].Points.Clear();
            chart1.Series[0].ChartType = a.Series[0].ChartType;

            foreach (var b in a.Series[0].Points)
            {
                chart1.Series[0].Points.Add(b);
            }
        }

        #endregion Public Constructors

        #region Private Methods

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #endregion Private Methods
    }
}