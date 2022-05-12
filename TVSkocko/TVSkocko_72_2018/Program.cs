using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSkocko_72_2018
{
    static class Program
    {
        /*
         * 
         * PROJEKTNI ZADATAK : IGRA SKOCKO 
         * PREDMET:  Dizajniranje Softvera
         * USTANOVA: Prirodno-matematički fakultet u Kragujevcu
         * MENTOR:   Dr Svetozar Rančić
         * STUDENT:  Marko Jovanović 72/2018
         * E-MAIL:   marko.rmg784.sorcem@gmail.com
         * E-MAIL2:  72-2018@pmf.kg.ac.rs
         * 
        */
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new TVSkockoForm1());
            Application.Run(new Form2());
        }
    }
}
