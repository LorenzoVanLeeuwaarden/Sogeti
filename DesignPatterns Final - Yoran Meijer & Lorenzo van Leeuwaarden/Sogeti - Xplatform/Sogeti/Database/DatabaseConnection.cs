using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Sogeti
{
    class DatabaseConnection
    {
        public MySqlConnection connection;
        public void setUpConnection()
        {
            //Maak verbinding met de server en open connectie met de database
            string MyConnectionString = "server=92.109.64.188;userid=school;password=school;database=sogeti";
            connection = new MySqlConnection(MyConnectionString);
            connection.Open();
            
        }
    }
}
