using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti
{
    class Bedrijf : iData
    {
        public String bedrijfsID { get; set; }
        public String gebruikersID { get; set; }
        public String bedrijfsnaam { get; set; }
        public String bedrijfslogo { get; set; }
        public String bedrijfseigenschappen { get; set; }
        public String plaats { get; set; }
        public String adres { get; set; }
        public String postcode { get; set; }
        public String emailadres { get; set; }
        public String telefoon { get; set; }


    }
}
