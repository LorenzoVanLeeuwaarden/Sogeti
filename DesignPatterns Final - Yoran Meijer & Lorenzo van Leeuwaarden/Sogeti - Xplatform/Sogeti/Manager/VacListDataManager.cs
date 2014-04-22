using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti
{
    class VacListDataManager
    {
        iSubDataFactory factory;

        public VacListDataManager(iSubDataFactory newfactory) {
            factory = newfactory;
        }
          

        public List<Vacature> buildData()
        {
            //Maak aan de hand van de opgegeven factory de bijbehorende list
            factory.retrieveData();
            List<Vacature> vacList = factory.getList();
            return vacList;
        }
    }
}
