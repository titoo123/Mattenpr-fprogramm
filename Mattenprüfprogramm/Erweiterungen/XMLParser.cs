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

            XmlNodeList nList = x.SelectNodes(n1);

            if (nList.Count > 1)
            {
                Zug z = new Zug();
                Scherung sc = new Scherung();

                foreach (XmlNode xNode in nList)
                {
                    string s = xNode["ParameterType"].InnerText;

                    if (true)
                    {
                        switch (s)
                        {
                            case "51":  //Durchmesser
                                break;
                            case "529": //Bruchdehnung
                                break;
                            case "163": //Masse
                                break;
                            case "?":   //Richtung
                                break;
                            case "308": //Zugfestigkeit
                                break;
                            case "372": //Dehngrenze
                                break;
                            case "374":  //Dehnung bei Maximalwert Agt
                                break;
                            case "199":  //Bediener
                                break;
                            case "311":  //Datum
                                break;
                            case "243":  //R/R
                                break;
                            case "37":  //se1
                                break;
                            case "38":  //se2
                                break;
                            case "39":  //se3<ParameterType>39</ParameterType>
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (s)
                        {
                            case "51":  //Durchmesser
                                break;
                            case "?":   //Richtung
                                break;                       
                            case "199":  //Bediener
                                break;
                            case "311":  //Datum
                                break;
                            case "318":  //Datum
                                break;
                            default:
                                break;
                        }
                    }

                }
            }

            

        }
    }
}
