using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TVSkocko_72_2018
{
    [Serializable]
    public class Save
    { 
        private Polje[] tabelaResenje; //čuva rešenje  
        private Polje[] panelMatrix1;  //čuva odigrane znakove u niz
        private int value;             //čuva vrednost progressBara
        private int brPoteza;          //čuva broj odigranih poteza

        public Save(Polje[] tabelaR, Polje[] p1, int v, int p)
        {
            tabelaResenje = tabelaR;
            panelMatrix1 = p1;
            value = v;
            brPoteza = p;
        }

        public int BrPoteza
        {
            get { return brPoteza; }
        }
        public int Value
        {
            get { return value; }
        }
        
        public Polje[] TabelaResenje
        {
            get { return tabelaResenje; }
        }

        public Polje[] PanelLevo
        {
            get { return panelMatrix1; }
        }


    }
}

