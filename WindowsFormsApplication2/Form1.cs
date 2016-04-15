using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
