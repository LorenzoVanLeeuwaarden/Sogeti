using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class CVManager
    {
        DatabaseConnection dc = new DatabaseConnection();
        public void deleteCV(string cvID)
        {
            //Verwijder cv uit de database aan de hand van opgegeven cvID
            dc.setUpConnection();
            MySqlCommand cmd = dc.connection.CreateCommand();
            cmd.CommandText = "DELETE FROM cv WHERE cvID = '" + cvID + "'";
            cmd.ExecuteNonQuery();
            dc.connection.Close();
        }
    }
}
