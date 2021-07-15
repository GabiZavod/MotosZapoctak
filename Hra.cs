using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace motos2
{ 
    enum Sipka { ziadna, dolava, hore, doprava, dolu};
    enum Stav { nezacala, bezi, vyhra, prehra};

    class Hra
    {
        List<Prvok> prvkybezMotos = new List<Prvok>();
        List<float> xove_suradnice = new List<float>();
        List<float> yove_suradnice = new List<float>();
        Graphics graphics;
        public Motos motos;
        public Stav stav = Stav.nezacala;
        public Stopwatch casomer = new Stopwatch();
        public int skore;

        public Hra(Graphics gr)
        {
            prvkybezMotos.Add(new Beecon());
            prvkybezMotos.Add(new Beecon());
            prvkybezMotos.Add(new RedPupa());
            prvkybezMotos.Add(new RedPupa());
            prvkybezMotos.Add(new BluePupa());
            motos = new Motos();
            VygenerujSuradnicePrvkov();
            graphics = gr;
            stav = Stav.bezi;
            skore = 0;
            VykresliPrvky();
            casomer.Start();
        }

        public void TimerUpdate(Sipka stsipka)
        {
            PohniPrvkami(stsipka);
            VykresliPrvky();
            CheckNaraz();
            CheckPrehra();
            CheckVyletel();
            CheckVyhra();
        }

        private void VygenerujSuradnicePrvkov()
        {
            xove_suradnice.Add(motos.X);
            yove_suradnice.Add(motos.Y);
            for (int i = 0; i<prvkybezMotos.Count; i++)
            {
                prvkybezMotos[i].VygenerujSuradnicu(xove_suradnice, yove_suradnice);
            }
        }

        private void PohniPrvkami(Sipka s)
        {
            motos.Pohyb(s);
            for (int i = 0; i < prvkybezMotos.Count; i++)
            {
                prvkybezMotos[i].Pohyb();
            }
        }

        private void VykresliPrvky()
        {
            motos.Vykresli(graphics);
            for (int i=0; i<prvkybezMotos.Count; i++)
            {
                prvkybezMotos[i].Vykresli(graphics);
            }
        }

        private void CheckNaraz()
        {
            for (int i=0; i<prvkybezMotos.Count; i++)
            {
                if (NaraziliDvaPrvky(motos, prvkybezMotos[i]))
                {
                    SpracujNaraz(motos, prvkybezMotos[i]);
                }
            }
        }

        private void CheckPrehra()
        {
            if (motos.JeMimo())
            {
                stav = Stav.prehra;
                casomer.Stop();
            }
        }

        private void CheckVyhra()
        {
            if (prvkybezMotos.Count == 0 && (motos.X >= 235 && motos.X<=245) && (motos.Y >= 245 && motos.Y <= 275 ))
            {
                stav = Stav.vyhra;
                casomer.Stop();
            }
        }

        private void CheckVyletel()
        {
            for (int i = 0; i < prvkybezMotos.Count; i++)
            {
                if (prvkybezMotos[i].JeMimo())
                {
                    skore += prvkybezMotos[i].Hodnota;
                    prvkybezMotos.RemoveAt(i);
                }
            }
        }

        private bool NaraziliDvaPrvky(Prvok jeden, Prvok druhy)
        {
            if (jeden.X >= druhy.X + 18 || druhy.X >= jeden.X + 18)
            {
                return false;
            }
            if (jeden.Y >= druhy.Y + 18 || druhy.Y >= jeden.Y + 18)
            {
                return false;
            }
            return true;
        }

        private void SpracujNaraz(Prvok jeden, Prvok druhy)
        {
            float t = 1;        //cas posobenia sily
            jeden.V_pohybe = true;
            jeden.Po_naraze = true;
            druhy.Po_naraze = true;

            if (druhy.GetType() == typeof(Beecon))
            {
                druhy.V_pohybe = true;
            }

            jeden.Vektor_sily = jeden.Hmotnost * jeden.Vektor_zrychlenia;           //sila vyvynuta jednym posobiaca na druhy
            druhy.Vektor_sily = druhy.Hmotnost * druhy.Vektor_zrychlenia;           //sila vyvynuta druhym posobiaca na jeden
            //zmeny hybnosti
            Vektor zmena_hybnosti_druheho = t * jeden.Vektor_sily;
            Vektor zmena_hybnosti_prveho = t * druhy.Vektor_sily;
            //zmeny rychlosti
            //prvy:
            jeden.Vektor_rychlosti = -jeden.Vektor_rychlosti +zmena_hybnosti_prveho;
            jeden.Vektor_rychlosti.norma = jeden.Vektor_rychlosti.Norma();
            //druhy: 
            druhy.Vektor_rychlosti = -druhy.Vektor_rychlosti + zmena_hybnosti_druheho;
            druhy.Vektor_rychlosti.norma = druhy.Vektor_rychlosti.Norma();
        }
    }

    class Highscore
    {
        string filename = @"highscores.txt";
        int[] skore = new int[3];
        string highscores;
        public string Highscores
        {
            get { return highscores; }
        }

        public Highscore()
        {
            StreamReader file = new StreamReader(filename);
            NacitajSkore(file);
            highscores = skore[0] + Environment.NewLine + skore[1] + Environment.NewLine + skore[2];
        }

        public void PoVyhre(int cas_konca)
        {
            PridajAktualneSkore(cas_konca);
            ZapisSkore();
        }

        private void NacitajSkore(StreamReader f)
        {
            for (int i = 0; i < 3; i++)
            {
                skore[i] = Convert.ToInt32(f.ReadLine());
            }
            f.Close();
        }

        private void PridajAktualneSkore(int cas)
        {
            if (cas < skore[0])
            {
                skore[2] = skore[1];
                skore[1] = skore[0];
                skore[0] = cas;
            }
            else if (skore[0] == 0)
            {
                skore[0] = cas;
            }
            else if (cas > skore[0] && cas < skore[1])
            {
                skore[2] = skore[1];
                skore[1] = cas;
            }
            else if (skore[1] == 0)
            {
                skore[1] = cas;
            }
            else if (cas > skore[1] && cas < skore[2] || skore[2] == 0)
            {
                skore[2] = cas;
            }

            highscores = skore[0] + Environment.NewLine + skore[1] + Environment.NewLine + skore[2];
        }

        private void ZapisSkore()
        {
            StreamWriter subor = new StreamWriter(filename, false);
            subor.Write(highscores);
            subor.Close();
        }
    }
}