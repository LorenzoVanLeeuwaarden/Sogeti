using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Sogeti
{
    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            //Vergelijk waardes voor sortering
            int returnVal = -1;
            String stringx =((ListViewItem)x).SubItems[col].Text;
            Double valuex = Int64.Parse(stringx);
            String stringy = ((ListViewItem)y).SubItems[col].Text;
            Double valuey = Int64.Parse(stringy);
            returnVal = valuex.CompareTo(valuey);
            returnVal *= -1; //Zorg dat de sortering aflopend is
            
            return returnVal;
        }
    }
}
