using System;
using System.Drawing;
using System.Collections.Generic;

namespace motos2
{
    abstract class Prvok
    {
        protected bool v_pohybe = false;
        public bool V_pohybe
        {
            get { return v_pohybe; }
            set { v_pohybe = value; }
        }

        protected bool po_naraze = false;
        public bool Po_naraze
        {
            get { return po_naraze; }
            set { po_naraze = value; }
        }

        protected float x;
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        protected float y;
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        protected float hmotnost;
        public float Hmotnost
        {
            get { return hmotnost; }
        }

        protected float max_rychlost;
        public float Max_rychlost
        {
            get { return max_rychlost; }
        }

        protected Vektor vektor_rychlosti;
        public Vektor Vektor_rychlosti
        {
            get { return vektor_rychlosti; }
            set { vektor_rychlosti = value; }
        }

        protected Vektor vektor_zrychlenia;
        public Vektor Vektor_zrychlenia
        {
            get { return vektor_zrychlenia; }
            set { vektor_zrychlenia = value; }
        }

        protected Vektor vektor_hybnosti;
        public Vektor Vektor_hybnosti
        {
            get { return vektor_hybnosti; }
            set { vektor_hybnosti = value; }
        }

        protected Vektor vektor_sily;
        public Vektor Vektor_sily
        {
            get { return vektor_sily; }
            set { vektor_sily = value; }
        }
        protected Bitmap ikona;
        protected int hodnota;
        public int Hodnota
        {
            get { return hodnota; }
        }

        protected int reakcia_po_naraze;
        protected int od_narazu;
        public int Od_narazu
        {
            set { od_narazu = value; }
        }

        public void VygenerujSuradnicu(List<float> pouzite_x, List<float> pouzite_y)
        {
            Random rnd = new Random();
            x = rnd.Next(50, 425);
            while (pouzite_x.Contains(x))
            {
                x = rnd.Next(50, 425);
            }
            pouzite_x.Add(x);

            y = rnd.Next(60, 435);
            while (pouzite_y.Contains(y))
            {
                y = rnd.Next(60, 435);
            }
            pouzite_y.Add(y);
        }

        public virtual void Pohyb()
        {
            //priratat zrychlenie
            if (vektor_rychlosti.norma < max_rychlost && vektor_rychlosti.norma != 0)
            {
                vektor_zrychlenia = vektor_rychlosti.Normalizuj();
                vektor_rychlosti = vektor_rychlosti + vektor_zrychlenia;
                vektor_rychlosti.norma = vektor_rychlosti.Norma();
            }
            vektor_hybnosti = hmotnost * vektor_rychlosti;
        }

        public void Vykresli(Graphics gr)
        {
            gr.DrawImage(ikona, x, y);
        }

