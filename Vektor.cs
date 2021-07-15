using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motos2
{
    class Vektor
    {
        public float x;
        public float y;
        public float norma;

        public Vektor(float x, float y)
        {
            this.x = x;
            this.y = y;
            norma = Norma();
        }

        public static Vektor operator + (Vektor v, Vektor u)
        {
            return new Vektor(v.x + u.x, v.y + u.y);
        }

        public static Vektor operator * (float skalar, Vektor v)
        {
            return new Vektor(skalar*v.x, skalar*v.y);
        }

        public static Vektor operator - (Vektor v)
        {
            return new Vektor(-v.x, -v.y);
        }

        public float Norma()
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public Vektor Normalizuj()
        {
            if (norma != 0)
            {
                return new Vektor(x / norma, y / norma);
            }
            else
            {
                return new Vektor(0, 0);
            }
        }

        public void Pregeneruj(float max)
        {
            Random rnd = new Random();
            x = rnd.Next(-(int)max, (int)max);
            y = rnd.Next(-(int)max, (int)max);
            norma = Norma();
        }
    }
}