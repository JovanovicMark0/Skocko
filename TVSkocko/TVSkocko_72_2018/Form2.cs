using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TVSkocko_72_2018
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "Igrac";
            TVSkockoForm1 igra = new TVSkockoForm1(textBox1.Text);
            
            igra.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2Top10 top10 = new Form2Top10();
            top10.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
