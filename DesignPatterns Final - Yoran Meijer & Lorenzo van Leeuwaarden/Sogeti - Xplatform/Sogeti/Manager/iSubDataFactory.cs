using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti
{
    interface iSubDataFactory
    {
        void retrieveData();
        List<Vacature> getList();
    }
}
