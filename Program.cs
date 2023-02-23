using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShannonFano
{
    
    class Program
    {
        public static List<bool> Encode(string dekoderb)
        {
            List<bool> povratna = new List<bool>();
            char[] niz = dekoderb.ToCharArray();
            for(int i =0; i<dekoderb.Length;i++)
            {
                if (niz[i] == '1')
                    povratna.Add(true);
                else
                    povratna.Add(false);
            }
            return povratna;
            
        }

        static void Main(string[] args)
        {
            //ispis koda svake cifre je u fajlu Pisanje.txt
            //ispis dekodiranog fajla sa recnikom je u IspisTeksta.txt
            string ime = "lorem100k.txt"; //ovde je potrebno promeniti ime fajla iz kog se cita
            string line;
            string fajlzaispis = "IspisTeksta.txt";
            string fajlzakodiranje = "Pisanje.txt";
            string fajlzaproveru = "Provera.txt";
            using (StreamReader sr = new StreamReader(ime))
            {
                line = sr.ReadToEnd();

            }

            string str = line;

            int i = 0;
            //broj pojavljivanja karaktera u fajlu 
            //Console.WriteLine(str.Length);
            PomocnaStruktura[] pomocninizkaraktera = new PomocnaStruktura[str.Length];
            //ova petlja cupa svaki karakter iz teksta i broji njegova pojavljivanja
            while (str.Length > 0)
            {
               
                int cal = 0;
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[0] == str[j])
                    {
                        cal++;
                    }
                }
                pomocninizkaraktera[i] = new PomocnaStruktura(str[0], cal);
                i++;
                
                str = str.Replace(str[0].ToString(), string.Empty);
            }

            PomocnaStruktura[] nizkaraktera = new PomocnaStruktura[i];
            i = 0;
            PomocnaStruktura temp = new PomocnaStruktura();
            while (i < pomocninizkaraktera.Length && pomocninizkaraktera[i] != null)
            {
                nizkaraktera[i] = new PomocnaStruktura(pomocninizkaraktera[i].slovo, pomocninizkaraktera[i].freq);
                i++;
            }
            //u nizkaraktera se nalaze sva slova koja su procitana i broj pojavljivanja
            int imin;
            //radimo sortiranje nizakaraktera po broju pojavljivanja nekog slova u tekstu koji je procitan
            for (i = 0; i < nizkaraktera.Length; i++)
            {
                imin = i;
                for (int l = i + 1; l < nizkaraktera.Length; l++)
                {
                    if (nizkaraktera[l].freq > nizkaraktera[imin].freq)
                    { imin = l; }

                }
                temp = nizkaraktera[imin];
                nizkaraktera[imin] = nizkaraktera[i];
                nizkaraktera[i] = temp;
            }
           
           //krecu operacije sa stablom
            BSTree drvo = new BSTree();
            drvo.Add(nizkaraktera);
            //kodiranje svih slova u stablu
            drvo.Kodiranje();
           
            i = 0;
            
            string duzina = null;
            using (StreamReader kralj = new StreamReader(ime))
            {
                duzina = kralj.ReadToEnd();
            }
            //upisivanje koda svakog slova u fajl Pisanje.txt
            //svako slovo je dobilo kod pomocu stabla
            //procitamo sad svako slovo iz fajla i sve dok ga ne nadjemo u nizukaraktera 
            //nastavljamo kretanje kroz niz
            //kada dodjemo do njega stampamo kod u Pisanje.txt
            using (StreamReader citac = new StreamReader(ime))
            {
                using (StreamWriter ss = new StreamWriter(fajlzakodiranje))
                {
                    int brojprocitanihkaraktera = 0;
                    while (brojprocitanihkaraktera < duzina.Length)
                    {
                        char p = (char)citac.Read();
                        brojprocitanihkaraktera++;
                        int pe = 0;
                        while (p != nizkaraktera[pe].slovo)
                            pe++;
                        ss.Write(nizkaraktera[pe].kod);
                    }
                }
            }
                
                string kodirani = null;
            
                using (StreamReader sk = new StreamReader(fajlzakodiranje))
                {
                    kodirani = sk.ReadToEnd();
                }
                //sad ovde taj kod za ceo procitan tekst koji je 10101.. smestam u string kodirani
                //onda da bih ga prebacio u bitarray prvo sam napisao funkciju Encode i stavio ga u List<bool>
                //i na kraju sam ga samo upisao u bitarray
                List<bool> encodedSource = new List<bool>();


                List<bool> encodedSymbol = Encode(kodirani);
                encodedSource.AddRange(encodedSymbol);

                BitArray bits = new BitArray(encodedSource.ToArray());
                string binarni = "Binarniispis.bin";
           
            byte[] bajtovi = new byte[bits.Length / 8 + (bits.Length % 8 == 0 ? 0 : 1)];
            bits.CopyTo(bajtovi, 0);
            
             string recnik = null;
            //ovde je sad ispis recnika u fajl za kodiranje
            using (StreamWriter arena = new StreamWriter("fajlzakodiranje"))
            {
                int pp = 0;
                while (pp < nizkaraktera.Length && nizkaraktera[pp] != null)
                {
                    arena.WriteLine(nizkaraktera[pp].slovo + nizkaraktera[pp].kod);
                  recnik += nizkaraktera[pp].slovo;
                    recnik += " ";
                    recnik += nizkaraktera[pp].kod;
                    pp++;
                }
                pp++;
            }
                StreamReader sp = new StreamReader("Pisanje.txt");
            string kod = sp.ReadToEnd();
            byte[] recnikb = Encoding.ASCII.GetBytes(recnik);
            byte[] bytes = new byte[bajtovi.Length + recnikb.Length];
            Buffer.BlockCopy(bajtovi, 0, bytes,0,bajtovi.Length);
            Buffer.BlockCopy(recnikb, 0, bytes, bajtovi.Length, recnikb.Length);
            //u bytes se nalazi prvi kod za ceo tekst,pa zatim i recnik
            File.WriteAllBytes(binarni, bytes);
            //proces dekodiranja
            //prvo sam sve procitao iz binarne arhive
            //onda se u bajtovi2 nalazi kod teksta
            byte[] sve = File.ReadAllBytes(binarni);
            byte[] bajtovi2 = new byte[bajtovi.Length];
            Buffer.BlockCopy(sve, 0, bajtovi2, 0, bajtovi.Length);
           // bajtovi2 = File.ReadAllBytes("Binarniispis.bin");
            BitArray bits2 = new BitArray(bajtovi2);

            //string path33 = null;
            //for (int s = 0; s < bits.Length; s++)
            //{
            //    if (bits2[s] == true)
            //        path33 += "1";
            //    else
            //        path33 += "0";
            //}
            //e sad ovde posto mi se kod nalazi u bits2
            //pretvaram ga u booleanarray
            //pa onda u string
            //pa u char[]
            //Naime kako sam radio zadatak za milion i 10m karaktera previse sporo mi je radilo za int i bitarray a mnogo brze za char[],pa sam smisljao nacin kako da prebacim u char[]
            Boolean[] nizkaraktera5 = new Boolean[bits2.Length];
            bits2.CopyTo(nizkaraktera5, 0);
            string path33 = null;
            string result = string.Join("", nizkaraktera5.Select(b => b.ToString()).ToArray());
            char[] nizkarakteranas=new char[bits2.Length];
            int brojacc=0;
            int brojacc2=0;
            while(brojacc<result.Length)
            {
                if (result[brojacc] == 'T')
                {
                    nizkarakteranas[brojacc2] = '1';

                    brojacc2++;
                }
                else if(result[brojacc]=='F')
                {
                    nizkarakteranas[brojacc2] = '0';
                    brojacc2++;
                }
                brojacc++;
                
            }
            brojacc = 0;

            //for (int s = 0; s < nizkaraktera5.Length; s++)
            //{
            //    if (nizkaraktera5[s] == true)
            //        path33 += "1";
            //    else
            //        path33 += "0";
            //}
            //ovo je sad samo dekodiranje 
            int counter = 0;
            int brojacunizu = 0;
            string pera = null;
            string dekoderb = null;
            int co = 0;
            using (StreamWriter sr = new StreamWriter(fajlzaproveru))
            {

                while (counter < nizkarakteranas.Length)
                {
                    pera += nizkarakteranas[co];
                    co++;
                    while (brojacunizu < nizkaraktera.Length && nizkaraktera[brojacunizu] != null)
                    {
                        if (pera == nizkaraktera[brojacunizu].kod)
                        {
                            sr.Write(nizkaraktera[brojacunizu].slovo);
                            pera = null;
                        }
                        brojacunizu++;
                    }
                    brojacunizu = 0;
                    counter++;
                }
            }


            FileInfo info1 = new FileInfo(ime);
            FileInfo info = new FileInfo(binarni);
            double k = (1 - ((double)info.Length / info1.Length)) * 100;

            Console.WriteLine("StepenKompresije je:" + k + "%");

        }
    }
    }


