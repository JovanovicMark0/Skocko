using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TVSkocko_72_2018
{
    public partial class Form2Top10 : Form
    {
        public Form2Top10()
        {
            InitializeComponent();

        }

        private void Form2Top10_Load(object sender, EventArgs e)
        {
            //POPUNI GRID VIEW PODACIMA IZ BAZE
            BazaRezultata baza = new BazaRezultata();
            List<StatistikaIgraca> statistike = baza.procitajIzBaze();
            DataTable t = new DataTable();
            t.TableName = "top10";
            DataColumn c0 = new DataColumn("Broj igrača");
            DataColumn c1 = new DataColumn("Igrač");
            DataColumn c2 = new DataColumn("Osvojeni rezultat");

            t.Columns.Add(c0);
            t.Columns.Add(c1);
            t.Columns.Add(c2);

            for(int i=0; i<statistike.Count;i++)
            {
                DataRow red = t.NewRow();
                red["Broj igrača"] = i+1;
                red["Igrač"] = statistike[i].KorisnickoIme;
                red["Osvojeni rezultat"] = statistike[i].BrBodova;
                t.Rows.Add(red);
            }
            DataView data = new DataView(t);
            dataGridView1.DataSource = data;
        }

    }
}
