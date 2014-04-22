using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Sogeti
{
    class PythonManager // Activeert python script om informatie op te halen
    {
        public PythonManager(String crawler) { 
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Python27\python.exe";
            start.Arguments = @"C:\Python27\CrawlersSogeti\" + crawler;
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            dialogForm form = new dialogForm();
            using (Process proces = Process.Start(start))
            {
                form.Show();
                proces.WaitForExit();// Wacht totdat het script voltooid is voordat de applicatie verder kan
                form.Close();
            }
        }
    }
}
