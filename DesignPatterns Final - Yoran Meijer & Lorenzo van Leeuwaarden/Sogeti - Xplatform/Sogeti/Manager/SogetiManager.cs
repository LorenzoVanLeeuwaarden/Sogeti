using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Sogeti
{

    class SogetiManager
    {
        DatabaseConnection dc = new DatabaseConnection();
        List<int> vacatures = new List<int>();
        List<int> cvs = new List<int>();
        string currentVac;
        string currentCV;

        List<string> koppelGebruiker = new List<string>();
        List<string> koppelVacature = new List<string>();

        public string bedrijfsnaam;
        public string werkervaring;
        public string opleiding;
        public string baantype;
        public string carriereniveau;
        public string locatie;
        public string omschrijving;
        public string vacatureTitel;
        string bedrijfsID;

        public string cvTitel;
        public string persoonsID; 
        public string cvSpreektalen;
        public string cvWerkervaring;
        public string cvOpleiding;
        public string cvProgrammeertalen;
        public string cvTalenervaring;
        public string persoonskenmerken;

        public string pNaam;
        public string pGebDatum;
        public string pGeslacht;
        public string pAdres;
        public string pPostcode;
        public string pPlaats;
        public string pNationaliteit;
        public string pEmailadres;
        public string pTelefoon;

        public void getConnection() {
            dc.setUpConnection();
        }

        public List<int> getVacatures() {
            vacatures.Clear();
            getConnection();
            MySqlTransaction myTrans = dc.connection.BeginTransaction();
            string mySelectQuery = "SELECT vacatureID FROM vacature";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, dc.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    currentVac = GetDBString("vacatureID", myReader);
                    vacatures.Add(int.Parse(currentVac));
                }
            }
            finally
            {
                myReader.Close();
                dc.connection.Close();
            }

            return vacatures;
        }

        public List<int> getCVs()
        {
            cvs.Clear();
            getConnection();
            MySqlTransaction myTrans = dc.connection.BeginTransaction();
            string mySelectQuery = "SELECT cvID FROM cv";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, dc.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    currentCV = GetDBString("cvID", myReader);
                    cvs.Add(int.Parse(currentCV));
                }
            }
            finally
            {
                myReader.Close();
                dc.connection.Close();
            }

            return cvs;
        }

       

        public void retrieveVacatureData(string selectedID)
        {
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM vacature WHERE vacatureID='" + selectedID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    vacatureTitel = GetDBString("vacatureID", myReader);
                    bedrijfsID = GetDBString("bedrijfsID", myReader);
                    locatie = GetDBString("locatie", myReader);
                    werkervaring = GetDBString("werkervaring", myReader);
                    opleiding = GetDBString("opleiding", myReader);
                    baantype = GetDBString("baantype", myReader);
                    carriereniveau = GetDBString("carriereniveau", myReader);
                    omschrijving = GetDBString("omschrijving", myReader);

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
            
            db.setUpConnection();
            myTrans = db.connection.BeginTransaction();
            mySelectQuery = "SELECT bedrijfsnaam FROM bedrijf WHERE bedrijfsID='" + bedrijfsID + "'";
            cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    bedrijfsnaam = GetDBString("bedrijfsnaam", myReader);

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
        }

        public void retrieveCVData(string selectedID)
        {
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM cv WHERE cvID='"+selectedID+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    cvTitel = GetDBString("cvID", myReader);
                    persoonsID = GetDBString("persoonsID", myReader);
                    cvSpreektalen = GetDBString("spreektalen", myReader);
                    cvWerkervaring = GetDBString("werkervaring", myReader);
                    cvOpleiding = GetDBString("opleidingen", myReader);
                    cvProgrammeertalen = GetDBString("programmeertalen", myReader);
                    cvTalenervaring = GetDBString("talenervaring", myReader);
                    persoonskenmerken = GetDBString("persoonskenmerken", myReader);

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }

            db.setUpConnection();
            myTrans = db.connection.BeginTransaction();
            mySelectQuery = "SELECT * FROM persoon WHERE persoonsID='" + persoonsID + "'";
            cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);

            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    pNaam = GetDBString("naam", myReader);
                    pGebDatum = GetDBString("geb_datum", myReader);
                    pGeslacht = GetDBString("geslacht", myReader);
                    pAdres = GetDBString("adres", myReader);
                    pPostcode = GetDBString("postcode", myReader);
                    pPlaats = GetDBString("plaats", myReader);
                    pNationaliteit = GetDBString("nationaliteit", myReader);
                    pEmailadres = GetDBString("emailadres", myReader);
                    pTelefoon = GetDBString("telefoon", myReader);

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
        }

        public void koppelGebruikerVacature(string vacatureID, string gebruikersID) {
            List<string> opgehaaldeVacatureIDs = new List<string>();
            List<string> opgehaaldeGebruikerIDs = new List<string>();
            string currentVacID;
            string currentGebID;
            Boolean alGereageerd = false;
            

            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT gebruikersID, vacatureID FROM koppeling WHERE vacatureID ='" + vacatureID + "' AND gebruikersID = '"+gebruikersID+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    opgehaaldeVacatureIDs.Add(GetDBString("vacatureID", myReader));
                    opgehaaldeGebruikerIDs.Add(GetDBString("gebruikersID", myReader));

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }

            for (int i = 0; i < opgehaaldeGebruikerIDs.Count(); i++) {
                currentVacID = opgehaaldeVacatureIDs[i];
                currentGebID = opgehaaldeGebruikerIDs[i];
                if (vacatureID.Equals(currentVacID) && gebruikersID.Equals(currentGebID)){
                    MessageBox.Show("U heeft al gereageerd!");
                    alGereageerd = true;
                    break;
                }
            }

            if (alGereageerd == false) {
                dc.setUpConnection();
                MySqlCommand cmd2 = dc.connection.CreateCommand();
                cmd2.CommandText = "INSERT INTO koppeling (gebruikersID, vacatureID) VALUES('" + gebruikersID + "', '" + vacatureID + "')";
                cmd2.ExecuteNonQuery();
                dc.connection.Close();
            }
        }

        public void getKoppels() {
            getConnection();
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT gebruikersID, vacatureID FROM koppeling";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                   koppelVacature.Add(GetDBString("vacatureID", myReader));
                   koppelGebruiker.Add(GetDBString("gebruikersID", myReader));

                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
        }

        public void updateInfo()
        {
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM cv WHERE persoonsID='" + persoonsID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            if (myReader.HasRows == false)
            {

                db.setUpConnection();
                MySqlCommand cmdInsert = db.connection.CreateCommand();
                cmdInsert.CommandText = "INSERT INTO cv(cvID,persoonsID,spreektalen,werkervaring,opleidingen,programmeertalen,talenervaring,persoonskenmerken) VALUES('" + cvTitel + "','" + persoonsID + "','" + cvSpreektalen + "','" + cvWerkervaring + "','" + cvOpleiding + "','" + cvProgrammeertalen + "','" + cvTalenervaring + "','" + persoonskenmerken + "')";
                cmdInsert.ExecuteNonQuery();

            }

            else try
                {

                    db.setUpConnection();
                    MySqlCommand cmdWijzigCv = db.connection.CreateCommand();
                    cmdWijzigCv.CommandText = "UPDATE cv SET spreektalen='" + cvSpreektalen + "', werkervaring='" + cvWerkervaring + "', opleidingen='" + cvOpleiding + "', programmeertalen='" + cvProgrammeertalen + "', talenervaring='" + cvTalenervaring + "', persoonskenmerken='" + persoonskenmerken + "' WHERE cvID='" + cvTitel + "'";
                    cmdWijzigCv.ExecuteNonQuery();

                    db.setUpConnection();
                    MySqlCommand cmdWijzigPersoon = db.connection.CreateCommand();
                    cmdWijzigPersoon.CommandText = "UPDATE persoon SET naam='" + pNaam + "', geslacht='" + pGeslacht + "', geb_datum='" + pGebDatum + "', adres='" + pAdres + "', postcode='" + pPostcode + "', plaats='" + pPlaats + "', nationaliteit='" + pNationaliteit + "', emailadres='" + pEmailadres + "', telefoon='" + pTelefoon + "' WHERE persoonsID='" + persoonsID + "'";
                    cmdWijzigPersoon.ExecuteNonQuery();

                }


                finally
                {
                    myReader.Close();
                    db.connection.Close();
                }


        }

        public void deleteKoppel(string gebID, string vacID) {
            dc.setUpConnection();
            MySqlCommand cmd = dc.connection.CreateCommand();
            cmd.CommandText = "DELETE FROM koppeling WHERE gebruikersID = '"+gebID+"' AND vacatureID = '" + vacID + "'";
            cmd.ExecuteNonQuery();
            dc.connection.Close();
        }

        public string getPersoonsID(string gebruikersID) {
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT persoonsID FROM persoon WHERE gebruikersID = '"+gebruikersID+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            string pID = "";
            try
            {
                while (myReader.Read())
                {
                    pID = GetDBString("persoonsID", myReader);
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
            return pID;
        }

        public string getCVID(string pID)
        {
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT cvID FROM cv WHERE persoonsID = '" + pID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            string cirvitID = "";
            try
            {
                while (myReader.Read())
                {
                    cirvitID = GetDBString("cvID", myReader);
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
            return cirvitID;
        }

        public List<string> getKoppelVacatures()
        {
            return koppelVacature;
        }
        public List<string> getKoppelGebruikers()
        {
            return koppelGebruiker;
        }


        //Haal value uit databasecolumn van gespecificeerde row
        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }
    }
}
