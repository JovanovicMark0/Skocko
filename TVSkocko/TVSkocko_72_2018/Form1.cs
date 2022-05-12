using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

namespace TVSkocko_72_2018
{
    public partial class TVSkockoForm1 : Form
    {
        //zvuk
        System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
        int mute = -1;
        
        //progressBAR
        int progressMAX = 50;

        public Igra igra = new Igra();
        String korisnickoIme;

        public int i, j;
        int brPoteza = 0;
        int novBrPoteza = 0;
        
        private const int yDim = 4;
        private const int xDim = 6;
        Panel[,] panelMatrix1 = new Panel[yDim, xDim];
        Panel[,] panelMatrix2 = new Panel[yDim, xDim];
        List<Panel> panelResenje = new List<Panel>();
        Polje[] odigranaPolja = new Polje[24];

        //SLIKE
        Image praznoLevo = System.Drawing.Image.FromFile(@"..\..\..\slike\praznolevo.bmp");
        Image praznoDesno = System.Drawing.Image.FromFile(@"..\..\..\slike\praznodesno.bmp");
        Image hearts = System.Drawing.Image.FromFile(@"..\..\..\slike\hearts.bmp");
        Image karo = System.Drawing.Image.FromFile(@"..\..\..\slike\karo.bmp");
        Image pik = System.Drawing.Image.FromFile(@"..\..\..\slike\pik.bmp");
        Image pogodak = System.Drawing.Image.FromFile(@"..\..\..\slike\pogodak.bmp");
        Image polupogodak = System.Drawing.Image.FromFile(@"..\..\..\slike\polupokodak.bmp");
        Image skocko = System.Drawing.Image.FromFile(@"..\..\..\slike\skocko.bmp");
        Image tref = System.Drawing.Image.FromFile(@"..\..\..\slike\tref.bmp");
        Image zvezda = System.Drawing.Image.FromFile(@"..\..\..\slike\zvezda.bmp");


        //----------------------------------------------------------------------------------------------
        //  INICIJALIZACIJA I POKRETANJE IGRE
        //----------------------------------------------------------------------------------------------

        public TVSkockoForm1(string username) //INICIJALIZACIJA IGRE I POSTAVLJANJE TAJMERA
        {
            korisnickoIme = username;
            System.Diagnostics.Debug.WriteLine("IGRA POČINJE!");
            InitializeComponent();
            panelMatrix1 = GeneratePanelMatrix(panel1);
            panelMatrix2 = GeneratePanelMatrix(panel2);
            panelResenje = GenerisiPanelResenja(panel3);
            restart();

            void timer1_Tick(object sender, EventArgs e)//SVAKI OTKUCAJ TAJMERA / PROGRESS BARA
            {
                if (progressBar1.Value == progressMAX)
                {
                    timer1.Stop();
                    DialogResult dR = MessageBox.Show("Ponovi igru?", "Vreme je isteklo!", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        restart();
                    }
                    else 
                    {
                        this.Close();
                    }
                }
                else
                {
                    progressBar1.Value++;
                }
            }

            timer1.Tick += new EventHandler(timer1_Tick);

        } 

        public void initSlike()     //POSTAVLJA INICIJALNIH SLIKA POLJA
        {
            for (int i = 0; i < yDim; i++)
                {
                    for (int j = 0; j < xDim; j++)
                    {
                        panelMatrix1[i, j].BackgroundImage = praznoLevo;
                        panelMatrix2[i, j].BackgroundImage = praznoDesno;
                    }
                    panelResenje[i].BackgroundImage = praznoLevo;
                }
        }

