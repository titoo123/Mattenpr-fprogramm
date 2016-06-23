using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mattenprüfprogramm.Erweiterungen
{
    class DATParser
    {
        private string v; //pfad
        private string n; //name
        private string l; //line
        private string f; //fehler

        Scherung s = new Scherung();
        Zug z = new Zug();

        Matten m = new Matten();

        public Scherung S
        {
            get
            {
                return s;
            }
        }
        public Zug Z
        {
            get
            {
                return z;
            }
        }

        public DATParser(string v, string n)
        {
            this.n = n;
            this.v = v;

            System.IO.StreamReader file = new System.IO.StreamReader(v, System.Text.Encoding.Default);

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();



            while ((l = file.ReadLine()) != null)
            {
                string[] p = l.Split(';');
                
                try
                {
                    string t = p[3];

                    if (n.Length == 9)//IF Scherung
                    {
                        switch (p[0])
                        {
                            case "601": //Höchstkraft

                                try
                                {
                                    s.Höchstkraft = Convert.ToDouble(t);

                                }
                                catch (Exception)
                                {
                                    f = "601";
                                }
                                break;
                            case "635": //Scherwert

                                try
                                {
                                    s.Scherwert = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "635";
                                }
                                break;
                            case "106": //Datum

                                try
                                {
                                    s.Datum = Convert.ToDateTime(t);
                                }
                                catch (Exception)
                                {
                                    f = "106";
                                }
                                break;
                            case "816": //Maschine
                                try
                                {
                                    z.Maschine = Convert.ToInt32(t.Substring(1));
                                }
                                catch (Exception)
                                {
                                    f = "816";
                                }
                                break;
                            case "815": //Mattentyp
                                try
                                {
                                    z.Mattentyp = t;
                                }
                                catch (Exception)
                                {
                                    f = "815";
                                }
                                break;
                            case "101": //Prüfer

                                try
                                {
                                    z.Prüfer = t;
                                }
                                catch (Exception)
                                {
                                    f = "101";
                                }


                                break;
                            case "205": //Durchmesser

                                try
                                {
                                    s.Durchmesser = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "205";
                                }
                                break;
                            case "218": //Richtung

                                try
                                {
                                    s.Richtung = t;
                                }
                                catch (Exception)
                                {
                                    f = "218";
                                }
                                break;
                            default:
                                f = String.Empty;
                                break;
                        }

                    }
                    else if (n.Length < 8)//IF Zug
                    {
                        switch (p[0])
                        {
                            case "603": //Zugfestigkeit

                                try
                                {
                                    z.Zugfestigkeit = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "603";
                                }
                                break;
                            case "622": //Dehngrenze

                                try
                                {
                                    z.Dehngrenze = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "622";
                                }
                                break;
                            case "639": //GD Höchstkraft

                                try
                                {
                                    z.DehnungHöchstkraft = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "639";
                                }
                                break;
                            case "664": //Bruchdehnung
                                try
                                {
                                    z.Bruchdehnung = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "664";
                                }

                                break;
                            case "665": //Streckgrenzverhältnis
                                try
                                {
                                    z.Streckengrenzverhältnis = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "665";
                                }

                                break;
                            //case "800": //Bruchlage
                            //    z.Bruchlage = t;
                            //    break;
                            //case "801": //Se1
                            //    z.Se1 = Convert.ToDouble(t);
                            //    break;
                            //case "802": //Se2
                            //    z.Se2 = Convert.ToDouble(t);
                            //    break;
                            //case "803": //Se3
                            //    z.Se3 = Convert.ToDouble(t);
                            //    break;
                            //case "804": //H1m
                            //    z.H1m = Convert.ToDouble(t);
                            //    break;
                            //case "805": //H2m
                            //    z.H2m = Convert.ToDouble(t);
                            //    break;
                            //case "806": //H3m
                            //    z.H3m = Convert.ToDouble(t);
                            //    break;
                            //case "807": //H125
                            //    z.H125 = Convert.ToDouble(t);
                            //    break;
                            //case "808": //H225
                            //    z.H225 = Convert.ToDouble(t);
                            //    break;
                            //case "809": //H325
                            //    z.H325 = Convert.ToDouble(t);
                            //    break;
                            //case "810": //H175
                            //    z.H175 = Convert.ToDouble(t);
                            //    break;
                            //case "811": //H275
                            //    z.H275 = Convert.ToDouble(t);
                            //    break;
                            //case "812": //H375
                            //    z.H375 = Convert.ToDouble(t);
                            //    break;
                            //case "813": //C
                            //    z.C = Convert.ToDouble(t);
                            //    break;
                            //case "814": //Fr
                            //    z.Fr = Convert.ToDouble(t);
                            //    break;
                            case "106": //Datum

                                try
                                {
                                    z.Datum = Convert.ToDateTime(t);
                                }
                                catch (Exception)
                                {
                                    f = "106";
                                }
                                break;
                            case "800":  //Maschine
                                try
                                {
                                   z.Maschine = Convert.ToInt32(t.Substring(1));
                                }
                                catch (Exception)
                                {
                                    f = "800";
                                }
                                break;
                                
                            case "801":  //Mattentyp
                                try
                                {
                                    z.Mattentyp = t;
                                }
                                catch (Exception)
                                {
                                    f = "801";
                                }
                                break;
                            case "101":  //Prüfer

                                try
                                {
                                    z.Prüfer = t;
                                }
                                catch (Exception)
                                {
                                    f = "101";
                                }


                                break;
                            case "205": //Durchmesser

                                try
                                {
                                    z.Durchmesser = Convert.ToDouble(t);
                                }
                                catch (Exception)
                                {
                                    f = "205";
                                }
                                break;
                            case "218": //Richtung

                                try
                                {
                                    z.Richtung = t;
                                }
                                catch (Exception)
                                {
                                    f = "218";
                                }
                                break;
                            default:
                                f = String.Empty;
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    if (f.Length > 1)
                    {
                        System.Windows.MessageBox.Show("Datei beschädigt! Bitte Zeilen auf vollständigkeit prüfen!\nZeile: " + f, "Fehler!");
                    }

                }

            }

            file.Close();
        }


    }
}
