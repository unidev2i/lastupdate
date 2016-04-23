using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication2
{
    public static class Database
    {
        private static MySqlConnection _conn;

        private const string COL_LOGIN = "Login";
        private const string COL_PASS = "Password";
        private const string COL_ADMIN = "Admin";
        private const string COL_IDSKILL = "idCompetence";
        private const string COL_IDELEVE = "idEleve";
        private const string COL_NOTE = "note";
        private const string COL_MAXNOTE = "maxNote";
        private const string TAB_ELEVE = "eleve";
        private const string TAB_TP = "tp";

        private const string TAB_USER = "user";

        public static string Server { get; private set; }

        public static string Username { get; private set; }

        public static string Password { get; private set; }

        public static string DatabaseName { get; private set; }

        public static void Connect(string ip = "localhost", string login = "root", string pass = "", string database = "mydb")
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

        public static bool SimpleRequest(this string r)
        {
            try
            {
                var neweleve = new MySqlCommand(r, _conn) { CommandText = r };
                neweleve.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int Login(string login, string pass)
        {
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_LOGIN + "," + COL_PASS + "," + COL_ADMIN + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + login + "' AND " + COL_PASS + "='" + pass + "'";
            var retour = command.ExecuteReader();

            if (!retour.Read())
            {//JE RAJOUTE UN COMMENTAIRE
                retour.Close();
                return -1;
            }

            var r = retour[COL_ADMIN].ToString().Equals("True") ? 1 : 0;
            retour.Close();
            return r;
        }

        public static IEnumerable<string> GetListRequest(string table, string[] columns, string additional_where_clause = "1")
        {
            var retour = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT * FROM " + table + " WHERE " + additional_where_clause;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                var toReturn = columns.Aggregate(string.Empty, (current, c) => current + r[c].ToString() + " ");
                retour.Add(toReturn);
            }
            r.Close();
            return retour;
            //COMMENTAIRE
        }

        public static void DelRequest(string table, Tuple<string, string>[] where_clause)
        {
            var request = "DELETE FROM " + table + " WHERE ";

            var secondLine = false;
            foreach (var a in where_clause)
            {
                if (secondLine)
                    request += " AND ";
                request += a.Item1 + "='" + a.Item2 + "'";
                secondLine = true;
            }

            var command = _conn.CreateCommand();
            command.CommandText = request;
            command.ExecuteNonQuery();
        }

        public static List<Tuple<string, float, int>> GetWtfRequest(string idEleve = "2")
        {
            var req =
                "SELECT " + COL_IDSKILL + ",COUNT(" + COL_NOTE + ") AS quantite, ((SUM(" + COL_NOTE + "/" + COL_MAXNOTE + ")/COUNT(" + COL_NOTE + "))*100) AS moyenne FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP + " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = " + idEleve + " GROUP BY " + COL_IDSKILL;
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            List<Tuple<string, float, int>> a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[COL_IDSKILL].ToString(), float.Parse(r["moyenne"].ToString()), int.Parse(r["quantite"].ToString())));
            }

            r.Close();
            return a;
        }

        public static string ChangerLogin(string login, string pass, string ancienlog)
        {
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + ancienlog + "'";

            var pass2 = "";
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString();
            }
            retour.Close();

            if (pass == pass2 && login != "")
            {
                var Request = "UPDATE " + TAB_USER + " SET " + COL_LOGIN + "='" + login + "' WHERE " + COL_LOGIN + "='" + ancienlog + "'";
                var command2 = _conn.CreateCommand();
                command2.CommandText = Request;
                command2.ExecuteNonQuery();

                MessageBox.Show("Changement réussi");
                return login;
            }

            else
            {
                MessageBox.Show("Mot de passe actuel incorrect");
                return "";
            }

        }

        public static int ChangerMdp(string login, string pass, string ancienmdp)
        {
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + login + "'";

            var pass2 = "";
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString(); // mot de passe récuperé de la BDD
            }
            retour.Close();

            if (ancienmdp == pass2 && pass != "") // on compare le mdp récuperé avec celui tapé dans le champ "ancien mot de passe"
            {                                     // on vérifie que le nouveau mdp ne soit pas vide
                var Request = "UPDATE " + TAB_USER + " SET " + COL_PASS + "='" + pass + "' WHERE " + COL_LOGIN + "='" + login + "' AND " + COL_PASS + "='" + ancienmdp + "'";
                var command2 = _conn.CreateCommand();
                command2.CommandText = Request;
                command2.ExecuteNonQuery();

                MessageBox.Show("Changement réussi");
                return 1;
            }

            else
            {
                MessageBox.Show("Ancien mot de passe incorrect");
                return 0;
            }

        }

        public static List<Tuple<string, float>> GetWebRequest(string idEleve = "2")
        {
            var retour = new List<Tuple<string, float>>();
            var req =
                "SELECT " + COL_IDSKILL + ", SUM(" + COL_NOTE + ") FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP + " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = '" + idEleve + "' GROUP BY " + COL_IDSKILL;
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[COL_IDSKILL].ToString(), float.Parse(r["SUM(" + COL_NOTE + ")"].ToString())));
            }

            r.Close();
            return retour;
        }

        public static string getpromo(string idClasse)
        {
            string promo = "";
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Promotion FROM classe WHERE idClasse ='" + idClasse + "'";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                promo = reader["Promotion"].ToString();
            }
            var promo2 = promo;
            //MessageBox.Show(id);
            reader.Close();

            return promo2;
        }

        public static void BackupDatabase(string backUpFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        SaveFileDialog fileData = new SaveFileDialog();
                        fileData.Title = "Exporter la base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = "Fichier SQL (*.sql)|*.sql";
                        DialogResult result = fileData.ShowDialog();
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
        public static int RestoreDatabase(string restoredFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        MessageBox.Show("Attention toutes modification de base de données est irréversible ! Le logiciel rédemarrera après importation.");
                        OpenFileDialog fileData = new OpenFileDialog();
                        fileData.Title = "Importer une base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = "Fichier SQL (*.sql)|*.sql";
                        DialogResult result = fileData.ShowDialog();
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

        public static string[] RecupEleveAvecPromo(string promo)
        {
            string[] retour = new String[1000];
            var i = 0;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Nom, Prenom FROM `classe` NATURAL JOIN eleve WHERE Promotion = " + promo;
            var r = command.ExecuteReader();
            while (r.Read())
            {
                var concatenate = r["Prenom"].ToString() + " " + r["Nom"].ToString();
                //MessageBox.Show(concatenate);
                retour[i] = concatenate;
                i = i + 1;
            }
            r.Close();
            return retour;
        }

        public static string[] EcritureInteligente(string concatenate)
        {
            string[] retour = new String[1000];
            var i = 0;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Prenom,Nom FROM eleve";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var concatenate2 = reader["Prenom"].ToString() + " " + reader["Nom"].ToString();
                if (concatenate2.Contains(concatenate)){
                    retour[i] = concatenate2;
                    i++;
                }
            }
            reader.Close();
            return retour;
        }

        public static int AddUser(string login, string mdp, int statut)
        {
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "INSERT INTO " + TAB_USER + "(" + COL_LOGIN + "," + COL_PASS + "," + COL_ADMIN + ") VALUES ('" + login + "','" + mdp + "','" + statut + "')";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si erreur il y a
            {
                retour.Close();
                return 1;
            }
            retour.Close();
            return 1;
        }

        public static int Delete(string user)
        {
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + user+"'";
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
            string[] eleve2 = eleve.Split(' ');
            MessageBox.Show(eleve2[0] + "   " + eleve2[1]);
            MySqlCommand command = _conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TAB_ELEVE + " WHERE Nom ='" + eleve2[0] + "'AND Prenom='" + eleve2[1] + "'";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si Erreur il y a
            {
                retour.Close();
                return -1;
            }
            retour.Close();
            return 1;
        }

        public static string[] PromoEleve(string promo)
        {
            string[] retour = new String[1000];
            var command = _conn.CreateCommand();
            var idClasse="";
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='"+promo+"'";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                idClasse = reader["idClasse"].ToString();
            }
            var id = idClasse;
            //MessageBox.Show(id);
            reader.Close();
            var command2 = _conn.CreateCommand();
            var i = 0;
            command2.CommandText= "SELECT Prenom,Nom FROM eleve WHERE idClasse ='" + id + "'";
            MySqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                var concatenate = reader2["Prenom"].ToString() + " " + reader2["Nom"].ToString();
                retour[i] = concatenate;
                i++;
            }
            reader2.Close();
            return retour;
       }
        public static string getidClasse(string promo)
        {
            string idclasse = "";
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                idclasse = reader["idClasse"].ToString();
            }
            var id = idclasse;
            //MessageBox.Show(id);
            reader.Close();
            var Location = 1;
            if (id == "")
            {
                ("INSERT INTO classe (Promotion,Location) VALUES ('" + promo + "','" + Location + "')").SimpleRequest();

                var command2 = _conn.CreateCommand();
                command2.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
                MySqlDataReader reader2 = command2.ExecuteReader();
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
    }
}
