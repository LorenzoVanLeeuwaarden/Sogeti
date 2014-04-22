using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Sogeti
{
    public partial class SogetiGUI : Form //GUI voor Sogeti zelf, alle vacatures, cv's en koppelingen kunnen worden bekeken, er kan ook gecrawld worden
    {
        SogetiManager sm = new SogetiManager();
        VacatureManager vm = new VacatureManager();
        CVManager cvm = new CVManager();
        ProgrammeertalenParser parse = new ProgrammeertalenParser();

        String Ptalen;
        List<String> talenList;
        List<String> ervList;
        String talen;
        String erv;
        List<int> vacatures = new List<int>();
        List<int> cvs = new List<int>();

        string bedrijfsnaam;
        string werkervaring;
        string opleiding;
        string baantype;
        string carriereniveau;
        string locatie;
        string omschrijving;
        string vacatureTitel;

        public SogetiGUI()
        {
            InitializeComponent();
            fillLists();
            profielPanel.Visible = false;

        }

        public void fillLists(){
            vacatures = sm.getVacatures();
            cvs = sm.getCVs();

            vacatures.Sort();
            cvs.Sort();

            vacatureListBox.DataSource = vacatures;
            cvListBox.DataSource = cvs;
        }


        private void cvWeergevenButton_Click(object sender, EventArgs e) //Het weergeven van de CV
        {
            string cvID = cvListBox.GetItemText(cvListBox.SelectedItem);
            sm.retrieveCVData(cvID);
            naamLabel.Text = sm.pNaam;
            geslachtLabel.Text = sm.pGeslacht;
            leeftijdLabel.Text = sm.pGebDatum;
            nationaliteitLabel.Text = sm.pNationaliteit;
            adresLabel.Text = sm.pAdres;
            postcodeLabel.Text = sm.pPostcode;
            plaatsLabel.Text = sm.pPlaats;
            emailLabel.Text = sm.pEmailadres;
            telefoonLabel.Text = sm.pTelefoon;
            SpreektalenLabel.Text = sm.cvSpreektalen;
            werkervaringLabel.Text = sm.cvWerkervaring;
            OpleidingenLabel.Text = sm.cvOpleiding;

                    
            String taal = "";
            if (sm.cvProgrammeertalen != null && sm.cvTalenervaring != null)
            {
                List<String> progtalen = sm.cvProgrammeertalen.Split(',').ToList();
                List<String> taalerv = sm.cvTalenervaring.Split(',').ToList();
                taal = parse.toDisplay(progtalen, taalerv);               
            }
            ProgrammeertalenLabel.Text = taal;
            PersoonskenmerkenLabel.Text = sm.persoonskenmerken;
            profielPanel.Visible = true;
        }

        private void cvModifyButton_Click(object sender, EventArgs e) //Het aanpassen van de CV
        {
            wijzigProfielPanel.Visible = true;
            string cvID = cvListBox.GetItemText(cvListBox.SelectedItem);
            sm.retrieveCVData(cvID);

            wijzigNameTextBox.Text = sm.pNaam;
            wijzigGeslachtComboBox.SelectedItem = sm.pGeslacht;
            if (sm.pGebDatum != "0000-00-00" && sm.pGebDatum != "" && sm.pGebDatum != null)
            {
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "yyyy-dd-MM";
                dtfi.DateSeparator = "-";
                DateTime objDate = Convert.ToDateTime(sm.pGebDatum, dtfi);

                wijzigGebDatumDatePicker.Value = objDate;
            }
            else
            {
                wijzigGebDatumDatePicker.Value = new DateTime(1990, 1, 1);
            }

            wijzigNationaliteitComboBox.SelectedItem = sm.pNationaliteit;
            wijzigAdresTextBox.Text = sm.pAdres;
            wijzigPostcodeTextBox.Text = sm.pPostcode;
            wijzigPlaatsTextBox.Text = sm.pPlaats;
            wijzigEmailTextBox.Text = sm.pEmailadres;
            wijzigTelefoonTextBox.Text = sm.pTelefoon;
            wijzigSprTalenTextBox.Text = sm.cvSpreektalen;
            wijzigWerkervaringTextBox.Text = sm.cvWerkervaring;
            wijzigOpleidingenTextBox.Text = sm.cvOpleiding;
            wijzigKenmerkenTextBox.Text = sm.persoonskenmerken;
            setProgTalenListView();
        }

        public void setProgTalenListView() // Skills weergeven
        {
            PtList.Clear();
            Ptalen = getProgTalen();

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

        private void cvVerwijderenButton_Click(object sender, EventArgs e) // Het verwijderen van de CV
        {
            string cvID = cvListBox.GetItemText(cvListBox.SelectedItem);
            cvm.deleteCV(cvID);
            cvListBox.DataSource = null;
            fillLists();
        }

        private void vacatureWeergevenButton_Click(object sender, EventArgs e) // Het weergeven van de vacature
        {
            string selected = vacatureListBox.GetItemText(vacatureListBox.SelectedItem);
            sm.retrieveVacatureData(selected);
            vacatureWeergavePanel.Visible = true;
            vacaturenaamLabel.Text = sm.vacatureTitel;
            bedrijfsnaamVacatureLabel.Text = sm.bedrijfsnaam;
            vacatureErvaringLabel.Text = sm.werkervaring;
            vacatureOpleidingLabel.Text = sm.opleiding;
            vacatureBaantypeLabel.Text = sm.baantype;
            vacatureCarriereLabel.Text = sm.carriereniveau;
            vacatureLocatieLabel.Text = sm.locatie;
            vacatureRichTextBox.Text = sm.omschrijving;
        }

        private void vacatureModifyButton_Click(object sender, EventArgs e) // Het wijzigen van de vacature
        {
            string selected = vacatureListBox.GetItemText(vacatureListBox.SelectedItem);
            sm.retrieveVacatureData(selected);
            wijzigingVacaturePanel.Visible = true;
            vacatureTitelWijzigingTextBox.Text = sm.vacatureTitel;
            werkervaringWijzigingTextBox.Text = sm.werkervaring;
            opleidingWijzigingTextBox.Text = sm.opleiding;
            baantypeWijzigingTextBox.Text = sm.baantype;
            carriereWijzigingTextBox.Text = sm.carriereniveau;
            locatieWijzigingTextBox.Text = sm.locatie;
            omschrijvingWijzigingTextBox.Text = sm.omschrijving;
        }

        private void vacatureVerwijderenButton_Click(object sender, EventArgs e) //Het verwijderen van de vacature
        {
            string vacatureID = vacatureListBox.GetItemText(vacatureListBox.SelectedItem);
            vm.deleteVacature(vacatureID);
            vacatureListBox.DataSource = null;
            fillLists();
        }

        private void backButtonVacature_Click(object sender, EventArgs e)
        {
            
            fillLists();
            vacatureWeergavePanel.Visible = false;
        }

        private void wijzigVacatureBackButton_Click(object sender, EventArgs e)
        {
          
            fillLists();
            wijzigingVacaturePanel.Visible = false;
        }

        private void wijzigVacaturebutton_Click(object sender, EventArgs e)
        {
            vacatureTitel = vacatureTitelWijzigingTextBox.Text;
            werkervaring = werkervaringWijzigingTextBox.Text;
            opleiding = opleidingWijzigingTextBox.Text;
            baantype = baantypeWijzigingTextBox.Text;
            carriereniveau = carriereWijzigingTextBox.Text;
            locatie = locatieWijzigingTextBox.Text;
            omschrijving = omschrijvingWijzigingTextBox.Text;
            vm.updateVacature(vacatureTitel, werkervaring, opleiding, baantype, carriereniveau, omschrijving, locatie);
            MessageBox.Show("Your vacancy has been updated!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            profielPanel.Visible = false;
        }

        private void AddProgtaalButton_Click(object sender, EventArgs e)// Toevoegen van skills
        {
            String inputTaal = Interaction.InputBox("Voer een Programmeertaal in:", "Prompt", "Default", 0, 0);
            String inputErvaring = Interaction.InputBox("Voer uw ervaring met deze taal in. \nKies uit: 5.5, 6, 7, 8, 9 of 10", "Prompt", "Default", 0, 0);


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


                if (sm.cvProgrammeertalen == null && sm.cvTalenervaring == null)
                {

                    sm.cvProgrammeertalen = inputTaal;
                    sm.cvTalenervaring = inputErvaring;


                }
                else if (sm.cvProgrammeertalen.Equals("") == true && sm.cvTalenervaring.Equals("") == true)
                {
                    sm.cvProgrammeertalen = inputTaal;
                    sm.cvTalenervaring = inputErvaring;
                }
                else
                {
                    sm.cvProgrammeertalen = addTaal + "," + inputTaal;
                    sm.cvTalenervaring = addErv + "," + inputErvaring;
                }



                updateProgTalenListView();
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

        public void updateProgTalenListView()
        { 
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

        private void wijzigProfielBackButton_Click(object sender, EventArgs e)
        {
            wijzigProfielPanel.Visible = false;
        }

        private void openCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrawlerForm crawler = new CrawlerForm();
            crawler.ShowDialog();
        }

        private void openKoppelschermToolStripMenuItem_Click(object sender, EventArgs e)
        {
            koppelGUI koppelaar = new koppelGUI();
            koppelaar.ShowDialog();
        }

        private void RemoveProgtaalButton_Click(object sender, EventArgs e) // Verwijderen van skills
        {
            if (PtList.SelectedItems.Count != 0)
            {
                ListViewItem.ListViewSubItem SelectedValue = PtList.SelectedItems[0].SubItems[0];

                String talen = getProgTalen();

                String taal = parse.TalenToDB(talen);
                String erv = parse.changeErvaring(talen);

                List<String> changeTalen = taal.Split(',').ToList();
                List<String> changeErvaring = erv.Split(',').ToList();

                for (int i = 0; i < changeTalen.Count; i++)
                {
                    if (changeTalen[i] == SelectedValue.Text)
                    {
                        changeTalen.RemoveAt(i);
                        changeErvaring.RemoveAt(i);



                        string taal2 = parse.toDisplay(changeTalen, changeErvaring);

                        sm.cvProgrammeertalen = parse.TalenToDB(taal2);
                        sm.cvTalenervaring = parse.changeErvaring(taal2);

                        updateProgTalenListView();
                    }
                }

            }
        }

        private void wijzigingToepassenButton_Click(object sender, EventArgs e)
        {
            wijzigProfiel();
            sm.updateInfo();
        }

        public void wijzigProfiel() //Het wijzigen van het profiel
        {
            sm.pNaam = wijzigNameTextBox.Text;
            sm.pGeslacht = wijzigGeslachtComboBox.Text;
            sm.pGebDatum = wijzigGebDatumDatePicker.Value.ToString("yyyy/MM/dd");
            sm.pNationaliteit = wijzigNationaliteitComboBox.SelectedItem.ToString();
            sm.pAdres = wijzigAdresTextBox.Text;
            sm.pPostcode = wijzigPostcodeTextBox.Text;
            sm.pPlaats = wijzigPlaatsTextBox.Text;
            sm.pEmailadres = wijzigEmailTextBox.Text;
            sm.pTelefoon = wijzigTelefoonTextBox.Text;
            sm.cvSpreektalen = wijzigSprTalenTextBox.Text;
            sm.cvWerkervaring = wijzigWerkervaringTextBox.Text;
            sm.cvOpleiding = wijzigOpleidingenTextBox.Text;
            sm.persoonskenmerken = wijzigKenmerkenTextBox.Text;

            naamLabel.Text = sm.pNaam;
            geslachtLabel.Text = sm.pGeslacht;
            leeftijdLabel.Text = sm.pGebDatum;
            nationaliteitLabel.Text = sm.pNationaliteit;
            adresLabel.Text = sm.pAdres;
            postcodeLabel.Text = sm.pPostcode;
            plaatsLabel.Text = sm.pPlaats;
            emailLabel.Text = sm.pEmailadres;
            telefoonLabel.Text = sm.pTelefoon;
            SpreektalenLabel.Text = sm.cvSpreektalen;
            werkervaringLabel.Text = sm.cvWerkervaring;
            OpleidingenLabel.Text = sm.cvOpleiding;


            String taal = "";
            if (sm.cvProgrammeertalen != null && sm.cvTalenervaring != null)
            {
                List<String> progtalen = sm.cvProgrammeertalen.Split(',').ToList();
                List<String> taalerv = sm.cvTalenervaring.Split(',').ToList();
                taal = parse.toDisplay(progtalen, taalerv);
            }
            ProgrammeertalenLabel.Text = taal;
            PersoonskenmerkenLabel.Text = sm.persoonskenmerken;


        }
    }
}
