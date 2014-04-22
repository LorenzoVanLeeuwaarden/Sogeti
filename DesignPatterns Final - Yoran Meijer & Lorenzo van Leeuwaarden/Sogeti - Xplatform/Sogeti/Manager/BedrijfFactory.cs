using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class BedrijfFactory : iDataFactory
    {
        public String bedrijfsID;
        public String gebruikersID;
        public String bedrijfsnaam;
        public String bedrijfseigenschappen;
        public String plaats;
        public String adres;
        public String postcode;
        public String emailadres;
        public String telefoon;

        public BedrijfFactory(String newID) {
            gebruikersID = newID;
        } 

        public void retrieveData()
        {
            //Haal gegevens uit de database op basis van het gegeven gebruikersID
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM bedrijf WHERE gebruikersID='" + gebruikersID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    bedrijfsID = GetDBString("bedrijfsID", myReader);
                    bedrijfsnaam = GetDBString("bedrijfsnaam", myReader);
                    bedrijfseigenschappen = GetDBString("bedrijfseigenschappen", myReader);
                    adres = GetDBString("adres", myReader);
                    postcode = GetDBString("postcode", myReader);
                    plaats = GetDBString("plaats", myReader);
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

        public iData setData()
        {
            //Maak een nieuw object aan en vul deze met de opgehaalde informatie
            Bedrijf bedrijf = new Bedrijf();
            bedrijf.adres = adres;
            bedrijf.bedrijfseigenschappen = bedrijfseigenschappen;
            bedrijf.bedrijfsID = bedrijfsID;
            bedrijf.bedrijfsnaam = bedrijfsnaam;
            bedrijf.emailadres = emailadres;
            bedrijf.gebruikersID = gebruikersID;
            bedrijf.plaats = plaats;
            bedrijf.postcode = postcode;
            bedrijf.telefoon = telefoon;
            return bedrijf;
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }

    }
}
