using Sogeti;
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
    
    public partial class inlogGUI : Form //GUI voor het inloggen van een admin of gebruiker
    {
        
        
        string gebruikersnaam = "";
        string wachtwoord = "";
        InLogManager loginManager = new InLogManager();
       
        
        public inlogGUI()
        {
            InitializeComponent();
            
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            loginManager.getinlogGUI(this);
            loginManager.getConnection();
            loginManager.getAllUsernamePasswords();
            connectDB();            
        }

        

        public void connectDB() { //laat logInManager controleren of gebruikersnaam en wachtwoord kloppen
            gebruikersnaam = usernameTextBox.Text;
            wachtwoord = passwordBox.Text;
            loginManager.getConnection();
            if (loginManager.compareStrings(gebruikersnaam, wachtwoord) == false)
            {
                usernameLabel.ForeColor = System.Drawing.Color.Red;
                passwordLabel.ForeColor = System.Drawing.Color.Red;
                errorLabel.ForeColor = System.Drawing.Color.Red;
                errorLabel.Text = "Deze gegevens zijn onjuist!";
            }
            else {
                usernameLabel.ForeColor = System.Drawing.Color.Black;
                passwordLabel.ForeColor = System.Drawing.Color.Black;
                errorLabel.ForeColor = System.Drawing.Color.Black;
                errorLabel.Text = "";
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            RegistratieGUI registratieGUI = new RegistratieGUI();
            registratieGUI.getSender(this);
            registratieGUI.ShowDialog();

        }
    }
}