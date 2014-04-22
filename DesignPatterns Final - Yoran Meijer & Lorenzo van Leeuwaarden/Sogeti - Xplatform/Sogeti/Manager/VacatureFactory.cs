using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class VacatureFactory : iSubDataFactory
    {
        public String vacatureTitel;
        public String gebruikersID;
        public String vacatureID;
        public String bedrijfsID;
        public String locatie;
        public String werkervaring;
        public String opleiding;
        public String baantype;
        public String carriereniveau;
        public String omschrijving;
        public List<Vacature> vacatureList = new List<Vacature>();

        public VacatureFactory(String newID) {
            bedrijfsID = newID;
        }

        public void retrieveData()
        {
            //Haal informatie op aan de hand van opgegeven bedrijfsID
            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
            string mySelectQuery = "SELECT * FROM vacature WHERE bedrijfsID='" + bedrijfsID + "'";
            MySqlCommand cmd = new MySqlCommand(mySelectQuery, db.connection, myTrans);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    vacatureID = GetDBString("vacatureID", myReader);
                    bedrijfsID = GetDBString("bedrijfsID", myReader);
                    locatie = GetDBString("locatie", myReader);
                    werkervaring = GetDBString("werkervaring", myReader);
                    opleiding = GetDBString("opleiding", myReader);
                    baantype = GetDBString("baantype", myReader);
                    carriereniveau = GetDBString("carriereniveau", myReader);
                    omschrijving = GetDBString("omschrijving", myReader);
                    setData();
                }
            }
            finally
            {
                myReader.Close();
                db.connection.Close();
            }
        }

        public void setData()
        {
            //Maak een nieuw object en vul deze met de opgehaald informatie
            Vacature vacature = new Vacature();
            vacature.baantype = baantype;
            vacature.bedrijfsID = bedrijfsID;
            vacature.carriereniveau = carriereniveau;
            vacature.locatie = locatie;
            vacature.omschrijving = omschrijving;
            vacature.opleiding = opleiding;
            vacature.vacatureID = vacatureID;
            vacature.werkervaring = werkervaring;
            vacatureList.Add(vacature);
            
        }

        public List<Vacature> getList() {
            return vacatureList;
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }
    }
}
