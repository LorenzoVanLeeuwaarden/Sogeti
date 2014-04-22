using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Sogeti
{
    public partial class CrawlerForm : Form //GUI voor het aansturen van de crawlers
    {
        public String crawlerCheck;
        public int paginas;

        public CrawlerForm()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new PythonManager(crawlerCheck); 
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 40;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            crawlerCheck = "vacaturebank.py";

        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            crawlerCheck = "jobbird.py";
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            crawlerCheck = "monsterboard.py";
        }

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            crawlerCheck = "CV.py";
        }
    }
}
