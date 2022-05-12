using System;
using System.Collections.Generic;
using System.Text;

namespace TVSkocko_72_2018
{
    public enum Polje
    {
        HEARTS,
        KARO,
        PIK,
        SKOCKO,
        TREF,
        ZVEZDA,
        PRAZNOL,
        PRAZNOD,
        POGODAK,
        POLUPOGODAK,
    }
    public class Igra
    {
        private const int yDim = 4;
        private const int xDim = 6;
        private Polje[,] tabelaLevo;
        private int[] trenutnoPolje;
        private Polje[,] tabelaDesno;
        private Polje[] tabelaResenje = new Polje[4];

        public Igra()
        {
            start();
        }

        public Polje[,] TabelaLevo
        {
            get { return tabelaLevo; }
            set { tabelaLevo = value; }
        }


        public int[] TrenutnoPolje
        {
            get { return trenutnoPolje; }
            set { trenutnoPolje = value; }
        }
        public Polje[] ResenjeIgre
        {
            get { return tabelaResenje; }
            set { tabelaResenje = value; }
        }


        public void start()
        {
            smisliResenje();
            ocistiTabele();
        }
        public void postaviZnak(Polje znak)
        {
            tabelaLevo[trenutnoPolje[0], trenutnoPolje[1]] = znak;
        }

        public void ocistiTabele()
        {
            tabelaLevo = new Polje[yDim, xDim];
            trenutnoPolje = new int[2];
            trenutnoPolje[0] = 0;
            trenutnoPolje[1] = 0;
            tabelaDesno = new Polje[yDim, xDim];
        }
        public void smisliResenje()
        {
            Random rand = new Random();
            Array Polja = typeof(Polje).GetEnumValues();
            int i;
            for (i = 0; i < 4; i++)
            {
                Polje polje = (Polje)Polja.GetValue(rand.Next(Polja.Length - 6));
                tabelaResenje[i] = polje;
            }
            //System.Diagnostics.Debug.WriteLine("RESENJE->" + resenjeIgre[3].ToString());
        }
    }
}
