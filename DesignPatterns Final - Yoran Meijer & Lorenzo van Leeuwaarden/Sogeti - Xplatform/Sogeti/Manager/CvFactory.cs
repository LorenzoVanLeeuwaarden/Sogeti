using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class CvFactory : iDataFactory
    {
        public String cvID;
        public String persoonsID;
        public String gebruikersID;
        public String spreektalen;
        public String werkervaring;
        public String opleidingen;
        public String programmeertalen;
        public String talenervaring;
        public String persoonskenmerken;

        public CvFactory(String newID) {
            persoonsID = newID;
        }

        public void retrieveData()
        {
            //Haal informatie op aan de hand van het opgegeven persoonsID
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM cv WHERE persoonsID='" + persoonsID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    cvID = GetDBString("cvID", myReader);
                    spreektalen = GetDBString("spreektalen", myReader);
                    werkervaring = GetDBString("werkervaring", myReader);
                    opleidingen = GetDBString("opleidingen", myReader);
                    programmeertalen = GetDBString("programmeertalen", myReader);
                    talenervaring = GetDBString("talenervaring", myReader);
                    persoonskenmerken = GetDBString("persoonskenmerken", myReader);
                    
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
            //Maak een nieuw object en vul deze met de opgehaalde informatie
            CV cv = new CV();
            cv.cvID = cvID;
            cv.gebruikersID = gebruikersID;
            cv.opleidingen = opleidingen;
            cv.persoonsID = persoonsID;
            cv.persoonskenmerken = persoonskenmerken;
            cv.programmeertalen = programmeertalen;
            cv.spreektalen = spreektalen;
            cv.talenervaring = talenervaring;
            cv.werkervaring = werkervaring;
            return cv;
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }

    }
}
