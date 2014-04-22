using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class AlgoritmeManager
    {
        
        ProgrammeertalenParser parse = new ProgrammeertalenParser();
        List<String> VacatureList = new List<String>();
        List<String> OmschrijvingList = new List<String>();
        List<double> matchGetallenList = new List<double>();
        
        List<string> distinctList;

        public void matchPersoon(Persoon persoon, CV cv)
        { 
            //Haal skills op en plaats ze in een list

            String talen = cv.programmeertalen;
            String ervaring = cv.talenervaring;
            int aantal = 0;
            List<string> talenList = new List<string>();
            List<string> ervList = new List<string>();


            if (talen != null)
            {
                talenList = talen.Split(',').ToList();
                ervList = ervaring.Split(',').ToList();

                //Verwijder onnodige karakters

                String Ptalen = parse.toDisplay(talenList, ervList);

                talen = parse.TalenToDB(Ptalen);
                ervaring = parse.TaalervaringToDB(Ptalen);

                talenList = talen.Split(',').ToList();
                ervList = ervaring.Split(',').ToList();

                for (int a = 0; a < ervList.Count(); a++)
                {
                    ervList[a] = ervList[a].Replace(".", ",");
                }
            }
            
            //Check database op skills en plaats "gematchte" vacatureID's in een list

            DatabaseConnection db = new DatabaseConnection();


            for (int i = 0; i < talenList.Count(); i++) {
                db.setUpConnection();
                MySqlTransaction myTrans = db.connection.BeginTransaction();
                string mySelectQuery = "SELECT vacatureID FROM vacature WHERE omschrijving LIKE '%"+talenList[i]+"%'";
                MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
                MySqlDataReader myReader;
                myReader = cmd.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                            VacatureList.Add(GetDBString("vacatureID", myReader));
                    }
                }
                finally
                {
                    myReader.Close();
                    
                }
                db.connection.Close();
            }

            distinctList = VacatureList.Distinct().ToList(); //Verwijder dubbele vacatures

            for (int i = 0; i < distinctList.Count(); i++)
            {
                //Haal omschrijvingen van vacatures op
                db.setUpConnection();
                MySqlTransaction myTrans = db.connection.BeginTransaction();
                string mySelectQuery = "SELECT omschrijving FROM vacature WHERE vacatureID = '"+distinctList[i]+"'";
                MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
                MySqlDataReader myReader;
                myReader = cmd.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {                     
                        OmschrijvingList.Add(GetDBString("omschrijving", myReader));                      
                    }
                }
                finally
                {
                    myReader.Close();
                    db.connection.Close();
                }
            }

            double matchGetal;
            

            for (int c = 0; c < distinctList.Count(); c++)
            {
                matchGetal = 0;
                for (int i = 0; i < talenList.Count(); i++)
                {
                    aantal = 0;
                    
                    string a = OmschrijvingList[c];
                    string b = talenList[i];

                    //Vergelijk voor elke Programeertaal in de lijst hoe vaak deze in de omschrijving voor komt
                    MatchCollection mc = Regex.Matches(a, b, RegexOptions.IgnoreCase);

                    foreach (Match m in mc)
                    {
                        aantal++;
                    }
                    //Vermenigvuldig het aantal keer dat de taal voorkomt met de persoonlijke vaardigheid in deze taal
                    matchGetal += aantal * Convert.ToDouble(ervList[i]);
                }
                matchGetallenList.Add(matchGetal);
            }


        }

        public List<string> getVacatureIDs() {
            return distinctList;
        }

        public List<double> getMatchGetallen() {
            return matchGetallenList;
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }
    }
}