        public virtual bool JeMimo()
        {
            if (x < 50 || x > 425 || y < 60 || y > 445)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    class Motos : Prvok
    {
        public Motos()
        {
            x = 250;
            y = 260;
            hmotnost = 5;
            max_rychlost = 7;
            vektor_rychlosti = new Vektor(0, 0);
            vektor_zrychlenia = vektor_rychlosti.Normalizuj();
            vektor_hybnosti = hmotnost * vektor_rychlosti;
            ikona = new Bitmap("motos.png");
            hodnota = 0;
            reakcia_po_naraze = 360;
        }

        public override bool JeMimo()
        {
            if (x < 37 | x > 437 | y < 47 | y > 447)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Pohyb(Sipka sipka)
        {
            if (po_naraze)
            {
                od_narazu += 60;
                if (od_narazu >= reakcia_po_naraze)
                {
                    po_naraze = false;
                    od_narazu = 0;
                    v_pohybe = false;
                    vektor_rychlosti = new Vektor(0, 0);
                }
            }

            if (!po_naraze || !v_pohybe)
            {
                switch (sipka)
                {
                    case Sipka.ziadna:
                        break;
                    case Sipka.dolava:
                        if (vektor_rychlosti.x >= 0)
                        {
                            vektor_rychlosti.x = -1;
                        }
                        vektor_rychlosti.y = 0;
                        v_pohybe = true;
                        break;
                    case Sipka.hore:
                        if (vektor_rychlosti.y >= 0)
                        {
                            vektor_rychlosti.y = -1;
                        }
                        vektor_rychlosti.x = 0;
                        v_pohybe = true;
                        break;
                    case Sipka.doprava:
                        if (vektor_rychlosti.x <= 0)
                        {
                            vektor_rychlosti.x = 1;
                        }
                        vektor_rychlosti.y = 0;
                        v_pohybe = true;
                        break;
                    case Sipka.dolu:
                        if (vektor_rychlosti.y <= 0)
                        {
                            vektor_rychlosti.y = 1;
                        }
                        vektor_rychlosti.x = 0;
                        v_pohybe = true;
                        break;
                    default:
                        break;
                }
            }

            if (v_pohybe)
            {
                x += vektor_rychlosti.x;
                y += vektor_rychlosti.y;
            }

            vektor_rychlosti.norma = vektor_rychlosti.Norma();
            base.Pohyb();

        }
    }

    abstract class Pupa : Prvok
    {
        protected int cas_na_zmenu;
        protected int od_poslednej_zmeny = 0;

        public override void Pohyb()
        {
            x += vektor_rychlosti.x;
            y += vektor_rychlosti.y;
            base.Pohyb();
            od_poslednej_zmeny = od_poslednej_zmeny + 60;       //timer1.Interval
            if (JeNaOkraji() && !po_naraze)
            {
                vektor_rychlosti = -vektor_rychlosti;
                od_poslednej_zmeny = 0;
            }

            if (po_naraze)
            {
                od_narazu += 60;
                if (od_narazu >= reakcia_po_naraze)
                {
                    po_naraze = false;
                    od_narazu = 0;
                    vektor_rychlosti.Pregeneruj(max_rychlost);
                    od_poslednej_zmeny = 0;
                }
            }

            if (od_poslednej_zmeny >= cas_na_zmenu)
            {
                vektor_rychlosti.Pregeneruj(max_rychlost);
                od_poslednej_zmeny = 0;
            }
        }

        public bool JeNaOkraji()
        {
            if (x < 65 || x > 410 || y < 75 || y > 430)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class RedPupa : Pupa
    {
        public RedPupa()
        {
            hmotnost = 5;
            max_rychlost = 6;
            vektor_rychlosti = new Vektor(4, 1);
            vektor_hybnosti = hmotnost * vektor_rychlosti;
            vektor_zrychlenia = vektor_rychlosti.Normalizuj();
            ikona = new Bitmap("red_pupa.png");
            cas_na_zmenu = 1500;
            reakcia_po_naraze = 720;
            hodnota = 300;
        }
    }

    class BluePupa : Pupa
    {
        public BluePupa()
        {
            hmotnost = 6;
            max_rychlost = 5;
            vektor_rychlosti = new Vektor(2, 0);
            vektor_hybnosti = hmotnost * vektor_rychlosti;
            vektor_zrychlenia = vektor_rychlosti.Normalizuj();
            ikona = new Bitmap("blue_pupa.png");
            cas_na_zmenu = 1200;
            reakcia_po_naraze = 600;
            hodnota = 400;
        }
    }

    class Beecon : Prvok
    {

        public Beecon()
        {
            hmotnost = 100;
            max_rychlost = 2;
            vektor_rychlosti = new Vektor(0, 0);
            vektor_hybnosti = hmotnost * vektor_rychlosti;
            vektor_zrychlenia = vektor_rychlosti.Normalizuj();
            ikona = new Bitmap("beecon.png");
            hodnota = 1000;
            reakcia_po_naraze = 300;
        }

        public override void Pohyb()
        {
            if (v_pohybe && od_narazu < reakcia_po_naraze)
            {
                x += vektor_rychlosti.x;
                y += vektor_rychlosti.y;
                base.Pohyb();
                od_narazu += 60;
            }
            else
            {
                od_narazu = 0;
                v_pohybe = false;
                vektor_rychlosti = new Vektor(0, 0);

            }
        }
    }
}