        private Panel[,] GeneratePanelMatrix(Panel parentP) //GENERIŠE PANELE IGRE
        {
            int[,] matrix = new int[4, 6];
            //InitializeMatrix(ref matrix);  //Corresponds to the real matrix
            int celNr = 1;
            Panel[,] pomMatrix = new Panel[yDim, xDim];
            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    pomMatrix[y, x] = new Panel()
                    {
                        Width = 40,
                        Height = 40,
                        Text = matrix[y, x].ToString(),
                        Location = new Point(y * 40,
                                              x * 40),  // <-- You might want to tweak this
                        Parent = parentP,
                        //BackColor = Color.Black,
                        //BackgroundImage = slika,
                        BorderStyle = BorderStyle.FixedSingle,
                    };
                    pomMatrix[y, x].Tag = celNr++;
                    //panelMatrix[y, x].Click += MatrixButtonClick;
                }
            }
            return pomMatrix;
        }

        private List<Panel> GenerisiPanelResenja(Panel parentP) //GENERIŠE PANEL REŠENJA
        {
            int celNr = 1;
            List<Panel> pomList = new List<Panel>();
            for (int y = 0; y < yDim; y++)
            {
                Panel p = new Panel()
                {
                    Width = 40,
                    Height = 40,
                    Text = "panelR" + y,
                    Location = new Point(y * 40),  
                    Parent = parentP,
                    BorderStyle = BorderStyle.FixedSingle,
                };
                pomList.Add(p);
                pomList[y].Tag = celNr++;
            }
            return pomList;
        }

        public void otvorenaSacuvanaIgra() //POPUNJAVA POLJA PO SAVE FAJLU
        { 
            for(int i=0;i<brPoteza; i++)
            {
                igra.postaviZnak(odigranaPolja[i]);
                postaviZnakNaPolje(odigranaPolja[i]);
                odigraj();   
            }
        }

        public void restart() //POSTAVLJA IGRU NA POČETNI POLOŽAJ
        {
            for (int i = 0; i < 24; i++)
            {
                odigranaPolja[i] = Polje.PRAZNOL;
            }
            brPoteza = 0;
            novBrPoteza = 0;
            igra.smisliResenje();
            igra.TrenutnoPolje[0] = 0;
            igra.TrenutnoPolje[1] = 0;
            karoBtn.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            initSlike();
            progressBar1.Value = 0;
            progressBar1.Maximum = progressMAX;
            sound.SoundLocation = "slagalica.wav";
            sound.PlayLooping();
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 1000;
            label1.Text = "";
        }

        //----------------------------------------------------------------------------------------------
        //  IGRA
        //----------------------------------------------------------------------------------------------
        public void popuniTabeluDesno(int tacno, int nijenamestu)   //NA OSNOVU POGODAKA POPUNJAVA TABELU DESNO
        {
            int i;
            for (i = 0; i < tacno; i++)
            {
                panelMatrix2[i, igra.TrenutnoPolje[1]].BackgroundImage = pogodak;
            }
            if (tacno < 4 && nijenamestu > 0)
                for (i = tacno; i < tacno + nijenamestu; i++)
                {
                    panelMatrix2[i, igra.TrenutnoPolje[1]].BackgroundImage = polupogodak;
                }
        }

        public bool proveriResenje() //PROVERAVA PRE POPUNJAVANJA TABELE DESNO
        {
            int tacnihPogodaka = 4;
            int tacnoNijeNaMestu = 0;
            int[] pokusajPreskoci = { 0, 0, 0, 0 };
            int[] resenjePreskoci = { 0, 0, 0, 0 };
            bool tacno = true;
            Polje[] resenjeIgre = igra.ResenjeIgre;
            for(int i=0;i<yDim;i++)
            {
                if (resenjeIgre[i] != igra.TabelaLevo[i, igra.TrenutnoPolje[1]]) //da li NEMA podudaranja na tom mestu
                {
                    tacno = false;
                    tacnihPogodaka--;

                    for (int j = 0; j < yDim; j++)
                    {
                        if (resenjeIgre[i] == igra.TabelaLevo[j, igra.TrenutnoPolje[1]] && pokusajPreskoci[j] == 0 && resenjePreskoci[i] == 0)
                        {
                            if(resenjeIgre[j] != igra.TabelaLevo[j, igra.TrenutnoPolje[1]])
                                tacnoNijeNaMestu++;
                            pokusajPreskoci[j] = 1;
                            resenjePreskoci[i] = 1;
                            break;
                        }
                    }
                }
                else
                {
                    pokusajPreskoci[i] = 1;
                    resenjePreskoci[i] = 1;
                }         
            }
            popuniTabeluDesno(tacnihPogodaka, tacnoNijeNaMestu);
            return tacno;
        }

        public int daLiJeKraj() //PROVERAVA DA LI JE KRAJ IGRE
        {
            //0 traje, 1 pobeda, -1 poraz
            int kraj = 0; //igra traje
            if (proveriResenje() == true) //da li je pogodio resenje 
            {
               return kraj = 1; //pobeda
            }
            else //nije pogodio resenje prelazimo u novi red
            {
                igra.TrenutnoPolje[0] = 0; //prva kolona
                igra.TrenutnoPolje[1]++;    //sledeci red
                kraj = 0; //igra traje
            }
            if (igra.TrenutnoPolje[1] == xDim) //da li je izasao iz poslednjeg reda?
            {
                return kraj = -1; //poraz
            }
            
            return kraj;
        }
        
        public void odigraj() //UKOLIKO JE POBEDA SACUVAJ, UKOLIKO JE PORAZ ZAVRSI, UKOLIKO NIJE POMERI TRENUTNO POLJE
        {
            //0 traje, 1 pobeda, -1 poraz
            if (igra.TrenutnoPolje[0] == 3)
            {
                int kraj = daLiJeKraj(); 
                if(kraj != 0) //poraz ili pobeda
                {
                    ispisiResenje();
 
                    if (kraj == 1)
                    {
                        int brBodova = 100;
                        brBodova -= progressBar1.Value;
                        brBodova -= (novBrPoteza / 4);
                        StatistikaIgraca statistika = new StatistikaIgraca(korisnickoIme, brBodova);
                        BazaRezultata baza = new BazaRezultata();
                        baza.upisiUBazu(korisnickoIme, statistika.BrBodova);

                        label1.Text = "Čestitamo pogodili ste rešenje!\n Osvojili ste "+statistika.BrBodova+" bodova!";
                    }
                    else
                    {
                        label1.Text = "Izgubili ste.";
                    }
                }
            }
            else
            {
                igra.TrenutnoPolje[0]++;
            }
        }

        public void postaviZnakNaPolje(Polje znak) //POSTAVLJA SE ZNAK NA ODGOVARAJUĆE POLJE
        {
            switch (znak)
            {
                case Polje.KARO:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = karo;
                    break;

                case Polje.HEARTS:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = hearts;
                    break;

                case Polje.PIK:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = pik;
                    break;

                case Polje.SKOCKO:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = skocko;
                    break;

                case Polje.TREF:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = tref;
                    break;

                case Polje.ZVEZDA:
                    panelMatrix1[igra.TrenutnoPolje[0], igra.TrenutnoPolje[1]].BackgroundImage = zvezda;
                    break;
            }
            odigranaPolja[novBrPoteza] = znak;
            novBrPoteza++;
        }

        public void ispisiResenje() //POPUNJAVA PANEL SA REŠENJEM IGRE (POBEDA/PORAZ/DUGME)
        {
            for (int i = 0; i < yDim; i++)
            {
                Polje znak = igra.ResenjeIgre[i];
                switch (znak)
                {
                    case Polje.KARO:
                        panelResenje[i].BackgroundImage = karo;
                        break;

                    case Polje.HEARTS:
                        panelResenje[i].BackgroundImage = hearts;
                        break;

                    case Polje.PIK:
                        panelResenje[i].BackgroundImage = pik;
                        break;

                    case Polje.SKOCKO:
                        panelResenje[i].BackgroundImage = skocko;
                        break;

                    case Polje.TREF:
                        panelResenje[i].BackgroundImage = tref;
                        break;

                    case Polje.ZVEZDA:
                        panelResenje[i].BackgroundImage = zvezda;
                        break;
                }
            }
            karoBtn.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            timer1.Stop();
        }

        //----------------------------------------------------------------------------------------------
        //  AKCIJE SA TASTERA
        //----------------------------------------------------------------------------------------------

        private void top10Btn_Click(object sender, EventArgs e)    //IZLISTAJ TOP 10 NAJBOLJIH IGRAČA
        {
            Form2Top10 Form = new Form2Top10();
            timer1.Stop();
            Form.ShowDialog();
            restart();
        }

        private void button60_Click(object sender, EventArgs e)    //REŠENJE -POZIVA POPUNJAVANJE REŠENJA
        {
            ispisiResenje();
        }

        private void novaIgraBtn_Click(object sender, EventArgs e) //NOVA IGRA DUGME
        {
            restart();
        }

        private void muteBtn_Click(object sender, EventArgs e)     //UGASI ZVUK
        {
            if (mute == -1)
            {
                sound.Stop();
            }
            else
            {
                sound.Play();
            }
            mute *= -1;
        }

        private void button1_Click(object sender, EventArgs e)     // NAPUSTI IGRU
        {
            DialogResult dR = MessageBox.Show("Želite da napustite igru?", "TV Skočko", MessageBoxButtons.YesNo);
            if (dR == DialogResult.Yes)
            {
                this.Close();
            }
        }

        //----------------------------------------------------------------------------------------------
        //  SERIJALIZACIJA I DESERIJALIZACIJA
        //----------------------------------------------------------------------------------------------

        private void button8_Click(object sender, EventArgs e)     //ČUVA SE TRENUTNO STANJE IGRE
        {
            //serijalizacija igre
            Save s = new Save(igra.ResenjeIgre, odigranaPolja, progressBar1.Value, novBrPoteza);
            IFormatter f = new BinaryFormatter();
            Stream st = new FileStream(@"../../sacuvano/igra.txt", FileMode.Create, FileAccess.Write);
            f.Serialize(st, s);
            st.Close();
            label1.Text = "Igra uspešno sačuvana.";
        }

        private void button9_Click(object sender, EventArgs e)     //NASTAVLJA SE SAČUVANA IGRA
        {
            //deserijalizacija igre
            IFormatter f = new BinaryFormatter();
            if(File.Exists(@"../../sacuvano/igra.txt"))
            {
                Stream st = new FileStream(@"../../sacuvano/igra.txt", FileMode.Open, FileAccess.Read);
                Save s = (Save)f.Deserialize(st);
                restart();

                igra.ResenjeIgre = s.TabelaResenje;
                odigranaPolja = s.PanelLevo;
                progressBar1.Value = s.Value;
                brPoteza = s.BrPoteza;
                st.Close();

                otvorenaSacuvanaIgra();
            }
            else
            {
                MessageBox.Show("Trenutno nemate sačuvanih partija!");
            }
        }

        //----------------------------------------------------------------------------------------------
        //  KLIK NA ZNAK 
        //----------------------------------------------------------------------------------------------

        private void button3_Click(object sender, EventArgs e) //TREF
        {
            igra.postaviZnak(Polje.TREF);
            postaviZnakNaPolje(Polje.TREF);
            odigraj();
        }

        private void button4_Click(object sender, EventArgs e) //HEARTS
        {
            igra.postaviZnak(Polje.HEARTS);
            postaviZnakNaPolje(Polje.HEARTS);
            odigraj();
        }

        private void button5_Click(object sender, EventArgs e) //PIK
        {
            igra.postaviZnak(Polje.PIK);
            postaviZnakNaPolje(Polje.PIK);
            odigraj();
        }

        private void button2_Click(object sender, EventArgs e) //KARO
        {
            igra.postaviZnak(Polje.KARO);
            postaviZnakNaPolje(Polje.KARO);
            odigraj();
        }

        private void button6_Click(object sender, EventArgs e) //ZVEZDA
        {
            igra.postaviZnak(Polje.ZVEZDA);
            postaviZnakNaPolje(Polje.ZVEZDA);
            odigraj();
        }

        private void button7_Click(object sender, EventArgs e) //SKOČKO
        {
            igra.postaviZnak(Polje.SKOCKO);
            postaviZnakNaPolje(Polje.SKOCKO);
            odigraj();
        }
    }
}
