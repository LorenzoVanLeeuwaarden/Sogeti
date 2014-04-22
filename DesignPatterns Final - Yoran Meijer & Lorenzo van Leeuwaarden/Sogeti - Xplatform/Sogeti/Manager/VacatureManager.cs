using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class VacatureManager
    {
        DatabaseConnection dc = new DatabaseConnection();

        public void insertVacature(string bedrijfsID, string werkervaring, string opleiding, string baantype, string carriereniveau, string omschrijving, string locatie)
        {
            //Invoeren van de meegegeven informatie in de database
            werkervaring = werkervaring.Replace("'", "");
            werkervaring = werkervaring.Replace('"', ' ');
            opleiding = opleiding.Replace("'", "");
            opleiding = opleiding.Replace('"', ' ');
            baantype = baantype.Replace("'", "");
            baantype = baantype.Replace('"', ' ');
            carriereniveau = carriereniveau.Replace("'", "");
            carriereniveau = carriereniveau.Replace('"', ' ');
            omschrijving = omschrijving.Replace("'", "");
            omschrijving = omschrijving.Replace('"', ' ');
            locatie = locatie.Replace("'", "");
            locatie = locatie.Replace('"', ' ');

            dc.setUpConnection();
            MySqlCommand cmd = dc.connection.CreateCommand();
            cmd.CommandText = "INSERT INTO vacature (bedrijfsID, werkervaring, opleiding, baantype, carriereniveau, omschrijving, locatie) VALUES('" + bedrijfsID + "','" + werkervaring + "', '" + opleiding + "', '" + baantype + "', '" + carriereniveau + "', '" + omschrijving + "', '" + locatie + "')";
            cmd.ExecuteNonQuery();
            dc.connection.Close();
        }

        public void updateVacature(string vacatureID, string werkervaring, string opleiding, string baantype, string carriereniveau, string omschrijving, string locatie)
        {
            //Updaten van de meegegeven informatie in de database

            werkervaring = werkervaring.Replace("'", "");
            werkervaring = werkervaring.Replace('"', ' ');
            opleiding = opleiding.Replace("'", "");
            opleiding = opleiding.Replace('"', ' ');
            baantype = baantype.Replace("'", "");
            baantype = baantype.Replace('"', ' ');
            carriereniveau = carriereniveau.Replace("'", "");
            carriereniveau = carriereniveau.Replace('"', ' ');
            omschrijving = omschrijving.Replace("'", "");
            omschrijving = omschrijving.Replace('"', ' ');
            locatie = locatie.Replace("'", "");
            locatie = locatie.Replace('"', ' ');

            dc.setUpConnection();
            MySqlCommand cmd = dc.connection.CreateCommand();
            cmd.CommandText = "UPDATE vacature SET werkervaring = '"+werkervaring+"', opleiding = '"+opleiding+"', baantype = '"+baantype+"', carriereniveau = '"+carriereniveau+"', omschrijving = '"+omschrijving+"', locatie = '"+locatie+"' WHERE vacatureID = '"+vacatureID+"'";
            cmd.ExecuteNonQuery();
            dc.connection.Close();
        }

        public void deleteVacature(string vacatureID)
        {
            dc.setUpConnection();
            MySqlCommand cmd2 = dc.connection.CreateCommand();
            cmd2.CommandText = "DELETE FROM koppeling WHERE vacatureID = '"+vacatureID+"'";
            cmd2.ExecuteNonQuery();

            //Verwijderen van vacature aan de hand van vacatureID
            
            MySqlCommand cmd = dc.connection.CreateCommand();
            cmd.CommandText = "DELETE FROM vacature WHERE vacatureID = '"+vacatureID+"'";
            cmd.ExecuteNonQuery();


            dc.connection.Close();
        }
    }

}
