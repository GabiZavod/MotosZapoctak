using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace motos2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pozadie = new Bitmap("pozadie.png");
            erease = new Bitmap("pozadie.png");
            g = Graphics.FromImage(pozadie);
            high = new Highscore();
            label1.Text = "Tri najlepšie skóre: " + Environment.NewLine + high.Highscores;
        }

        Graphics g;
        Bitmap pozadie;
        Bitmap erease;
        Hra hra;
        TimeSpan ubehlo;
        Highscore high;

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (hra.stav)
            {
                case Stav.bezi:
                    pozadie = erease;
                    g.DrawImage(pozadie, 0, 0);
                    hra.TimerUpdate(stlacenaSipka);
                    PrepisSkore();
                    pictureBox1.Refresh();
                    PrepisCas();
                    break;
                case Stav.prehra:
                    timer1.Enabled = false;
                    MessageBox.Show("PREHRA" + Environment.NewLine + "Pre obnovenie hry kliknite na OK");
                    Restart();
                    break;
                case Stav.vyhra:
                    timer1.Enabled = false;
                    MessageBox.Show("VYHRA" + Environment.NewLine + "Pre obnovenie hry kliknite na OK");
                    high.PoVyhre(Convert.ToInt32(ubehlo.TotalSeconds));
                    Restart();
                    break;
            }
        }

        private void PrepisSkore()
        {
            label2.Text = "Skore: " + Convert.ToString(hra.skore);
        }

        private void PrepisCas()
        {
            ubehlo = hra.casomer.Elapsed;
            label3.Text = "Cas: " + Convert.ToInt32(ubehlo.TotalSeconds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            label1.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
            button1.Visible = false;
            pictureBox1.Visible = true;
            pictureBox1.Image = pozadie;
            pictureBox2.Visible = false;

            hra = new Hra(g);
        }

        Sipka stlacenaSipka = Sipka.ziadna;

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            stlacenaSipka = Sipka.ziadna;
            if (hra != null)
            {
                hra.motos.V_pohybe = false;
            }
        }

        private void Restart()
        {
            Form1 novaHra = new Form1();
            novaHra.Show();
            this.Hide();
        }

        private void ZavriHru(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (hra != null)
            {
                if (e.KeyCode == Keys.A)
                {
                    stlacenaSipka = Sipka.dolava;
                    hra.motos.V_pohybe = true;
                }
                if (e.KeyCode == Keys.W)
                {
                    stlacenaSipka = Sipka.hore;
                    hra.motos.V_pohybe = true;
                }
                if (e.KeyCode == Keys.D)
                {
                    stlacenaSipka = Sipka.doprava;
                    hra.motos.V_pohybe = true;
                }
                if (e.KeyCode == Keys.S)
                {
                    stlacenaSipka = Sipka.dolu;
                    hra.motos.V_pohybe = true;
                }
                e.Handled = true;
            }
        }
    }
}