using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ingatlanok
{
    internal class Program
    {
        #region adatstrukturak
        struct zenestruct
        {
            public string eloado;
            public string cim;
            public int szavazatSzam;
        }
        #endregion

        #region program adatai
        static List<zenestruct> Zenek = new List<zenestruct>();
        #endregion

        #region modulvaltozok
        static List<int> cursorTops = new List<int>();//a kurzol ideiglenes pozitciojanak valtozoja
        static int maxview = 4;//a táblalista maximálisan egyszerre láthatő elemeinek száma
        static List<string> strings = new List<string>();//a tablalista számára befogadható adatlista melyet a táblabeviteli lista előtt adunk adatot illetve változtatjuk
        static int opcio = 0;//a táblalista által visszadott szám, mely annak az adatlisat elemindexet adja vissza amely adatlistából a "strings" lista készült. 
        #endregion

        static void Main(string[] args)//fő program
        {
            filekezeles("");
            filekezeles("load");
            menu();
        }


        static void menu()//menu
        {
            ConsoleKey consoleKey = default;
            while (consoleKey != ConsoleKey.Escape)
            {
                Console.Clear();
                cursorTops.Clear();
                Console.WriteLine("Fő Menü (Kilépés: Esc)  (Kiválasztás: Enter)");
                cursorTops.Add(Console.CursorTop); Console.WriteLine("( ) zene felvitele");
                cursorTops.Add(Console.CursorTop); Console.WriteLine("( ) TOP 10");
                cursorTops.Add(Console.CursorTop); Console.WriteLine("( ) szavazás");
                consoleKey = mozgas(consoleKey);
                if (consoleKey == ConsoleKey.Enter && cursorTops[0] == Console.CursorTop)//zene felvitele
                {
                    Console.Clear();
                    cursorTops.Clear();
                    zeneNew(zene_adatbekeres("eloado-cim"));
                }
                else if (consoleKey == ConsoleKey.Enter && cursorTops[1] == Console.CursorTop)//top 10
                {
                    Console.Clear();
                    cursorTops.Clear();
                    Console.WriteLine("TOP 10 (Kilépés: Esc)  (Kiválasztás: Enter)");
                    strings.Clear();
                    for (int i = 0; i < Zenek.Count && i < 10; i++)
                    {
                        strings.Add(Zenek[i].eloado + ";" + Zenek[i].cim + ";" + Zenek[i].szavazatSzam);
                    }
                    strings = szamrend(strings, "zene", "szavazat");
                    for (int i = 0; i < strings.Count; i++)
                    {
                        strings[i] = (i+1)+".  "+strings[i];
                    }
                    opcio = TablaLista(strings, consoleKey, "zene");
                    consoleKey = default;
                }
                else if (consoleKey == ConsoleKey.Enter && cursorTops[2] == Console.CursorTop)//szavazás
                {


                    Console.Clear();
                    cursorTops.Clear();
                    Console.WriteLine("szavazz a zenékre (Kilépés: Esc)  (Kiválasztás: Enter)");
                    strings.Clear();
                    for (int i = 0; i < Zenek.Count; i++)
                    {
                        strings.Add(Zenek[i].eloado + ";" + Zenek[i].cim + ";" + Zenek[i].szavazatSzam);
                    }
                    consoleKey = default;
                    opcio = TablaLista(strings, consoleKey, "zene");
                    for (int i = 0; i < Zenek.Count; i++)
                    {
                        if (i == opcio)
                        {
                            zenestruct ideigleneszene = new zenestruct();
                            ideigleneszene.cim = Zenek[opcio].cim;
                            ideigleneszene.eloado = Zenek[opcio].eloado;
                            ideigleneszene.szavazatSzam = Zenek[opcio].szavazatSzam + 1;
                            Zenek[opcio] = ideigleneszene;
                            filekezeles("save");
                        }
                    }



                }

            }
            consoleKey = default;
        }
        static int TablaLista(List<string> adatok, ConsoleKey consoleKey, string tipus)//táblalista(egy string lista amely a kiíratandó adatokat tartalmazza  ,  tartalmazza a consolekey-t  ,  egy tipust amely azt határozza meg , hogy mien adatokkal dolgozik és azon tipus alapján zajlódjanak a műveletek)
        {
            int pozitcio = 0;
            int pozitcio2 = 0;
            consoleKey = default;
            int kurzolpozicio = Console.CursorTop;
            int[] kozok = new int[adatok[0].Split(';').Length];
            for (int i = 0; i < kozok.Length; i++)
            {
                for (int j = 0; j < adatok.Count; j++)
                {
                    if (kozok[i] < adatok[j].Split(';')[i].Length)
                    {
                        kozok[i] = adatok[j].Split(';')[i].Length;
                    }
                }
            }
            while (consoleKey != ConsoleKey.Enter && consoleKey != ConsoleKey.Escape)
            {
                cursorTops.Clear();
                Console.SetCursorPosition(0, kurzolpozicio);
                Console.WriteLine("---");
                for (int i = pozitcio2; i < adatok.Count && i < pozitcio2 + maxview; i++)
                {
                    Console.WriteLine(hossz(80, 0, " "));
                }
                Console.SetCursorPosition(0, kurzolpozicio);

                Console.WriteLine("---");
                for (int i = pozitcio2; i < adatok.Count && i < pozitcio2 + maxview; i++)
                {
                    if (cursorTops.Count == pozitcio)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (tipus == "zene")
                    {
                        cursorTops.Add(Console.CursorTop); Console.WriteLine("( ) " + adatok[i].Split(';')[0] + hossz(kozok[0]+5, adatok[i].Split(';')[0].Length, " ") + adatok[i].Split(';')[1] + hossz(kozok[1]+5, adatok[i].Split(';')[1].Length, " ") + adatok[i].Split(';')[2]);
                    }
                    else
                    {
                        cursorTops.Add(Console.CursorTop); Console.WriteLine("( ) " + adatok[i]);
                    }
                    if (cursorTops.Count - 1 == pozitcio)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }
                Console.WriteLine("---");

                Console.SetCursorPosition(1, cursorTops[pozitcio]);
                consoleKey = Console.ReadKey().Key;

                switch (consoleKey)
                {
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop != cursorTops[0])
                        {
                            pozitcio--;
                        }
                        else if (pozitcio2 > 0)
                        {
                            pozitcio2--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop != cursorTops[cursorTops.Count - 1])
                        {
                            pozitcio++;
                        }
                        else if (pozitcio2 < adatok.Count - maxview)
                        {
                            pozitcio2++;
                        }
                        break;
                    case ConsoleKey.O:

                        adatok = szamrend(adatok, tipus, "szavazat");

                        break;
                    default:
                        break;
                }
            }
            return pozitcio2 + pozitcio;
        }
        static List<string> szamrend(List<string> szoveg, string tipus, string rendezes)
        {

            if (tipus == "zene")
            {
                List<zenestruct> struktura = new List<zenestruct>();
                for (int i = 0; i < szoveg.Count; i++)
                {
                    zenestruct ideigleneszene = new zenestruct();
                    ideigleneszene.eloado = szoveg[i].Split(';')[0];
                    ideigleneszene.cim = szoveg[i].Split(';')[1];
                    ideigleneszene.szavazatSzam = int.Parse(szoveg[i].Split(';')[2]);
                    struktura.Add(ideigleneszene);
                }
                for (int i = 0; i < struktura.Count; i++)
                {
                    int van = -1;
                    for (int j = i; j >= 0; j--)
                    {
                        if (rendezes == "szavazat")
                        {
                            if (struktura[i].szavazatSzam > struktura[j].szavazatSzam)
                            {
                                van = j;
                            }
                        }


                    }
                    if (i != 0 && van != -1)
                    {
                        zenestruct ideiglenes = struktura[i];
                        struktura.Remove(struktura[i]);
                        struktura.Insert(van, ideiglenes);
                    }

                }
                for (int i = 0; i < struktura.Count; i++)
                {
                    szoveg[i] = "" + struktura[i].eloado + ";" + struktura[i].cim + ";" + struktura[i].szavazatSzam;
                }
            }

            return szoveg;
        }
        static ConsoleKey mozgas(ConsoleKey consoleKey)
        {
            consoleKey = default;
            Console.SetCursorPosition(1, cursorTops[0]);
            while (consoleKey != ConsoleKey.Enter && consoleKey != ConsoleKey.Escape)
            {
                consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop != cursorTops[0])
                        {
                            Console.SetCursorPosition(1, cursorTops[cursorTops.IndexOf(Console.CursorTop) - 1]);
                        }
                        else
                        {
                            Console.SetCursorPosition(1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop != cursorTops[cursorTops.Count - 1])
                        {
                            Console.SetCursorPosition(1, cursorTops[cursorTops.IndexOf(Console.CursorTop) + 1]);
                        }
                        else
                        {
                            Console.SetCursorPosition(1, Console.CursorTop);
                        }
                        break;

                    case ConsoleKey.W:
                        if (Console.CursorTop != cursorTops[0])
                        {
                            Console.SetCursorPosition(1, cursorTops[cursorTops.IndexOf(Console.CursorTop) - 1]);
                        }
                        else
                        {
                            Console.SetCursorPosition(1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.S:
                        if (Console.CursorTop != cursorTops[cursorTops.Count - 1])
                        {
                            Console.SetCursorPosition(1, cursorTops[cursorTops.IndexOf(Console.CursorTop) + 1]);
                        }
                        else
                        {
                            Console.SetCursorPosition(1, Console.CursorTop);
                        }
                        break;
                    default:
                        break;
                }
            }
            return consoleKey;
        }
        static string hossz(int hosz, int kivonando, string kitoltes)
        {
            string vege = "";
            int kulombseg = 0;
            if (hosz > kivonando)
            {
                kulombseg = hosz - kivonando;
            }
            else
            {
                kulombseg = kivonando - hosz;
            }
            for (int i = 0; i < kulombseg; i++)
            {
                vege += kitoltes;
            }
            return vege;
        }
        static zenestruct zene_adatbekeres(string parancs)
        {
            zenestruct ugyfel = new zenestruct();
            if (parancs.Contains("szavazat"))
            {
                Console.WriteLine("adja meg a szavazatszámot");
                ugyfel.szavazatSzam = int.Parse(Console.ReadLine());
            }
            else
            {
                ugyfel.szavazatSzam = 0;
            }
            if (parancs.Contains("eloado"))
            {
                Console.WriteLine("adja meg az előadót");
                ugyfel.eloado = Console.ReadLine();
            }
            if (parancs.Contains("cim"))
            {
                Console.WriteLine("adja meg a zene címét");
                ugyfel.cim = Console.ReadLine();
            }
            return ugyfel;
        }
        static void zeneDelete(zenestruct zene_delete)
        {
            for (int i = 0; i < Zenek.Count; i++)
            {
                if (Zenek[i].cim == zene_delete.cim && Zenek[i].eloado == zene_delete.eloado)
                {
                    Zenek.RemoveAt(i);
                }
            }
            filekezeles("save");
        }
        static void zeneNew(zenestruct ugyfel_new)
        {
            Zenek.Add(ugyfel_new);
            filekezeles("save");
        }

        static void filekezeles(string parancs)
        {
            Console.WriteLine("mentés");
            if (parancs == "save")
            {
                StreamWriter streamWriter_Zene = new StreamWriter("zenek.txt");
                Console.WriteLine("zene mentése");
                for (int i = 0; i < Zenek.Count; i++)
                {
                    Console.WriteLine(i);
                    streamWriter_Zene.WriteLine(Zenek[i].eloado + "\t" + Zenek[i].cim + "\t" + Zenek[i].szavazatSzam);
                }
                streamWriter_Zene.Close();
            }
            else if (parancs == "load")
            {
                string[] allomanyUgyfel = File.ReadAllLines("zenek.txt");

                for (int i = 0; i < allomanyUgyfel.Length; i++)
                {
                    zenestruct zene = new zenestruct();
                    zene.eloado = allomanyUgyfel[i].Split('\t')[0];
                    zene.cim = allomanyUgyfel[i].Split('\t')[1];
                    zene.szavazatSzam = int.Parse(allomanyUgyfel[i].Split('\t')[2]);
                    Zenek.Add(zene);
                }
            }
            else
            {
               
                if (!File.Exists("zenek.txt"))
                {
                    StreamWriter streamWriter = new StreamWriter("zenek.txt");
                    streamWriter.Close();
                }
             
            }
        }
    }
}