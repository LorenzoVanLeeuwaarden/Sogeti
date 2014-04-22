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

    public partial class RegistratieGUI : Form //GUI voor het registeren van een persoon of bedrijf
    {
        string username;
        string password;
        string verifypassword;
        string naam;
        string plaats;
        string adres;
        string postcode;
        string email;
        string telefoon;
        Boolean isPerson;
            
        

        Sogeti.InLogManager loginManager = new Sogeti.InLogManager();
        inlogGUI inlogGui;

        public RegistratieGUI()
        {
            InitializeComponent();
        }

        public void getSender(inlogGUI gui) {
            inlogGui = gui;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loginManager.getRegistratieGUI(this);
            loginManager.getinlogGUI(inlogGui);
            checkFields();            
        }
        
        public void checkFields() { //Controleer of alles ingevuld is
            username = UsernameTextBox.Text;
            password = PasswordTextBox.Text;
            verifypassword = verifyPasswordTextBox.Text;
            naam = nameTextBox.Text;
            plaats = cityTextBox.Text;
            adres = adressTextBox.Text;
            postcode = zipcodeTextBox.Text;
            email = emailTextBox.Text;
            telefoon = telephoneTextBox.Text;
            
            if (personRadioButton.Checked) {
                isPerson = true;
            } if (companyRadioButton.Checked) {
                isPerson = false;
            }

            Boolean noPass = false;
            Boolean noUser = false;
            Boolean noName = false;

            if (password.Equals("") || verifypassword.Equals(""))
            {
                errorLabel.Text = "Please enter a password!";
                passwordLabel.ForeColor = System.Drawing.Color.Red;
                verifyPasswordLabel.ForeColor = System.Drawing.Color.Red;
                noPass = true;
            }

            if (username.Equals(""))
            {
                noUser = true;
                usernameLabel.ForeColor = System.Drawing.Color.Red;
                errorLabel.Text = "Please enter a username!";
                errorLabel.ForeColor = System.Drawing.Color.Red;
            }

            if (naam.Equals(""))
            {
                noName = true;
                nameLabel.ForeColor = System.Drawing.Color.Red;
                errorLabel.ForeColor = System.Drawing.Color.Red;
                errorLabel.Text = "Please enter your name or that of your company!";
            }

            if (!password.Equals(verifypassword))
            {
                errorLabel.Text = "The passwords don't match!";
                errorLabel.ForeColor = System.Drawing.Color.Red;
                passwordLabel.ForeColor = System.Drawing.Color.Red;
                verifyPasswordLabel.ForeColor = System.Drawing.Color.Red;
            }
            else if (noPass == false && noUser == false && noName == false)
            {
                
                errorLabel.Text = "";
                usernameLabel.ForeColor = System.Drawing.Color.Black;
                passwordLabel.ForeColor = System.Drawing.Color.Black;
                verifyPasswordLabel.ForeColor = System.Drawing.Color.Black;
                nameLabel.ForeColor = System.Drawing.Color.Black;
                checkUsername();
            }
        
        }

        public void checkUsername() //Laat loginManager controleren of alles klopt
        {
            loginManager.getConnection();
            loginManager.getAllUsernamePasswords();
            if (loginManager.checkUsername(UsernameTextBox.Text) == true)
            {
                errorLabel.Text = "This username is already in use, please use a different one!";
                errorLabel.ForeColor = System.Drawing.Color.Red;
                usernameLabel.ForeColor = System.Drawing.Color.Red;
            }
            else {
                register();
            }

        }

        public void register() { //Voeg persoon of bedrijf toe
            loginManager.getConnection();
            loginManager.register(username, password, naam, plaats, adres, postcode, email, telefoon, isPerson);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            inlogGui.Visible = true;
            this.Close();
        }

    }
}
