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
    public partial class dialogForm : Form //GUI voor het geven van informatie van de crawler
    {

       

        public dialogForm()
        {
            InitializeComponent();
            label1.Text = "Uw informatie wordt opgehaald,\ndit kan enkele minuten duren.";
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
