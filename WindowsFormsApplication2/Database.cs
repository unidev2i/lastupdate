using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public static class Database
    {
        #region Private Fields

        private const string COL_ADMIN = "Admin";
        private const string COL_IDELEVE = "idEleve";
        private const string COL_IDSKILL = "idCompetence";
        private const string COL_LOGIN = "Login";
        private const string COL_MAXNOTE = "maxNote";
        private const string COL_NOTE = "note";
        private const string COL_PASS = "Password";
        private const string TAB_ELEVE = "eleve";
        private const string TAB_TP = "tp";
        private const string TAB_USER = "user";
        private static MySqlConnection _conn;

        #endregion Private Fields

        #region Public Properties

        public static string DatabaseName { get; private set; }
        public static string Password { get; private set; }
        public static string Server { get; private set; }

        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static int AddUser(string login, string mdp, int statut)
        {
            var command = _conn.CreateCommand();
            command.CommandText =
                $"INSERT INTO {TAB_USER}({COL_LOGIN},{COL_PASS},{COL_ADMIN}) VALUES ('{login}','{mdp}','{statut}')";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si erreur il y a
            {
                retour.Close();
                return 1;
            }
            retour.Close();
            return 1;
        }

        public static void BackupDatabase(string backUpFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        var fileData = new SaveFileDialog();
                        fileData.Title = @"Exporter la base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = @"Fichier SQL (*.sql)|*.sql";
                        var result = fileData.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            backUpFile = fileData.FileName;
                        }
                        cmd.Connection = _conn;
                        mb.ExportToFile(backUpFile);
                        Connect();
                    }
                }
            }
        }

        /// <summary>
        ///     The changer login.
        /// </summary>
        /// <param name="login">
        ///     The login.
        /// </param>
        /// <param name="pass">
        ///     The pass.
        /// </param>
        /// <param name="ancienlog">
        ///     The ancienlog.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string ChangerLogin(string login, string pass, string ancienlog)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + ancienlog +
                                  "'";

            var pass2 = string.Empty;
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString();
            }
            retour.Close();

            if ((pass == pass2) && (login != string.Empty))
            {
                var request = $"UPDATE {TAB_USER} SET {COL_LOGIN}='{login}' WHERE {TAB_USER}='{ancienlog}'";
                var command2 = _conn.CreateCommand();
                command2.CommandText = request;
                command2.ExecuteNonQuery();

                MessageBox.Show(@"Changement réussi");
                return login;
            }

            MessageBox.Show(@"Mot de passe actuel incorrect");
            return string.Empty;
        }

        public static int ChangerMdp(string login, string pass, string ancienmdp)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + login +
                                  "'";

            var pass2 = string.Empty;
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString(); // mot de passe récuperé de la BDD
            }
            retour.Close();

            if ((ancienmdp == pass2) && (pass != string.Empty))
                // on compare le mdp récuperé avec celui tapé dans le champ "ancien mot de passe"
            {
                // on vérifie que le nouveau mdp ne soit pas vide
                var request = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}='{4}' AND {1}='{5}'", TAB_USER, COL_PASS,
                    pass, COL_LOGIN, login, ancienmdp);
                var command2 = _conn.CreateCommand();
                command2.CommandText = request;
                command2.ExecuteNonQuery();

                MessageBox.Show(@"Changement réussi");
                return 1;
            }

            MessageBox.Show(@"Ancien mot de passe incorrect");
            return 0;
        }

        public static void Connect(string ip = "localhost", string login = "root", string pass = "",
            string database = "mydb")
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = ip,
                UserID = login,
                Password = pass,
                Database = database
            };

            Server = ip;
            Username = login;
            Password = pass;
            DatabaseName = database;

            _conn = new MySqlConnection(builder.ToString());
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static int Delete(string user)
        {
            var command = _conn.CreateCommand();
            command.CommandText = $"DELETE FROM {TAB_USER} WHERE {COL_LOGIN}='{user}'";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si Erreur il y a
            {
                retour.Close();
                return -1;
            }
            retour.Close();
            return 1;
        }

        public static int DeleteElv(string eleve)
        {
            var eleve2 = eleve.Split(' ');
            MessageBox.Show(eleve2[0] + "   " + eleve2[1]);
            var command = _conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TAB_ELEVE + " WHERE Nom ='" + eleve2[0] + "'AND Prenom='" + eleve2[1] +
                                  "'";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si Erreur il y a
            {
                retour.Close();
                return -1;
            }
            retour.Close();
            return 1;
        }

        /// <summary>
        ///     The del request.
        /// </summary>
        /// <param name="table">
        ///     The table.
        /// </param>
        /// <param name="where_clause">
        ///     The where_clause.
        /// </param>
        public static void DelRequest(string table, Tuple<string, string>[] where_clause)
        {
            var request = "DELETE FROM " + table + " WHERE ";

            var secondLine = false;
            foreach (var a in where_clause)
            {
                if (secondLine)
                {
                    request += " AND ";
                }
                request += a.Item1 + "='" + a.Item2 + "'";
                secondLine = true;
            }

            var command = _conn.CreateCommand();
            command.CommandText = request;
            command.ExecuteNonQuery();
        }

        public static string[] EcritureInteligente(string concatenate)
        {
            var retour = new string[1000];
            var i = 0;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Prenom,Nom FROM eleve";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var concatenate2 = $"{reader["Prenom"]} {reader["Nom"]}";
                if (!concatenate2.Contains(concatenate)) continue;
                retour[i] = concatenate2;
                i++;
            }
            reader.Close();
            return retour;
        }

        public static string GetidClasse(string promo)
        {
            var idclasse = string.Empty;
            var command = _conn.CreateCommand();
            command.CommandText = $"SELECT idClasse FROM classe WHERE Promotion ='{promo}'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idclasse = reader["idClasse"].ToString();
            }
            var id = idclasse;
            //MessageBox.Show(id);
            reader.Close();
            var Location = 1;
            if (id == string.Empty)
            {
                ("INSERT INTO classe (Promotion,Location) VALUES ('" + promo + "','" + Location + "')").SimpleRequest();

                var command2 = _conn.CreateCommand();
                command2.CommandText = $"SELECT idClasse FROM classe WHERE Promotion ='{promo}'";
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    idclasse = reader2["idClasse"].ToString();
                }
                id = idclasse;
                reader2.Close();
                MessageBox.Show(id);
            }
            return id;
        }

        /// <summary>
        ///     The get list request.
        /// </summary>
        /// <param name="table">
        ///     The table.
        /// </param>
        /// <param name="columns">
        ///     The columns.
        /// </param>
        /// <param name="additionalWhereClause">
        ///     The additional_where_clause.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public static IEnumerable<string> GetListRequest(string table, string[] columns,
            string additionalWhereClause = "1")
        {
            var retour = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT * FROM " + table + " WHERE " + additionalWhereClause;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                var toReturn = columns.Aggregate(string.Empty, (current, c) => current + r[c].ToString() + " ");
                retour.Add(toReturn);
            }

            r.Close();
            return retour;
        }

        public static string getpromo(string idClasse)
        {
            var promo = string.Empty;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Promotion FROM classe WHERE idClasse ='" + idClasse + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                promo = reader["Promotion"].ToString();
            }
            var promo2 = promo;
            //MessageBox.Show(id);
            reader.Close();

            return promo2;
        }

        public static List<Tuple<string, float>> GetWebRequest(string idEleve = "2")
        {
            var retour = new List<Tuple<string, float>>();
            var req =
                "SELECT " + COL_IDSKILL + ", SUM(" + COL_NOTE + ") FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP +
                " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = '" + idEleve + "' GROUP BY " + COL_IDSKILL;
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[COL_IDSKILL].ToString(),
                    float.Parse(r["SUM(" + COL_NOTE + ")"].ToString())));
            }

            r.Close();
            return retour;
        }

        /// <summary>
        ///     The get wtf request.
        /// </summary>
        /// <param name="idEleve">
        ///     The id eleve.
        /// </param>
        /// <returns>
        ///     The <see cref="List" />.
        /// </returns>
        public static List<Tuple<string, float, int>> GetWtfRequest(string idEleve = "2")
        {
            var req =
                string.Format(
                    "SELECT {0},COUNT({1}) AS quantite, ((SUM({1}/{2})/COUNT({1}))*100) AS moyenne FROM {3} NATURAL JOIN {4} NATURAL JOIN {1} WHERE {5} = {6} GROUP BY {0}",
                    COL_IDSKILL, COL_NOTE, COL_MAXNOTE, TAB_ELEVE, TAB_TP, COL_IDELEVE, idEleve);
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[COL_IDSKILL].ToString(), float.Parse(r["moyenne"].ToString()),
                    int.Parse(r["quantite"].ToString())));
            }

            r.Close();
            return a;
        }

        /// <summary>
        ///     The login.
        /// </summary>
        /// <param name="login">
        ///     The login.
        /// </param>
        /// <param name="pass">
        ///     The pass.
        /// </param>
        /// <returns>
        ///     1=admin 0=user -1=nothing
        /// </returns>
        public static int Login(string login, string pass)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_LOGIN + "," + COL_PASS + "," + COL_ADMIN + " FROM " + TAB_USER +
                                  " WHERE " + COL_LOGIN + "='" + login + "' AND " + COL_PASS + "='" + pass + "'";
            var retour = command.ExecuteReader();

            if (!retour.Read())
            {
                // JE RAJOUTE UN COMMENTAIRE
                retour.Close();
                return -1;
            }

            var r = retour[COL_ADMIN].ToString().Equals("True") ? 1 : 0;
            retour.Close();
            return r;
        }

        public static string[] PromoEleve(string promo)
        {
            var retour = new string[1000];
            var command = _conn.CreateCommand();
            var idClasse = string.Empty;
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idClasse = reader["idClasse"].ToString();
            }
            var id = idClasse;
            //MessageBox.Show(id);
            reader.Close();
            var command2 = _conn.CreateCommand();
            var i = 0;
            command2.CommandText = "SELECT Prenom,Nom FROM eleve WHERE idClasse ='" + id + "'";
            var reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                var concatenate = reader2["Prenom"] + " " + reader2["Nom"];
                retour[i] = concatenate;
                i++;
            }
            reader2.Close();
            return retour;
        }

        public static string[] RecupEleveAvecPromo(string promo)
        {
            var retour = new string[1000];
            var i = 0;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Nom, Prenom FROM `classe` NATURAL JOIN eleve WHERE Promotion = " + promo;
            var r = command.ExecuteReader();
            while (r.Read())
            {
                var concatenate = r["Prenom"] + " " + r["Nom"];
                //MessageBox.Show(concatenate);
                retour[i] = concatenate;
                i = i + 1;
            }
            r.Close();
            return retour;
        }

        public static int RestoreDatabase(string restoredFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        MessageBox.Show(
                            "Attention toutes modification de base de données est irréversible ! Le logiciel rédemarrera après importation.");
                        var fileData = new OpenFileDialog();
                        fileData.Title = "Importer une base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = "Fichier SQL (*.sql)|*.sql";
                        var result = fileData.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            restoredFile = fileData.FileName;
                        }
                        cmd.Connection = _conn;
                        try
                        {
                            mb.ImportFromFile(restoredFile);
                            return 0;
                        }
                        catch
                        {
                            MessageBox.Show("L'importation a échouée");
                            return 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     The simple request.
        /// </summary>
        /// <param name="r">
        ///     The request
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool SimpleRequest(this string r)
        {
            try
            {
                var neweleve = new MySqlCommand(r, _conn) {CommandText = r};
                neweleve.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}