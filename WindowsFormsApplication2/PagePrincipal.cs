using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static WindowsFormsApplication2.Database;

namespace WindowsFormsApplication2
{
    public partial class PagePrincipal : Form
    {
        #region Private Fields

        private readonly BindingSource _bindingSource1 = new BindingSource();
        private readonly AssistantConnexion _form1;
/*
        private DataGridDebug _dataForm;
*/
        private AjoutEleve _graphic;
        private Inscription _graphic2;
        private Suppresion_User _graphic3;
        private ChangerLogin _graphic4;
        private ChangerMdp _graphic5;
        private DelEleve _graphic7;
        private string _login;

        #endregion Private Fields

        #region Public Constructors

        public PagePrincipal()
        {
            InitializeComponent();
            dataGridView1.AutoResizeColumns();
        }

        public PagePrincipal(AssistantConnexion form1, string login, bool statut)
        {
            InitializeComponent();
            _form1 = form1;
            if (statut == false)
            {
                aToolStripMenuItem.Visible = false;
            }

            HelloBox(login);
        }

        #endregion Public Constructors

        #region Public Methods

        public void HelloBox(string nom)
        {
            label4.Text = @"Professeur connecté: " + nom;
            _login = nom;
        }

        public void Majlog(string newlog)
        {
            _login = newlog;
            HelloBox(_login);
        }

        public void UpdateLogin(string login)
        {
            _login = login;
            label4.Text = @"Professeur connecté: " + login;
        }

        #endregion Public Methods

        #region Private Methods

        private void ajouterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _graphic2 = new Inscription();
            _graphic2.Show();
        }

        private void ajouterToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //BOUTON POUR AJOUTER UN ELEVE
            _graphic = new AjoutEleve();
            _graphic.Show();
        }

        private void ajouterUnPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void changerDeLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphic4 = new ChangerLogin(_login, this);
            _graphic4.Show();
        }

        private void changerDeMotDePasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphic5 = new ChangerMdp(_login);
            _graphic5.Show();
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            new Form1(chart2).Show();
        }

        private void chart3_Click(object sender, EventArgs e)
        {
            new Form1(chart3).Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CHANGEMENT D'ELEVE, FAIRE LES MODIFICATIONS DES GRAPHIQUES ...
            var str = comboBox1.Text;
            var result = Regex.Split(str, " ");
            var prenom = result[0];
            var nom = result[1];
            var idClasse = "";
            var classe = new List<string>();

            foreach (
                var a in
                    GetListRequest("eleve", new[] {"idClasse"}, "Nom='" + nom + "' and Prenom='" + prenom + "'")
                )
                idClasse = a;
            comboBox3.Text = getpromo(idClasse);

            comboBox1.Select(50, 50);

            var str2 = Regex.Split(comboBox1.Text, " ");
            var idEleve = "";

            foreach (
                var a in
                    GetListRequest("eleve", new[] {"idEleve"},
                        "Nom='" + str2[1] + "' and Prenom='" + str2[0] + "'"))
                idEleve = a ?? "1";

            GetData(
                "SELECT Prenom, Nom, idTp AS TP, date AS Date, idCompetence AS Competence, Note, maxNote AS 'Note Maximum' FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE idEleve='" +
                idEleve + "'");
            dataGridView1.AutoResizeColumns();

            // Draw graphics

            var w = GetWtfRequest(idEleve);
            var z = GetWebRequest(idEleve);

            DrawGraph(w);
            DrawWeb(z);
            chart1.Visible = true;
            chart2.Visible = true;
            chart3.Visible = true;
            //chart1.ChartAreas[0].AxisX.Maximum = 100;
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            var concatenate = comboBox1.Text;
            var i = 0;
            var eleve = EcritureInteligente(concatenate);
            if (eleve != null)
            {
                for (i = 0; (i < eleve.Length) && (eleve[i] != null); i++)
                {
                    //MessageBox.Show(eleve[i]);
                    comboBox1.Items.Add(eleve[i]);
                }
            }
            comboBox1.Select(50, 50);
            comboBox3.Select(50, 50);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //REFAIRE PAREIL QUE POUR LE NIVEAU
            var i = 0;
            var promo = comboBox3.Text;
            var eleve = new string[1000];
            eleve = RecupEleveAvecPromo(promo);
            comboBox1.Items.Clear();
            if (eleve != null)
            {
                for (i = 0; (i < eleve.Length) && (eleve[i] != null); i++)
                {
                    //MessageBox.Show(eleve[i]);
                    comboBox1.Items.Add(eleve[i]);
                }
            }
        }

        private void comboBox3_TextUpdate(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            foreach (var a in GetListRequest("classe", new[] {"numClasse"}))
                comboBox3.Items.Add(a);
            comboBox3.Select(50, 50);
        }

/*
        private void dataGridDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dataForm = new DataGridDebug();
            _dataForm.Show();
        }
*/

        private void deconnexionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sgInfo = new ProcessStartInfo("WindowsFormsApplication2.exe");
            Process.Start(sgInfo);
            Close();
        }

        private void DrawGraph(List<Tuple<string, float, int>> tuples)
        {
            var array = tuples.Select(a => a.Item1).ToList();
            var parray = tuples.Select(a => a.Item2).ToList();
            var xarray = tuples.Select(a => a.Item3).ToList();

            //chart1.Palette = ChartColorPalette.Excel;
            chart1.Series.Clear();

            for (var a = 0; a != array.Count; a++)
            {
                chart1.Series.Add(new Series(array[a]));
            }

            var i = 0;
            foreach (var tSeries in from tSeries in chart1.Series let a = tSeries select tSeries)
            {
                tSeries.Points.AddY(parray[i]);
                i++;
            }

            chart2.Series[0].Points.Clear();

            foreach (var a in tuples)
            {
                chart2.Series[0].Points.AddXY(a.Item1 + Environment.NewLine + a.Item3, a.Item3);
            }
        }

        private void DrawWeb(IEnumerable<Tuple<string, float>> aTuples)
        {
            chart3.Series[0].Points.Clear();
            foreach (var a in aTuples)
            {
                var p = chart3.Series[0].Points.Add(a.Item2);
                p.Name = a.Item1;
                p.AxisLabel = a.Item1;
                // p.Label = a.Item1;
            }
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e) => BackupDatabase();

        private void Form3_FormClosing_1(object sender, FormClosingEventArgs e) => _form1.Close();

        private void Form3_Load_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _bindingSource1;
            dataGridView1.RowHeadersVisible = false;

            foreach (var a in GetListRequest("eleve", new[] {"Prenom", "Nom"}))
                comboBox1.Items.Add(a);

            foreach (var a in GetListRequest("classe", new[] {"Promotion"}))
                comboBox3.Items.Add(a);
        }

        private void GetData(string selectCommand)
        {
            var connectionString = "Server=" + Server + ";Uid=" + Username + ";Database=" +
                                   DatabaseName + ";Password=" + Password + ";";
            var connection = new MySqlConnection(connectionString);
            try
            {
                var dataAdapter = new MySqlDataAdapter(selectCommand, connectionString);
                var commandBuilder = new MySqlCommandBuilder(dataAdapter);

                var table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);
                _bindingSource1.DataSource = table;

                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RestoreDatabase() != 0) return;

            var sgInfo = new ProcessStartInfo("WindowsFormsApplication2.exe");
            Process.Start(sgInfo);
            Close();
        }

        private void importerTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphic3 = new Suppresion_User();
            _graphic3.Show();
        }

        private void supprimerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _graphic7 = new DelEleve();
            _graphic7.Show();
        }

/*
        private void unEleveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
*/

        #endregion Private Methods
    }
}