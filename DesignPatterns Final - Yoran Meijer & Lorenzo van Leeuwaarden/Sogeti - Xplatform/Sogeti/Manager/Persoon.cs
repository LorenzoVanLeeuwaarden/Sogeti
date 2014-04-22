using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti
{
    class Persoon : iData
    {
        public String persoonsID { get; set; }
        public String gebruikersID { get; set; }
        public String naam { get; set; }
        public String date { get; set; }
        public String geslacht { get; set; }
        public String postcode { get; set; }
        public String plaats { get; set; }
        public String adres { get; set; }
        public String nationaliteit { get; set; }
        public String profielfoto { get; set; }
        public String emailadres { get; set; }
        public String telefoon { get; set; }

        
    }
}
