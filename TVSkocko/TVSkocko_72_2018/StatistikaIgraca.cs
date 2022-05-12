using System;
using System.Collections.Generic;
using System.Text;

namespace TVSkocko_72_2018
{
    public class StatistikaIgraca
    {
        private string korisnickoIme;
        private int brBodova;
        
        public string KorisnickoIme
        {
            get { return korisnickoIme; }
            set { korisnickoIme = value; }
        }
        public int BrBodova
        {
            get {return brBodova; }
            set { brBodova = value; }
        }

        public StatistikaIgraca(string username, int brBodova)
        {
            korisnickoIme = username;
            this.brBodova = brBodova;
        }
    }
}
