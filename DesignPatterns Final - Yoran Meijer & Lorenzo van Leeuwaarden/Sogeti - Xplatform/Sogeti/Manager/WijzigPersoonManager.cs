using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class WijzigPersoonManager
    {
        Persoon persoon;
        CV cv;
        public WijzigPersoonManager(Persoon newPersoon, CV newCV) {
            persoon = newPersoon;
            cv = newCV;
            parseGegevens();
        }

        public void updateInfo() { 

            //Check of persoon al een cv in de database heeft staan

            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM cv WHERE persoonsID='"+persoon.persoonsID+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            if (myReader.HasRows == false) {

                db.setUpConnection();
                MySqlCommand cmdInsert = db.connection.CreateCommand();
                cmdInsert.CommandText = "INSERT INTO cv(cvID,persoonsID,spreektalen,werkervaring,opleidingen,programmeertalen,talenervaring,persoonskenmerken) VALUES('"+cv.cvID+"','"+cv.persoonsID+"','"+cv.spreektalen+"','"+cv.werkervaring+"','"+cv.opleidingen+"','"+cv.programmeertalen+"','"+cv.talenervaring+"','"+cv.persoonskenmerken+"')";
                cmdInsert.ExecuteNonQuery();
                
            }

            else try{

                //Invoeren of updaten van cv

                db.setUpConnection();
                MySqlCommand cmdWijzigCv = db.connection.CreateCommand();
                cmdWijzigCv.CommandText = "UPDATE cv SET spreektalen='" + cv.spreektalen + "', werkervaring='" + cv.werkervaring + "', opleidingen='" + cv.opleidingen + "', programmeertalen='" + cv.programmeertalen + "', talenervaring='" + cv.talenervaring + "', persoonskenmerken='" + cv.persoonskenmerken + "' WHERE cvID='"+cv.cvID+"'";
                cmdWijzigCv.ExecuteNonQuery();

                db.setUpConnection();
                MySqlCommand cmdWijzigPersoon = db.connection.CreateCommand();
                cmdWijzigPersoon.CommandText = "UPDATE persoon SET naam='" + persoon.naam + "', geslacht='" + persoon.geslacht + "', geb_datum='" + persoon.date + "', adres='" + persoon.adres + "', postcode='" + persoon.postcode + "', plaats='" + persoon.plaats + "', nationaliteit='" + persoon.nationaliteit + "', emailadres='" + persoon.emailadres + "', telefoon='" + persoon.telefoon + "' WHERE persoonsID='" + persoon.persoonsID + "'";
                cmdWijzigPersoon.ExecuteNonQuery();


            
            
            }
            
           
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
            
        
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }

        private void parseGegevens() { 
            //Verwijder alle onnodige characters

            persoon.adres = persoon.adres.Replace("'", "");
            persoon.adres = persoon.adres.Replace('"', ' ');
            persoon.emailadres = persoon.emailadres.Replace("'", "");
            persoon.emailadres = persoon.emailadres.Replace('"', ' ');
            persoon.geslacht = persoon.geslacht.Replace("'", "");
            persoon.geslacht = persoon.geslacht.Replace('"', ' ');
            persoon.naam = persoon.naam.Replace("'", "");
            persoon.naam = persoon.naam.Replace('"', ' ');
            persoon.nationaliteit = persoon.nationaliteit.Replace("'", "");
            persoon.nationaliteit = persoon.nationaliteit.Replace('"', ' ');
            persoon.plaats = persoon.plaats.Replace("'", "");
            persoon.plaats = persoon.plaats.Replace('"', ' ');
            persoon.postcode = persoon.postcode.Replace("'", "");
            persoon.postcode = persoon.postcode.Replace('"', ' ');
            persoon.telefoon = persoon.telefoon.Replace("'", "");
            persoon.telefoon = persoon.telefoon.Replace('"', ' ');

            cv.opleidingen = cv.opleidingen.Replace("'", "");
            cv.opleidingen = cv.opleidingen.Replace('"', ' ');
            cv.persoonskenmerken = cv.persoonskenmerken.Replace("'", "");
            cv.persoonskenmerken = cv.persoonskenmerken.Replace('"', ' ');
            cv.programmeertalen = cv.programmeertalen.Replace("'", "");
            cv.programmeertalen = cv.programmeertalen.Replace('"', ' ');
            cv.spreektalen = cv.spreektalen.Replace("'", "");
            cv.spreektalen = cv.spreektalen.Replace('"', ' ');
            cv.werkervaring = cv.werkervaring.Replace("'", "");
            cv.werkervaring = cv.werkervaring.Replace('"', ' ');




        }
    }
}
