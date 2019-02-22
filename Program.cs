using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu2
{
    class Program
    {
        static void Main(string[] args) {

            // TESZT
            Menu fm = new Menu();
            fm.felsoSzoveg = "Asd klsdaflksdf l\n lkasdwna nsajdn kjasdn \nlaskndolwnadlskdnl dslakjn lasn";
            MenuElem a = new MenuElem("asd");
            MenuElem b = new MenuElem("efg");
            MenuElem c = new MenuElem("xyz");
            MenuElem kilep = new MenuElem(() => fm.Bezar(), "kilépés");
            fm.HozzaadMenuElem(a);
            fm.HozzaadMenuElem(b);
            fm.HozzaadMenuElem(c);
            
            Menu m2 = new Menu();
            MenuElem vissza = new MenuElem(fm, "vissza");
            MenuElem me2 = new MenuElem(m2, "m2");
            m2.HozzaadMenuElem(vissza);
            fm.HozzaadMenuElem(me2);
            fm.HozzaadMenuElem(kilep);

            fm.Megnyit();
        }
    }

    public class Menu
    {
        private List<MenuElem> elemek = new List<MenuElem>();    // kiválasztható menüpontok
        public static ConsoleColor kijelolesSzin = ConsoleColor.Green;
        public string felsoSzoveg = "<Felso szoveg>";   // szöveg a kiválasztható menüpontok fölött
        public string alsoSzoveg = "<Also szoveg>"; // szöveg a kiválasztható menüpontok alatt
        private const string kijelolesSzov = " <";
        private bool fut;

        public void Megnyit() {
            fut = true;
            Console.Clear();
            Console.WriteLine(felsoSzoveg);
            int elsoSor = Console.CursorTop;    // innen kezdődnek a kilelölhető menüpontok
            int kijelolesIndex = 0; // épp kijelölt menüpont indexe
            ConsoleKeyInfo gomb;

            if (elemek.Count > 0)
                KiirSzines(elemek[0].szoveg + kijelolesSzov, kijelolesSzin);
            Console.WriteLine();
            for (int i = 1; i < elemek.Count; i++) {
                Console.WriteLine(elemek[i].szoveg);
            }
            Console.WriteLine(alsoSzoveg);

            Console.CursorTop = elsoSor;
            if (elemek.Count > 0)
                Console.CursorLeft = elemek[0].szoveg.Length + kijelolesSzov.Length;

            do {
                gomb = Console.ReadKey();
                if(gomb.Key == ConsoleKey.UpArrow && kijelolesIndex > 0) {  // ha lehet feljebb lépni
                    Console.CursorLeft = 0; // vissza a sor elejére
                    Console.Write(elemek[kijelolesIndex].szoveg + new string(' ', kijelolesSzov.Length));   // felülírjuk normál színnel, a végén a kijelölő karaktersorozatot space-ekkel
                    Console.CursorLeft = 0; // vissza a sor elejére
                    Console.CursorTop = elsoSor + --kijelolesIndex; // előbb csökkentjuk az indexet, majd odalépünk (eggyel feljebb)
                    KiirSzines(elemek[kijelolesIndex].szoveg + kijelolesSzov, kijelolesSzin);   // felülírjuk az új kijelölt sort színesen + hoozzáírjuk a kijelölő string-et
                } else if(gomb.Key == ConsoleKey.DownArrow && kijelolesIndex < elemek.Count - 1) { // ha lehet lejjebb lépni
                    Console.CursorLeft = 0;
                    Console.Write(elemek[kijelolesIndex].szoveg + new string(' ', kijelolesSzov.Length));
                    Console.CursorLeft = 0;
                    Console.CursorTop = elsoSor + ++kijelolesIndex;
                    KiirSzines(elemek[kijelolesIndex].szoveg + kijelolesSzov, kijelolesSzin);
                } else if(gomb.Key == ConsoleKey.Enter) {
                    //Console.WriteLine();
                    elemek[kijelolesIndex].Kivalaszt();
                }
            } while (fut);
        }

        public void Bezar() {
            fut = false;
        }

        public void HozzaadMenuElem(MenuElem elem) {
            elem.menu = this;
            elemek.Add(elem);
        }

        private void KiirSzines(object szoveg, ConsoleColor szin) {
            Console.ForegroundColor = szin;
            Console.Write(szoveg);
            Console.ResetColor();
        }
    }

    public class MenuElem
    {
        public Menu menu;  // Az a menu amiben ez az elem benne van
        public delegate void Feladat(); // egy olyan fgv típusa, ami nem tér vissza semmivel és nem is kér paramétert
        Feladat amikorKivalaszt;    // Vmilyen fgv, ami tartalmazza a menüpont kiválasztásakor végrehajtandó utasításokat
        public string szoveg;

        public MenuElem(string szov) {
            szoveg = szov;
        }

        public MenuElem(Feladat f, string szov) {
            amikorKivalaszt = f;
            szoveg = szov;
        }

        public MenuElem(Menu masikMenu, string szov) {  // gyakran a menüpont csak egy másik menübe vezet, ezért így kódot spórolunk
            szoveg = szov;
            amikorKivalaszt = () => {
                menu.Bezar();   // bezárja a saját menüjét
                masikMenu.Megnyit();    // megnyitja a másikat
            };
        }

        public void Kivalaszt() {
            if(amikorKivalaszt != null)
                amikorKivalaszt();  // meghivjuk a tárolt függvényt
        }
    }
}
