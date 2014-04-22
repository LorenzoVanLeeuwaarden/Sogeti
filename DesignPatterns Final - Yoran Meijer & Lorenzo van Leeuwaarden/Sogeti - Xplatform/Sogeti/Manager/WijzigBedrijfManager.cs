using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class WijzigBedrijfManager
    {
        Bedrijf bedrijf;
        public WijzigBedrijfManager(Bedrijf updateBedrijf) {
            bedrijf = updateBedrijf;
            parseGegevens();
        }

        public void updateBedrijf() {
            //Update gegevens

            DatabaseConnection db = new DatabaseConnection();
            db.setUpConnection();
            MySqlTransaction myTrans = db.connection.BeginTransaction();
             try
                {

                    //Invoeren of updaten van cv

                    db.setUpConnection();
                    MySqlCommand cmdWijzigBedrijf = db.connection.CreateCommand();
                    cmdWijzigBedrijf.CommandText = "UPDATE bedrijf SET bedrijfsnaam='" + bedrijf.bedrijfsnaam + "', bedrijfseigenschappen='" + bedrijf.bedrijfseigenschappen + "', adres='" + bedrijf.adres + "', postcode='" + bedrijf.postcode + "', plaats='" + bedrijf.plaats + "', emailadres='" + bedrijf.emailadres + "', telefoon='"+ bedrijf.telefoon +"' WHERE bedrijfsID='" + bedrijf.bedrijfsID + "'";
                    cmdWijzigBedrijf.ExecuteNonQuery();

                }


                finally
                {
                    
                    db.connection.Close();
                }
        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }


        private void parseGegevens()
        {
            //Verwijder alle onnodige characters

            bedrijf.adres = bedrijf.adres.Replace("'", "");
            bedrijf.adres = bedrijf.adres.Replace('"', ' ');
            bedrijf.bedrijfseigenschappen = bedrijf.bedrijfseigenschappen.Replace("'", "");
            bedrijf.bedrijfseigenschappen = bedrijf.bedrijfseigenschappen.Replace('"', ' ');
            bedrijf.bedrijfsnaam = bedrijf.bedrijfsnaam.Replace("'", "");
            bedrijf.bedrijfsnaam = bedrijf.bedrijfsnaam.Replace('"', ' ');
            bedrijf.emailadres = bedrijf.emailadres.Replace("'", "");
            bedrijf.emailadres = bedrijf.emailadres.Replace('"', ' ');
            bedrijf.plaats = bedrijf.plaats.Replace("'", "");
            bedrijf.plaats = bedrijf.plaats.Replace('"', ' ');
            bedrijf.postcode = bedrijf.postcode.Replace("'", "");
            bedrijf.postcode = bedrijf.postcode.Replace('"', ' ');
            bedrijf.telefoon = bedrijf.telefoon.Replace("'", "");
            bedrijf.telefoon = bedrijf.telefoon.Replace('"', ' ');

        }
    }
}
