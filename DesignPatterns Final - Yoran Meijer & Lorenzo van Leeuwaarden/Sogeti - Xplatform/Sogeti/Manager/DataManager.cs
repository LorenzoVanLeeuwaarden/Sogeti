using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Sogeti
{
    class DataManager
    {
        iDataFactory factory;

        public DataManager(iDataFactory newfactory) {
            factory = newfactory;
        }
          

        public iData buildData()
        {
            //Maakt aan de hand van de opgegeven factory het bijbehorende object
            factory.retrieveData();
            iData data=factory.setData();
            return data;
        }

      
    }
}
