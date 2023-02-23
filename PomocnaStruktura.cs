using System;
using System.Collections.Generic;
using System.Text;

namespace ShannonFano
{
    class PomocnaStruktura
    {
        public char slovo;
        public int freq;
        public string kod;
        public PomocnaStruktura()
        { }
        public PomocnaStruktura(char sl, int fre)
        {
            slovo = sl;
            freq = fre;
            kod = null;
        }
    }
}
