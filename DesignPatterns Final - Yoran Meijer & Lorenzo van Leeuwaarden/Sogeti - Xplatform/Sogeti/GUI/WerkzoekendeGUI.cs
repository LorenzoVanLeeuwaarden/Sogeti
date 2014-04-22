using Sogeti;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Threading;

namespace Sogeti
{
    public partial class WerkzoekendeGUI : Form //GUI van de werkzoekende
    {
        private System.Windows.Forms.ColumnHeader lvcState;
        private System.Windows.Forms.ColumnHeader lvcCapital;
        Persoon persoon;
        CV cv;
        String Ptalen;
        List<String> talenList;
        List<String> ervList;
        String talen;
        String erv;
        ProgrammeertalenParser parse = new ProgrammeertalenParser();
        List<string> VacatureIds = new List<string>();
        List<double> VacatureMatches = new List<double>();
        string vacatureID;

        public WerkzoekendeGUI(String gebruikersID){
            DataManager dm = new DataManager(new PersoonFactory(gebruikersID));
            persoon = (Persoon)dm.buildData();
            dm = new DataManager(new CvFactory(persoon.persoonsID));
            cv = (CV)dm.buildData();
            InitializeComponent();
            getUserInfo();
            wijzigProfielButton.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e){
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 
        }
              
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            ListViewItem.ListViewSubItem item = vacatureListView.SelectedItems[0].SubItems[1];
            string selectedVacature = item.Text;
            displayVacature(selectedVacature);
            vacaturePanel.Visible = true;
                    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vacaturePanel.Visible = false;
        }

        public void getUserInfo() { //Ophalen van gebruikers info
            naamLabel.Text = persoon.naam;
            geslachtLabel.Text = persoon.geslacht;
            leeftijdLabel.Text = persoon.date;
            nationaliteitLabel.Text = persoon.nationaliteit;
            adresLabel.Text = persoon.adres;
            postcodeLabel.Text = persoon.postcode;
            plaatsLabel.Text = persoon.plaats;
            emailLabel.Text = persoon.emailadres;
            telefoonLabel.Text = persoon.telefoon;

            Ptalen = getProgTalen();

            SpreektalenLabel.Text = cv.spreektalen;
            WerkervaringLabel.Text = cv.werkervaring;
            OpleidingenLabel.Text = cv.opleidingen;
            ProgrammeertalenLabel.Text = Ptalen;
            PersoonskenmerkenLabel.Text = cv.persoonskenmerken;


            AlgoritmeManager AM = new AlgoritmeManager();

            ManualResetEvent syncEvent = new ManualResetEvent(false);
            Thread t1 = new Thread(
            () =>
                {
                    AM.matchPersoon(persoon, cv);
                    syncEvent.Set();
                }
            );
            t1.Start();

            Thread t2 = new Thread( //Ophalen van alle vacatures om te matchen via een nieuwe thread zodat de GUI responsive blijft
            () =>
                {
                syncEvent.WaitOne();

                VacatureIds = AM.getVacatureIDs();
                VacatureMatches = AM.getMatchGetallen();

                setListView();
                }
            );
            
            t2.Start();
           

            
        }

         public void getProfielInfo() //Haal alle profiel info op
         {
            wijzigNameTextBox.Text = persoon.naam;
            wijzigGeslachtComboBox.SelectedItem = persoon.geslacht;
            if (persoon.date != "0000-00-00"&&persoon.date!=""&&persoon.date!=null)
            {
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "yyyy-MM-dd";
                dtfi.DateSeparator = "-";
                DateTime objDate = Convert.ToDateTime(persoon.date, dtfi);

                wijzigGebDatumDatePicker.Value = objDate;
            }
            else {
                wijzigGebDatumDatePicker.Value = new DateTime(1990,1, 1);
            }
            if (!persoon.nationaliteit.Equals(""))
            {
                wijzigNationaliteitComboBox.SelectedItem = persoon.nationaliteit;
            }
            else {
                wijzigNationaliteitComboBox.SelectedIndex = 1;
            }
            wijzigAdresTextBox.Text = persoon.adres;
            wijzigPostcodeTextBox.Text = persoon.postcode;
            wijzigPlaatsTextBox.Text = persoon.plaats;
            wijzigEmailTextBox.Text = persoon.emailadres;
            wijzigTelefoonTextBox.Text = persoon.telefoon;
            wijzigSprTalenTextBox.Text = cv.spreektalen;
            wijzigWerkervaringTextBox.Text = cv.werkervaring;
            wijzigOpleidingenTextBox.Text = cv.opleidingen;
            wijzigKenmerkenTextBox.Text = cv.persoonskenmerken;
            setProgTalenListView();
        }

