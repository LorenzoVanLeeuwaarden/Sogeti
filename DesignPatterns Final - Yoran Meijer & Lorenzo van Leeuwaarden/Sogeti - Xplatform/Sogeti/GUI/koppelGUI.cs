using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sogeti
{
    public partial class koppelGUI : Form //GUI voor het laten zien van gekoppelde CV's en Vacatures
    {

        SogetiManager sm = new SogetiManager();
        public koppelGUI()
        {
            InitializeComponent();
            getKoppels();
        }

        public void getKoppels() { //Laat SogetiManager de koppels ophalen
            koppelListView.Clear();
            koppelListView.Items.Clear();
            sm.getKoppels();

            List<string> vacatures = sm.getKoppelVacatures();
            List<string> gebruikers = sm.getKoppelGebruikers();

            koppelListView.Columns.Add("User ID", 225);
            koppelListView.Columns.Add("Vacancy ID", 225);
            koppelListView.View = View.Details;

            for (int a = 0; a < vacatures.Count; a++)
            {
                string vacatureID = vacatures[a];
                string gebruikersID = gebruikers[a];

                ListViewItem row = new ListViewItem(gebruikersID);
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, vacatureID));
                koppelListView.Items.Add(row);
            }
        }

        private void koppelVerwijderButton_Click(object sender, EventArgs e)
        {
            koppelVerwijderen();
        }

        public void koppelVerwijderen() { // Verwijderen van de koppeling
            ListViewItem.ListViewSubItem item1 = koppelListView.SelectedItems[0].SubItems[0];
            ListViewItem.ListViewSubItem item2 = koppelListView.SelectedItems[0].SubItems[1];
            sm.deleteKoppel(item1.Text, item2.Text);
            foreach (ListViewItem eachItem in koppelListView.SelectedItems)
            {
                koppelListView.Items.Remove(eachItem);
            }
        }

        
    }
}
