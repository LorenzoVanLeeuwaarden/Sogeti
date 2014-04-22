using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class PersoonFactory : iDataFactory
    {
        public String persoonsID;
        public String gebruikersID;
        public String naam;
        public String date;
        public String geslacht;
        public String postcode;
        public String plaats;
        public String adres;
        public String nationaliteit;
        public String emailadres;
        public String telefoon;



        public PersoonFactory(string newID) {
            gebruikersID = newID;
        }
        
        void iDataFactory.retrieveData() {

            //Haal informatie op aan de hand van het opgegeven gebruikersID
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM persoon WHERE gebruikersID='"+gebruikersID+"'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    persoonsID = GetDBString("persoonsID", myReader);
                    naam = GetDBString("naam", myReader);
                    date = GetDBString("geb_datum", myReader);
                    geslacht = GetDBString("geslacht", myReader);
                    adres = GetDBString("adres", myReader);
                    postcode = GetDBString("postcode", myReader);
                    plaats = GetDBString("plaats", myReader);
                    nationaliteit = GetDBString("nationaliteit", myReader);
                    emailadres = GetDBString("emailadres", myReader);
                    telefoon = GetDBString("telefoon", myReader);
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
            
        }

        iData iDataFactory.setData() {
            //Maak een nieuw object en vul deze met opgehaalde informatie
            Persoon persoon = new Persoon();
            persoon.gebruikersID = gebruikersID;
            persoon.persoonsID = persoonsID;
            persoon.naam = naam;
            persoon.date = date;
            persoon.geslacht = geslacht;
            persoon.adres = adres;
            persoon.postcode = postcode;
            persoon.plaats = plaats;
            persoon.nationaliteit = nationaliteit;
            persoon.emailadres = emailadres;
            persoon.telefoon = telefoon;
            return persoon;
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }
    }
}