        public void wijzigProfiel() { //Het wijzigen van de profiel
            persoon.naam = wijzigNameTextBox.Text;
            persoon.geslacht = wijzigGeslachtComboBox.Text;
            persoon.date = wijzigGebDatumDatePicker.Value.ToString("yyyy/MM/dd");
            persoon.nationaliteit = wijzigNationaliteitComboBox.SelectedItem.ToString();
            persoon.adres = wijzigAdresTextBox.Text;
            persoon.postcode = wijzigPostcodeTextBox.Text;
            persoon.plaats = wijzigPlaatsTextBox.Text;
            persoon.emailadres = wijzigEmailTextBox.Text;
            persoon.telefoon = wijzigTelefoonTextBox.Text;
            cv.spreektalen = wijzigSprTalenTextBox.Text;
            cv.werkervaring = wijzigWerkervaringTextBox.Text;
            cv.opleidingen = wijzigOpleidingenTextBox.Text;
            cv.persoonskenmerken = wijzigKenmerkenTextBox.Text;

            getUserInfo();
            

        }
        

        private void wijzigProfielButton_Click(object sender, EventArgs e)
        {
            profielPanel.Visible = false;
            wijzigProfielPanel.Visible = true;
            getProfielInfo();
        }

        private void wijzigingToepassenButton_Click(object sender, EventArgs e)
        {
            wijzigProfiel();
            WijzigPersoonManager wpm = new WijzigPersoonManager(persoon,cv);
            wpm.updateInfo();
            
            MessageBox.Show("Your profile has been modified!");
        }

        private void wijzigProfielBackButton_Click(object sender, EventArgs e)
        {
            profielPanel.Visible = true;
            wijzigProfielPanel.Visible = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void setProgTalenListView() { 
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

        public void updateProgTalenListView() { 
            PtList.Clear();
            String talen = getProgTalen();
            String taal = parse.TalenToDB(talen);
            String erv = parse.TaalervaringToDB(talen);

            List<String> progtalen = taal.Split(',').ToList();
            List<String> taalerv = erv.Split(',').ToList();
            PtList.Columns.Add("Programmeertaal", 233);
            PtList.Columns.Add("Ervaring", 233);
            PtList.View = View.Details;

            for (int a = 0; a < progtalen.Count; a++)
            {

                ListViewItem row = new ListViewItem(progtalen[a]);
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, taalerv[a]));
                PtList.Items.Add(row);
            }
        
        }

