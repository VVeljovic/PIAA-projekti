using System;
using System.Collections.Generic;
using System.Text;

namespace ShannonFano
{
    class BSTNode
    {
        public PomocnaStruktura[] key;
        public BSTNode right;
        public BSTNode left;
        public BSTNode()
        {
            key = null;
            left = null;
            right = null;
        }
        public BSTNode(PomocnaStruktura[] e)
        {
            key = e;
            left = null;
            right = null;
        }
        public  void Half(PomocnaStruktura[] niz, out PomocnaStruktura[] niz1, out PomocnaStruktura[] niz2)
        {

            int brojilo = 0;
            int suma = 0;
            int i = 0;
            for (i = 0; i < niz.Length; i++)
            {

                suma += niz[i].freq;
            }
            i = 0;
            int polovinasume = 0;
            int p = 0;

            while (polovinasume < suma / 2)
            {
                brojilo++;
                polovinasume += niz[i].freq;
                i++;
            }
            niz1 = new PomocnaStruktura[i];

            niz2 = new PomocnaStruktura[niz.Length - i];
            i = 0;
            while (i != niz1.Length)
            {
                niz1[i] = niz[i];
                i++;
            }
            int k = 0;
            while (i < niz.Length)
            {
                niz2[k] = niz[i];
                k++;
                i++;
            }

        }
    }
}
