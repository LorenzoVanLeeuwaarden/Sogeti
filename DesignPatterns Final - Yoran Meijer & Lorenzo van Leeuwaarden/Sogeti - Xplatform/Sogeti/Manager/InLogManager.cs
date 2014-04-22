using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Sogeti
{
    class InLogManager // De Manager die al het verkeer voor inloggen en registreren afhandelt.
    {
        DatabaseConnection db = new DatabaseConnection();                    //leg connectie met DatabaseConnector   
        List<string> gebruikersnamen = new List<string>();              //List met gebruikersnamen (login checken)
        List<string> wachtwoorden = new List<string>();                 //List met wachtwoorden (login checken)
        List<string> namen = new List<string>();                        //List met namen (checken of persoon die wil registreren al met zijn naam in database staat)
        List<string> ids = new List<string>();
        inlogGUI inloggui;
        RegistratieGUI regGui;

        Boolean exisitingName;
        Boolean existingID;

        public InLogManager(){
    
        }

        public void getinlogGUI(inlogGUI gui) {
            inloggui = gui;
        }

        public void getRegistratieGUI(RegistratieGUI registratieGUI) {
            regGui = registratieGUI;
        }

        public void getConnection() {   //Krijg de connectie van DatabaseConnection
            db.setUpConnection();
        }

        public void getAllUsernamePasswords()   //Krijg alle usernames en passwords voor vergelijkingen
        {
            
                MySqlTransaction myTrans = db.connection.BeginTransaction();
                string mySelectQuery = "SELECT gebruikersnaam, wachtwoord FROM login";
                MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
                MySqlDataReader myReader;
                myReader = cmd.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        gebruikersnamen.Add(GetDBString("gebruikersnaam", myReader));
                        wachtwoorden.Add(GetDBString("wachtwoord", myReader));
                    }
                }
                finally
                {
                    myReader.Close();
                    db.connection.Close();
                }
            
        }

        public Boolean compareStrings(string username, string password)        //Kijk of de gebruiker die in wil loggen de juiste data invoert.
        {
            string currentNaam = "";
            string currentPass = "";
            string gebruikersnaam = username;
            string wachtwoord = password;
            Boolean a = true;

            for (int i = 0; i < gebruikersnamen.Count; i++)
            {
                
                currentNaam = gebruikersnamen[i];
                currentPass = wachtwoorden[i];
                if (gebruikersnaam.Equals(currentNaam) && wachtwoord.Equals(currentPass))
                {
                    logOn(username);
                    break;
                }
                else
                {
                    if (i == gebruikersnamen.Count-1) {
                        a = false;
                    }
                }
            }

            if (a == false)
            {
                return a;
            }
            else {
                return true;
            }

        }

        public Boolean checkUsername(string username) {         //Kijk of username al in database staat voor registreren
            Boolean foundusername = false;
            for (int i = 0; i < gebruikersnamen.Count; i++)
            {
                string currentNaam = gebruikersnamen[i];
                if (username.Equals(currentNaam))
                {
                    foundusername = true;
                    break;
                }
                else {
                    foundusername = false;
                }
            }

            if (foundusername == true) { return true; }
            else return false;
        }

        //Registreer fase 1
        public void register(string username, string password, string naam, string city, string adres, string zipcode, string email, string telefoon, bool isPerson ) { 
            exisitingName = false;
            if (checkExistingName(naam, isPerson) == true) {
                exisitingName = true;
            }
            existingID = false;
            if (checkExistingID(naam, isPerson) == true) {
                existingID = true;
            }
            registerUsernamePassword(username, password, isPerson);
            registerPersonalInfo(username, naam, city, adres, zipcode, email, telefoon, isPerson, exisitingName);

            
        }

        //Registreer de Username en Password met bijbehorende gebruikersID
        public void registerUsernamePassword(string username, string password, bool isPerson)
        {
            getConnection();
            MySqlCommand cmd = db.connection.CreateCommand();
            username = username.Replace("'", "");
            username = username.Replace('"', ']');
            username = username.Replace("]", "");
            password = password.Replace("'", "");
            password = password.Replace('"', ']');
            password = password.Replace("]", "");
            cmd.CommandText = "INSERT INTO login (gebruikersnaam, wachtwoord) VALUES('" + username + "', '" + password + "')";
            cmd.ExecuteNonQuery();
            
            string category = "";
            if (isPerson == true) {
                category = "Person";    
            }else {
                category = "Company";
            }

            cmd.CommandText = "INSERT INTO gebruiker (gebruikersnaam, category) VALUES('" + username + "', '" + category + "')";
            cmd.ExecuteNonQuery();
            db.connection.Close();


        }

        //Registreer de persoonlijke informtie van de gebruiker met bijbehorende gebruikersID
        public void registerPersonalInfo(string username, string naam, string stad, string adres, string postcode, string email, string telefoon, bool isPerson, bool existingName)
        {
            string gebruikersID;

            username = username.Replace("'", "");
            username = username.Replace('"', ']');
            username = username.Replace("]", "");
            naam = naam.Replace("'", "");
            naam = naam.Replace('"', ' ');
            stad = stad.Replace("'", "");
            stad = stad.Replace('"', ' ');
            adres = adres.Replace("'", "");
            adres = adres.Replace('"', ' ');
            postcode = postcode.Replace("'", "");
            postcode = postcode.Replace('"', ' ');
            email = email.Replace("'", "");
            email = email.Replace('"', ' ');
            telefoon = telefoon.Replace("'", "");
            telefoon = telefoon.Replace('"', ' ');

            getConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT gebruikersID FROM gebruiker WHERE gebruikersnaam = '"+username+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            myReader.Read();
            gebruikersID = myReader.GetString(0);
            myReader.Close();
            db.connection.Close();


            getConnection();
            MySqlCommand cmd2 = db.connection.CreateCommand();
            if (isPerson == true)
            {
                if (existingName == true)
                {
                    if (existingID == true)
                    {
                        cmd2.CommandText = "UPDATE persoon SET gebruikersID = '" + gebruikersID + "', plaats = '" + stad + "', adres = '" + adres + "', postcode = '" + postcode + "', emailadres = '" + email + "', telefoon = '" + telefoon + "' WHERE naam = '" + naam + "' ";
                        cmd2.ExecuteNonQuery();
                        regGui.Close();
                        inloggui.Visible = true;
                    }
                    else if (existingID == false) {
                        cmd2.CommandText = "INSERT INTO persoon (gebruikersID, naam, adres, postcode, plaats, emailadres, telefoon) VALUES('" + gebruikersID + "', '" + naam + "', '" + adres + "', '" + postcode + "','" + stad + "','" + email + "','" + telefoon + "')";
                        cmd2.ExecuteNonQuery();
                        regGui.Close();
                        inloggui.Visible = true;
                    }
                }
                else if(existingName == false) {
                    cmd2.CommandText = "INSERT INTO persoon (gebruikersID, naam, adres, postcode, plaats, emailadres, telefoon) VALUES('" + gebruikersID + "', '" + naam + "', '" + adres + "', '"+postcode+"','"+stad+"','"+email+"','"+telefoon+"')";
                    cmd2.ExecuteNonQuery();
                    regGui.Close();
                    inloggui.Visible = true;
                }
                
            }
            else {
                if (existingName == true) {
                    if (existingID == true)
                    {
                        cmd2.CommandText = "UPDATE bedrijf SET gebruikersID = '" + gebruikersID + "', plaats = '" + stad + "', adres = '" + adres + "', postcode = '" + postcode + "', emailadres = '" + email + "', telefoon = '" + telefoon + "' WHERE bedrijfsnaam = '" + naam + "' ";
                        cmd2.ExecuteNonQuery();
                        regGui.Close();
                        inloggui.Visible = true;
                    }
                    else if (existingID == false){
                        cmd2.CommandText = "INSERT INTO bedrijf (gebruikersID, bedrijfsnaam, adres, postcode, plaats, emailadres, telefoon) VALUES('" + gebruikersID + "', '" + naam + "', '" + adres + "', '" + postcode + "','" + stad + "','" + email + "','" + telefoon + "')";
                        cmd2.ExecuteNonQuery();
                        regGui.Close();
                        inloggui.Visible = true;
                    }
                }
                else if (existingName==false){
                    cmd2.CommandText = "INSERT INTO bedrijf (gebruikersID, bedrijfsnaam, adres, postcode, plaats, emailadres, telefoon) VALUES('" + gebruikersID + "', '" + naam + "', '" + adres + "', '" + postcode + "','" + stad + "','" + email + "','" + telefoon + "')";
                    cmd2.ExecuteNonQuery();
                    regGui.Close();
                    inloggui.Visible = true;
                }
            }
           db.connection.Close();
           MessageBox.Show("Your account has been registered!");
        }

        //Kijk of gebruiker al met deze naam in de database staat
        public Boolean checkExistingID(string naam, Boolean isPerson)
        {
            naam = naam.Replace("'", "");
            naam = naam.Replace('"', ' ');
            
            getConnection();
            string mySelectQuery = "";
            Boolean foundID = false;

            MySqlTransaction myTrans = db.connection.BeginTransaction();
            if (isPerson == true)
            {
                mySelectQuery = "SELECT gebruikersID FROM persoon WHERE naam = '"+naam+"'";
            }
            else
            {
                mySelectQuery = "SELECT gebruikersID FROM bedrijf WHERE bedrijfsnaam = '"+naam+"'";
            }
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                
                while (myReader.Read())
                {
                    ids.Add(GetDBString("gebruikersID", myReader));
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }

            for (int i = 0; i < ids.Count; i++) {
                string id = ids[i];
                if (id != null) {
                    foundID = true;
                }
            }

            if (foundID == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Kijk of gebruiker al met deze naam in de database staat
        public Boolean checkExistingName(string naam, Boolean isPerson) {

            naam = naam.Replace("'", "");
            naam = naam.Replace('"', ' ');

            string mySelectQuery = "";
            Boolean foundname = false;
            getConnection();

            MySqlTransaction myTrans = db.connection.BeginTransaction();
            if (isPerson == true)
            {
                mySelectQuery = "SELECT naam FROM persoon";
            }
            else {
                mySelectQuery = "SELECT bedrijfsNaam FROM bedrijf";
            }
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    namen.Add(myReader.GetString(0));
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }

            for (int i = 0; i < namen.Count; i++)
            {
                string currentNaam = namen[i];
                if (naam.Equals(currentNaam)) {
                    foundname = true;
                }
            }

            if (foundname == true)
            {
                return true;
            }
            else {
                return false;
            }

        }

        public void logOn(string username)
        {
            username = username.Replace("'", "");
            username = username.Replace('"', ']');
            username = username.Replace("]", "");

            string category = "";
            string gebruikersID = "";

            getConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT category,gebruikersID FROM gebruiker WHERE gebruikersnaam = '" + username + "' ";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    category = GetDBString("category", myReader);
                    gebruikersID = GetDBString("gebruikersID", myReader);
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }

            switch (category)
            {
                case "Company":
                    inloggui.Visible = false;
                    BedrijfsGUI bedrijfsGUI = new BedrijfsGUI(gebruikersID);
                    bedrijfsGUI.ShowDialog();
                    break;
                case "Person":
                    inloggui.Visible = false;
                    WerkzoekendeGUI werkzoekendeGUI = new WerkzoekendeGUI(gebruikersID);
                    werkzoekendeGUI.ShowDialog();
                    break;
                case "admin":
                    inloggui.Visible = false;
                    SogetiGUI sogetiGUI = new SogetiGUI();
                    sogetiGUI.ShowDialog();
                    break;
            }
        }

        //Haal value uit databasecolumn van gespecificeerde row
        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }
            
    }
}