        private void setListView() { //gematchte vacatures laten zien op een gesoorteerde wijze
            
                Invoke((MethodInvoker)delegate {
                wijzigProfielButton.Enabled = true;
                vacatureListView.Clear();
                vacatureListView.Columns.Add("Match Punten", 233);
                vacatureListView.Columns.Add("Vacature ID", 233);
                vacatureListView.View = View.Details;

                for (int a = 0; a < VacatureIds.Count; a++)
                {
                    string vacatureID = VacatureIds[a];
                    double matchpunten = VacatureMatches[a];

                    ListViewItem row = new ListViewItem(matchpunten.ToString());
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, vacatureID));
                    vacatureListView.Items.Add(row);
                }

                vacatureListView.ListViewItemSorter = new ListViewItemComparer(0);
                vacatureListView.Sort();


                });

                wijzigProfielButton.Enabled = true;
                
            }

        

        private void AddProgtaalButton_Click(object sender, EventArgs e) //toevoegen van skills
        {
            String inputTaal = Interaction.InputBox("Voer een Programmeertaal in:", "", "Default", 0, 0);
            String inputErvaring = Interaction.InputBox("Voer uw ervaring met deze taal in. \nKies uit: 5.5, 6, 7, 8, 9 of 10", "", "Default", 0, 0);
            

            if (inputTaal != "" && inputErvaring != "")
            {

                switch (inputErvaring)
                {
                    case "5":
                        inputErvaring = "0";
                        break;
                    case "5.5":
                        inputErvaring = "40";
                        break;
                    case "10":
                        inputErvaring = "1";
                        break;
                    case "9":
                        inputErvaring = "5";
                        break;
                    case "8":
                        inputErvaring = "10";
                        break;
                    case "7":
                        inputErvaring = "20";
                        break;
                    case "6":
                        inputErvaring = "30";
                        break;
                    case "":
                        inputTaal = "";
                        break;
                    default:
                        inputErvaring = "Ongeldig";
                        break;
                }

                String Taal = getProgTalen();
                String addTaal = parse.TalenToDB(Taal);
                String addErv = parse.changeErvaring(Taal);


                if (cv.programmeertalen == null && cv.talenervaring == null)
                {
                    
                        cv.programmeertalen = inputTaal;
                        cv.talenervaring = inputErvaring;


                }
                else if (cv.programmeertalen.Equals("") == true && cv.talenervaring.Equals("") == true) {
                    cv.programmeertalen = inputTaal;
                    cv.talenervaring = inputErvaring;
                }
                else
                {
                    cv.programmeertalen = addTaal + "," + inputTaal;
                    cv.talenervaring = addErv + "," + inputErvaring;
                }



                updateProgTalenListView();
            }
        }

        public String getProgTalen()
        {
            String taal = "";
            if (cv.programmeertalen != null && cv.talenervaring != null)
            {
                List<String> progtalen = cv.programmeertalen.Split(',').ToList();
                List<String> taalerv = cv.talenervaring.Split(',').ToList();
                taal = parse.toDisplay(progtalen, taalerv);
            }
            return taal;
        }

        private void RemoveProgtaalButton_Click(object sender, EventArgs e)  // verwijderen van skills
        {

            if (PtList.SelectedItems.Count != 0)
            {
                ListViewItem.ListViewSubItem SelectedValue = PtList.SelectedItems[0].SubItems[0];
            
            String talen = getProgTalen();
            
            String taal = parse.TalenToDB(talen);
            String erv = parse.changeErvaring(talen);

            List<String> changeTalen = taal.Split(',').ToList();
            List<String> changeErvaring = erv.Split(',').ToList();

            for (int i = 0; i < changeTalen.Count; i++) {
                if (changeTalen[i] == SelectedValue.Text)
                {
                    changeTalen.RemoveAt(i);
                    changeErvaring.RemoveAt(i);



                    string taal2 = parse.toDisplay(changeTalen, changeErvaring);

                    cv.programmeertalen = parse.TalenToDB(taal2);
                    cv.talenervaring = parse.changeErvaring(taal2);

                    updateProgTalenListView();
                }
                }
                
            }

        }

        public void displayVacature(String selectedVacature) {
            SogetiManager sm = new SogetiManager();
            sm.retrieveVacatureData(selectedVacature);
            vacatureID = sm.vacatureTitel;
            vacaturenaamLabel.Text = sm.vacatureTitel;
            vacatureErvaringLabel.Text = sm.werkervaring;
            vacatureOpleidingLabel.Text = sm.opleiding;
            vacatureBaantypeLabel.Text = sm.baantype;
            vacatureCarriereLabel.Text = sm.carriereniveau;
            vacatureLocatieLabel.Text = sm.locatie;
            vacatureRichTextBox.Text = sm.omschrijving;
            

        
        }

        private void vacatureReageerButton_Click(object sender, EventArgs e)
        {
            SogetiManager sm = new SogetiManager();
            string gebruikersID = persoon.gebruikersID;
            sm.koppelGebruikerVacature(vacatureID, gebruikersID);
        }

    }
}
