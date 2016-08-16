using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mattenprüfprogramm.Erweiterungen
{
    class XMLParser
    {
        String n1;  //Erster Knoten
        String a;   //Art
        String n;   //Filename


        Scherung s = new Scherung();
        Zug z = new Zug();

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

        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public XMLParser(XmlDocument x,String n1, String n) {

            this.n1 = n1;
            this.n = n;

            a = x.SelectSingleNode(n1).SelectSingleNode("Art").InnerText;

            if (a == "zug")
            {
                try
                {
                    Zug z = new Zug()
                    {
                        D = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Durchmesser").InnerText),
                        Richtung = x.SelectSingleNode(n1).SelectSingleNode("Richtung").InnerText,
                        M = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        Rm = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("zugfestigkeit").InnerText),
                        Rp = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Dehngrenze").InnerText),
                        Agt = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("DehnungHöchstkraft").InnerText),
                        A = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Bruchdehnung").InnerText),
                        RmRp = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Streckengrenzverhältnis").InnerText),
                        Dgs = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("AbweichungNenngewicht").InnerText),
                        Br = x.SelectSingleNode(n1).SelectSingleNode("Bruchlage").InnerText,

                        se1 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        se2 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        se3 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H1m = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H2m = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H3m = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H125 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H225 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H325 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H175 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H275 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        H375 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        c1 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        c2 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        c3 = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText),
                        fR = Convert.ToDouble(x.SelectSingleNode(n1).SelectSingleNode("Masse").InnerText)


                    };
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Datei: " + n + "ist wohl möglich beschädigt! ", "Fehler!");
                }
              

            }
            else if (a == "scherung")
            {
                try
                {
                    Scherung s = new Scherung()
                    {
                        D = Convert.ToInt32(x.SelectSingleNode(n1).SelectSingleNode("Durchmesser").InnerText),
                        Richtung = x.SelectSingleNode(n1).SelectSingleNode("Richtung").InnerText,
                        Fm = Convert.ToInt32(x.SelectSingleNode(n1).SelectSingleNode("Höchstkraft").InnerText),
                        Sw = Convert.ToInt32(x.SelectSingleNode(n1).SelectSingleNode("Scherwert").InnerText)

                    };
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Datei: " + n + "ist wohl möglich beschädigt! ", "Fehler!");
                }
                

            }

        }
    }
}
