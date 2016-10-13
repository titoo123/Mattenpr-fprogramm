using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Mattenprüfprogramm.Erweiterungen
{
    class CSV_Reader
    {
        private string path;

        List<Zug> p_List = new List<Zug>();
        public CSV_Reader(string path)
        {
            this.path = path;
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    Zug p = new Zug();

                    try
                    {

                        //if (((DateTime.Parse(fields[0]).ToShortDateString()) == (Convert.ToDateTime(p.Produktionsdatum).ToShortDateString()))
                        //    && fields[1] == "WR"
                        //    && fields[2] == p.Charge
                        //    && fields[4] == p.Bundnummer
                        //    )
                        //{
                        p.Datum = DateTime.Parse(fields[0]);
                        //1 unwichtig
                        p.Maschine = fields[2];
                        p.Mattentyp = fields[3];
                        //4 Durchmesser
                        p.H1m = Convert.ToDouble(fields[5]);
                        p.H2m = Convert.ToDouble(fields[6]);
                        p.H3m = Convert.ToDouble(fields[7]);
                        //p.a4m = Convert.ToDouble(fields[9]);
                        //10 Mittelwert
                        p.H125 = Convert.ToDouble(fields[9]);
                        p.H225 = Convert.ToDouble(fields[10]);
                        p.H325 = Convert.ToDouble(fields[11]);
                        //p.a4_025 = Convert.ToDouble(fields[14]);
                        //15 Mittelwert
                        p.H175 = Convert.ToDouble(fields[13]);
                        p.H275 = Convert.ToDouble(fields[14]);
                        p.H375 = Convert.ToDouble(fields[15]);
                        //p.a4_075 = Convert.ToDouble(fields[19]);
                        //20 Mittelwert
                        p.c1 = Convert.ToDouble(fields[17]);
                        p.c2 = Convert.ToDouble(fields[18]);
                        p.c3 = Convert.ToDouble(fields[19]);
                        //p.c4 = Convert.ToDouble(fields[24]);
                        //25 Mittelwert
                        p.se1 = Convert.ToDouble(fields[21]);
                        p.se2 = Convert.ToDouble(fields[22]);
                        p.se3 = Convert.ToDouble(fields[23]);
                        //p.se4 = Convert.ToDouble(fields[29]);
                        //30 Summe
                        //p.Beta = Convert.ToDouble(fields[36]);
                        //p.Alpha = Convert.ToDouble(fields[37]);
                        p.fR = Convert.ToDouble(fields[32]);
                        //}
                        p_List.Add(p);

                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        internal Zug Read(Zug p)
        {
            if (p != null)
            {
                foreach (Zug item in p_List)
                {
                    if (((Convert.ToDateTime(item.Datum).ToShortDateString()) == (Convert.ToDateTime(p.Datum).ToShortDateString()))
                    && p.Maschine.Replace("S", "").Replace(" ", "") == item.Maschine.Replace("S","").Replace(" ","")
                    && item.D == p.D
                    )
                    {
                        return item;
                    }
                }

            }

            return null;
        }
    }
}
