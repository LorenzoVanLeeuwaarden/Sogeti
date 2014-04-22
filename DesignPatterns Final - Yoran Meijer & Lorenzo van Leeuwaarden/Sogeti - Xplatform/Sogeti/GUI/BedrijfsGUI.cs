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
    public partial class BedrijfsGUI : Form //GUI die de bedrijven gebruiken
    {
        VacatureManager vm = new VacatureManager();
        SogetiManager sm = new SogetiManager();
        Bedrijf bedrijf;
        List<Vacature> vacList;
        Vacature selectedVacature;

        String Ptalen;
        List<String> talenList;
        List<String> ervList;
        String talen;
        String erv;
        ProgrammeertalenParser parse = new ProgrammeertalenParser();

        public BedrijfsGUI(string gebruikersID)
        {
            DataManager dm = new DataManager(new BedrijfFactory(gebruikersID));
	        bedrijf = (Bedrijf)dm.buildData();
            InitializeComponent();
            getBedrijfsInfo();
            getBedrijfsVacatures();
            getKoppels();

        }


        string bedrijfsnaam;
        string werkervaring;
        string opleiding;
        string baantype;
        string carriereniveau;
        string locatie;
        string omschrijving;
        string vacatureTitel;

        public void getBedrijfsInfo() { // Geef opgehaalde informatie weer
            bedrijfsnaamLabel.Text = bedrijf.bedrijfsnaam;
            bedrijfsadresLabel.Text = bedrijf.adres;
            bedrijfspostcodeLabel.Text = bedrijf.postcode;
            bedrijfsplaatsLabel.Text = bedrijf.plaats;
            bedrijfsemailLabel.Text = bedrijf.emailadres;
            bedrijfstelefoonLabel.Text = bedrijf.telefoon;
            bedrijfsOmschrijvingTextBox.Text = bedrijf.bedrijfseigenschappen;            
        }

        public void getBedrijfsVacatures() // Haal alle vacatures van het huidige bedrijf op
        {
            VacListDataManager dam = new VacListDataManager(new VacatureFactory(bedrijf.bedrijfsID));
            vacList = dam.buildData();
            
            for (int i = 0; i < vacList.Count(); i++)
            {
                Vacature vac = (Vacature)vacList[i];
                bedrijfsvacaturesListBox.Items.Add(vac.vacatureID);
            }
        }

        
        private void vacatureWeergevenButton_Click(object sender, EventArgs e)
        {
            setVacature();
            string selected = bedrijfsvacaturesListBox.GetItemText(bedrijfsvacaturesListBox.SelectedItem);
            if (!(selected.Equals(""))){
                bedrijfsprofielPanel.Visible = false;
                vacaturePanel.Visible = true;
            }
        }

        public void setVacature() { //Weergeven van vacature
            string selected = bedrijfsvacaturesListBox.GetItemText(bedrijfsvacaturesListBox.SelectedItem);
            for (int i = 0; i < vacList.Count; i++) {
                if (vacList[i].vacatureID == selected) {
                    selectedVacature = vacList[i];
                }
            }


                bedrijfsnaamVacatureLabel.Text = bedrijf.bedrijfsnaam;
                vacaturenaamLabel.Text = selectedVacature.vacatureID;
                vacatureRichTextBox.Text = selectedVacature.omschrijving;
                vacatureErvaringLabel.Text = selectedVacature.werkervaring;
                vacatureOpleidingLabel.Text = selectedVacature.opleiding;
                vacatureBaantypeLabel.Text = selectedVacature.baantype;
                vacatureCarriereLabel.Text = selectedVacature.carriereniveau;
                vacatureLocatieLabel.Text = selectedVacature.locatie;
            
            
        }

        public void getSelectedVacature() //Ophalen van vacature
        {
            string selected = bedrijfsvacaturesListBox.GetItemText(bedrijfsvacaturesListBox.SelectedItem);
            for (int i = 0; i < vacList.Count; i++)
            {
                if (vacList[i].vacatureID == selected)
                {
                    selectedVacature = vacList[i];
                }
            }
            vacatureTitelWijzigingTextBox.Text = selectedVacature.vacatureID;
            werkervaringWijzigingTextBox.Text = selectedVacature.werkervaring;
            opleidingWijzigingTextBox.Text = selectedVacature.opleiding;
            baantypeWijzigingTextBox.Text = selectedVacature.baantype;
            carriereWijzigingTextBox.Text = selectedVacature.carriereniveau;
            locatieWijzigingTextBox.Text = selectedVacature.locatie;
            omschrijvingWijzigingTextBox.Text = selectedVacature.omschrijving;
        }

        public void wijzigSelectedVacature() //Wijzigen van vacature
        {
            vacatureTitel = vacatureTitelWijzigingTextBox.Text;
            werkervaring = werkervaringWijzigingTextBox.Text;
            opleiding = opleidingWijzigingTextBox.Text;
            baantype = baantypeWijzigingTextBox.Text;
            carriereniveau = carriereWijzigingTextBox.Text;
            locatie = locatieWijzigingTextBox.Text;
            omschrijving = omschrijvingWijzigingTextBox.Text;
            vm.updateVacature(vacatureTitel, werkervaring, opleiding, baantype, carriereniveau, omschrijving, locatie);
        }

        private void wijzigVacaturebutton_Click(object sender, EventArgs e)
        {
            wijzigSelectedVacature();
        }

        private void backButtonVacature_Click(object sender, EventArgs e)
        {
            vacList.Clear();
            bedrijfsvacaturesListBox.Items.Clear();
            getBedrijfsVacatures();
            vacaturePanel.Visible = false;
            bedrijfsprofielPanel.Visible = true;           
        }

        private void vacatureWijzigenButton_Click(object sender, EventArgs e)
        {
            wijzigingVacaturePanel.Visible = true;
            bedrijfsprofielPanel.Visible = false;
            getSelectedVacature();
            toevoegenVacatureButton.Visible = false;
            wijzigVacaturebutton.Visible = true;
        }

        
        

        private void wijzigVacatureBackButton_Click(object sender, EventArgs e)
        {
            wijzigingVacaturePanel.Visible = false;
            bedrijfsprofielPanel.Visible = true;
            bedrijfsvacaturesListBox.Items.Clear();
            vacList.Clear();
            getBedrijfsVacatures();
        }

        private void vacatureVerwijderenButton_Click(object sender, EventArgs e)
        {
            string selected = bedrijfsvacaturesListBox.GetItemText(bedrijfsvacaturesListBox.SelectedItem);
            for (int i = 0; i < vacList.Count; i++)
            {
                if (vacList[i].vacatureID == selected)
                {
                    selectedVacature = vacList[i];
                }
            }
            string vacatureID = selectedVacature.vacatureID;
            vm.deleteVacature(vacatureID);
            bedrijfsvacaturesListBox.Items.Remove(bedrijfsvacaturesListBox.SelectedItem);
        }

        private void vacatureToevoegenButton_Click(object sender, EventArgs e)
        {
            bedrijfsprofielPanel.Visible = false;
            wijzigingVacaturePanel.Visible = true;
            toevoegenVacatureButton.Visible = true;
            wijzigVacaturebutton.Visible = false;
        }

        private void toevoegenVacatureButton_Click(object sender, EventArgs e) //Toevoegen van vacature
        {
            string bedrijfsID = bedrijf.bedrijfsID;
            werkervaring = werkervaringWijzigingTextBox.Text;
            opleiding = opleidingWijzigingTextBox.Text;
            baantype = baantypeWijzigingTextBox.Text;
            carriereniveau = carriereWijzigingTextBox.Text;
            locatie = locatieWijzigingTextBox.Text;
            omschrijving = omschrijvingWijzigingTextBox.Text;
            vm.insertVacature(bedrijfsID, werkervaring,opleiding,baantype,carriereniveau,omschrijving,locatie);
            MessageBox.Show("The vacancy has been added!");
        }

        private void applyBedrijfWijzigingButton_Click(object sender, EventArgs e)
        {

            bedrijf.adres = wijzigAdresLabel.Text;
            bedrijf.bedrijfseigenschappen = wijzigEigenschappenLabel.Text;
            bedrijf.bedrijfsnaam = wijzigBedrijfsnaamLabel.Text;
            bedrijf.emailadres = wijzigEmailLabel.Text;
            bedrijf.plaats = wijzigPlaatsLabel.Text;
            bedrijf.postcode = wijzigPostcodeLabel.Text;
            bedrijf.telefoon = wijzigTelefoonLabel.Text;
            WijzigBedrijfManager wm = new WijzigBedrijfManager(bedrijf);
            wm.updateBedrijf();
            MessageBox.Show("Profile has been updated!");

        }

        private void wijzigBedrijfBackButton_Click(object sender, EventArgs e)
        {
            getBedrijfsInfo();
            wijzigBedrijfPanel.Visible = false;
        }

        private void modifyBedrijfButton_Click(object sender, EventArgs e)
        {
            wijzigAdresLabel.Text = bedrijf.adres;
            wijzigEigenschappenLabel.Text = bedrijf.bedrijfseigenschappen;
            wijzigBedrijfsnaamLabel.Text = bedrijf.bedrijfsnaam;
            wijzigEmailLabel.Text = bedrijf.emailadres;
            wijzigPlaatsLabel.Text = bedrijf.plaats;
            wijzigPostcodeLabel.Text = bedrijf.postcode;
            wijzigTelefoonLabel.Text = bedrijf.telefoon;
            wijzigBedrijfPanel.Visible = true;
        }

        private void werkzoekendeSelecterenButton_Click(object sender, EventArgs e)
        {
            ListViewItem.ListViewSubItem item = cvListView.SelectedItems[0].SubItems[0];
            string selectedCV = item.Text;
            string pID;
            string cirvicID;
            pID = sm.getPersoonsID(selectedCV);
            cirvicID = sm.getCVID(pID);
            sm.retrieveCVData(cirvicID);
            spreektalenLabel.Text = sm.cvSpreektalen;
            werkervaringLabel.Text = sm.cvWerkervaring;
            opleidingLabel.Text = sm.cvOpleiding;
            persoonskenmerkenTextBox.Text = sm.persoonskenmerken;
            Ptalen = getProgTalen();
            setProgTalenListView();
            cvPanel.Visible = true;

        }

        private void cvBackButton_Click(object sender, EventArgs e)
        {
            cvPanel.Visible = false;
        }

        public void getKoppels()
        { //Laat SogetiManager de koppels ophalen
            cvListView.Clear();
            cvListView.Items.Clear();
            sm.getKoppels();

            List<string> vacatures = sm.getKoppelVacatures();
            List<string> gebruikers = sm.getKoppelGebruikers();

            cvListView.Columns.Add("User ID", 225);
            cvListView.Columns.Add("Vacancy ID", 225);
            cvListView.View = View.Details;

            

            for (int a = 0; a < vacatures.Count; a++)
            {
                foreach (string item in bedrijfsvacaturesListBox.Items)
                {
                    if (vacatures[a].Equals(item)) {
                        string vacatureID = vacatures[a];
                        string gebruikersID = gebruikers[a];


                        ListViewItem row = new ListViewItem(gebruikersID);
                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, vacatureID));
                        cvListView.Items.Add(row);
                    }
                }

                
            }
        }

        public void setProgTalenListView()
        {
            PtList.Clear();

            talen = parse.TalenToDB(Ptalen);
            erv = parse.TaalervaringToDB(Ptalen);

            talenList = talen.Split(',').ToList();
            ervList = erv.Split(',').ToList();



            PtList.Columns.Add("Programmeertaal", 233);
            PtList.Columns.Add("Ervaring", 233);
            PtList.View = View.Details;

            for (int a = 0; a < talenList.Count; a++)
            {

                ListViewItem row = new ListViewItem(talenList[a]);
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ervList[a]));
                PtList.Items.Add(row);
            }

        }

        public String getProgTalen()
        {
            String taal = "";
            if (sm.cvProgrammeertalen != null && sm.cvTalenervaring != null)
            {
                List<String> progtalen = sm.cvProgrammeertalen.Split(',').ToList();
                List<String> taalerv = sm.cvTalenervaring.Split(',').ToList();
                taal = parse.toDisplay(progtalen, taalerv);
            }
            return taal;
        }
        


    }
}
