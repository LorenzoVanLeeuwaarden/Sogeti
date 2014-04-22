using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti
{
    class ProgrammeertalenParser
    {
       
        String Ptalen;

        public String toDisplay(List<String>Talen, List<String>Ervaring) {
            //Voeg talen en ervaring list samen tot een string die weergegeven kan worden in de GUI

            if (Talen.Count() == 0 && Ervaring.Count() == 0) {
                Ptalen = "";
            }

            for (int i = 0; i < Talen.Count; i++)
            {
                if (Talen[i] == "")
                {
                    Talen.RemoveAt(i);
                }
            }
            

            for (int i = 0; i < Talen.Count; i++)
            {
                
                Talen[i] = Talen[i].Replace("/", "");
                switch (Ervaring[i])
                {
                    case "0":
                        Talen[i] = Talen[i] + " / 5";
                        break;
                    case "1":
                        Talen[i] = Talen[i] + " / 10";
                        break;
                    case "5":
                        Talen[i] = Talen[i] + " / 9";
                        break;
                    case "10":
                        Talen[i] = Talen[i] + " / 8";
                        break;
                    case "20":
                        Talen[i] = Talen[i] + " / 7";
                        break;
                    case "30":
                        Talen[i] = Talen[i] + " / 6";
                        break;
                    case "40":
                        Talen[i] = Talen[i] + " / 5.5";
                        break;
                    case "Ongeldig":
                        Talen[i] = Talen[i] + " / 0";
                        break;
                    default:
                        Talen[i] = Talen[i] + " / 0";
                        break;
                }

                if (i == 0)
                {
                    Ptalen = "[" + Talen[i] + "]";
                }
                else
                {
                    Ptalen = Ptalen + ", [" + Talen[i] + "]";
                }
            }

            return Ptalen;
        }

        public String TalenToDB(String Talen) {
            //Haal de programmeertalen uit de opgegeven string zodat deze in de database geplaatst kan worden
            String newTalen="";

            Talen = Talen.Replace("[","");
            Talen = Talen.Replace("]", "");
            Talen = Talen.Replace(" ", "");
            Talen = Talen.Replace("/", ",");
            
            List<String> progtalen = Talen.Split(',').ToList();
            for (int i = 0; i < progtalen.Count; i += 2)
            {
                if (i == 0)
                {
                    newTalen = newTalen + progtalen[i];
                }
                else
                {
                    newTalen = newTalen + "," + progtalen[i];
                }
            }
            return newTalen;
        }

        public String TaalervaringToDB(String ervaring) {
            //Haal de ervaring uit de opgegeven string zodat deze in de database geplaatst kan worden
            String newErvaring = "";

            ervaring = ervaring.Replace("[", "");
            ervaring = ervaring.Replace("]", "");
            ervaring = ervaring.Replace(" ", "");
            ervaring = ervaring.Replace("/", ",");
            List<String> progErvaring = ervaring.Split(',').ToList();
            for (int i = 1; i < progErvaring.Count; i += 2)
            {

                if (i == 1)
                {
                    ervaring = newErvaring + progErvaring[i];
                }
                else
                {
                    ervaring = ervaring + "," + progErvaring[i];
                }
            }
            return ervaring;
        }

        public String changeErvaring(String ervaring) {

            String newErvaring = "";

            ervaring = ervaring.Replace("[", "");
            ervaring = ervaring.Replace("]", "");
            ervaring = ervaring.Replace("/", ",");
            ervaring = ervaring.Replace(" ", "");
            List<String> progErvaring = ervaring.Split(',').ToList();
            for (int i = 1; i < progErvaring.Count; i += 2)
            {
                switch (progErvaring[i])
                {
                    case "5":
                        progErvaring[i] = "0";
                        break;
                    case "5.5":
                        progErvaring[i] = "40";
                        break;
                    case "10":
                        progErvaring[i] = "1";
                        break;
                    case "9":
                        progErvaring[i] = "5";
                        break;
                    case "8":
                        progErvaring[i] = "10";
                        break;
                    case "7":
                        progErvaring[i] = "20";
                        break;
                    case "6":
                        progErvaring[i] = "30";
                        break;
                    default:
                        progErvaring[i] = "Ongeldig";
                        break;
                }

                if (i == 1)
                {
                    ervaring = newErvaring + progErvaring[i];
                }
                else
                {
                    ervaring = ervaring + "," + progErvaring[i];
                }
            }
            return ervaring;
        
        }


    }
}
