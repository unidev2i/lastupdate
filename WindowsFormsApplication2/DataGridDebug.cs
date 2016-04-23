using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class DataGridDebug : Form
    {
        #region Public Constructors

        public DataGridDebug()
        {
            InitializeComponent();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.RowHeadersVisible = false;

            reloadButton.Text = "reload";
            submitButton.Text = "submit";
            reloadButton.Click += reloadButton_Click;
            submitButton.Click += submitButton_Click;

            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] {reloadButton, submitButton});

            Controls.AddRange(new Control[] {dataGridView1, panel});
            Load += Form1_Load;
            Text = "DataGridView databinding and updating demo";
        }

        #endregion Public Constructors

        #region Private Fields

        private readonly BindingSource bindingSource1 = new BindingSource();
        private readonly SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private readonly DataGridView dataGridView1 = new DataGridView();
        private readonly Button reloadButton = new Button();
        private readonly Button submitButton = new Button();

        #endregion Private Fields

        #region Private Methods

        private void Form1_Load(object sender, EventArgs e)
        {
            // Bind the DataGridView to the BindingSource
            // and load the data from the database.
            dataGridView1.DataSource = bindingSource1;
            GetData(
                "SELECT idTp AS TP, date AS Date, idCompetence AS Competence, Note, maxNote AS 'Note Maximum' FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE Nom ='dupond' AND Prenom='jean'");
            //GetData("SELECT idTp AS 'TP', date AS 'Date', idCompetence AS 'Competence', Note, maxNote AS 'Note Maximum' FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE Nom ='dupond' AND Prenom='jean'");
            dataGridView1.AutoResizeColumns();
        }

        private void GetData(string selectCommand)
        {
            // Specify a connection string. Replace the given value with a
            // valid connection string for a Northwind SQL Server sample
            // database accessible to your system.

            var connectionString = "Server=localhost;Uid=root;Database=mydb;";

            var connection = new MySqlConnection(connectionString);

            try
            {
                // Create a new data adapter based on the specified query.
                var dataAdapter = new MySqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                var commandBuilder = new MySqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable();
                table.Locale = CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            // Reload the data from the database.
            GetData(dataAdapter.SelectCommand.CommandText);
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // Update the database with the user's changes.
            dataAdapter.Update((DataTable) bindingSource1.DataSource);
        }

        #endregion Private Methods
    }
}