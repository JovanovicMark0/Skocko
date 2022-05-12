using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace TVSkocko_72_2018
{
    public class BazaRezultata
    {
        public SqliteConnection konekcija;
        public SqliteDataReader reader;
        public SqliteCommand komanda;
        
        public BazaRezultata()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../db/rezultati.db";
            konekcija = napraviKonekciju(path);
        }

        SqliteConnection napraviKonekciju(string path)
        {
            konekcija = new SqliteConnection("Data Source=" + path);
            return konekcija;
        }

        public List<StatistikaIgraca> procitajIzBaze()
        {
            konekcija.Open();

            komanda = konekcija.CreateCommand();
            komanda.CommandText = "SELECT * FROM top10 ORDER BY rezultat DESC LIMIT 10;";
            List<StatistikaIgraca> statistike = new List<StatistikaIgraca>();
            reader = komanda.ExecuteReader();
            while(reader.Read())
            {
                statistike.Add(new StatistikaIgraca(reader.GetString(0), reader.GetInt32(1)));
            }

            konekcija.Close();

            return statistike;
        }

        public void upisiUBazu(string username, int brBodova)
        {
            konekcija.Open();

            komanda = konekcija.CreateCommand();
            komanda.CommandText = "INSERT INTO top10(username, rezultat) VALUES ('" + username + "'," + brBodova + ")";
            reader = komanda.ExecuteReader();

            konekcija.Close();
        }
    }
}
