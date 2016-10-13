using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace Mattenprüfprogramm.Erweiterungen
{
    class XMLParser
    {
        private static string STRING_VALUE = "Value";
        private static string STRING_PARAMETER_TYPE = "ParameterType";

        String node_Name;       //Erster Knoten
        String file_Name;       //Filename

        String clean_file_Name; //...

        XmlDocument x = new XmlDocument();

        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public XMLParser(XmlDocument x, String node_Name, String file_Name)
        {
            this.node_Name = node_Name;
            this.file_Name = file_Name;
            this.x = x;

            clean_file_Name = file_Name.Substring(5);

        }
        public Zug ReadData_Zug()
        {
            Zug z = new Zug();

            XmlNodeList nList = x.SelectNodes(node_Name);

            if (nList.Count > 1)
            {
                foreach (XmlNode xNode in nList)
                {
                    string s = xNode[STRING_PARAMETER_TYPE].InnerText;

                    if (clean_file_Name.Contains("L") || clean_file_Name.Contains("Z"))
                    {
                        z.Richtung = "Längs";
                    }
                    switch (s)
                    {
                        case "51":  //Durchmesser
                            z.D = Parse_Double("Durchmesser", xNode, STRING_VALUE, file_Name, 0);
                            break;
                        case "163": //Dgs
                            z.D = Parse_Double("Dgs", xNode, STRING_VALUE, file_Name, 0);
                            break;
                        case "308": //Zugfestigkeit
                            z.Rm = Parse_Double("Zugfestigkeit", xNode, STRING_VALUE, file_Name, 0);
                            break;
                        case "257": //Streckgrenze
                            z.Rp = Parse_Double("Streckgrenze", xNode, STRING_VALUE, file_Name, 2);
                            break;

                        case "318":  //Agt
                            z.Agt = Parse_Double("Agt", xNode, STRING_VALUE, file_Name, 2);
                            break;
                        case "602":  //Bediener
                            string p = xNode[STRING_VALUE].InnerText;

                            var pru = from k in d.Prüfer
                                      where k.Name == p
                                      select k.Id;

                            if (pru.Count() == 0)
                            {
                                d.Prüfer.InsertOnSubmit(new Prüfer { Name = p });

                                try
                                {
                                    d.SubmitChanges();
                                }
                                catch (Exception)
                                {

                                }
                            }


                            z.Prüfer = p;

                            break;
                        case "607":  //Datum

                            try
                            {
                                z.Datum = DateTime.Parse(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein Datum in der XML-Datei: " + file_Name + " gefunden werden!", "Fehler!");
                            }

                            break;
                        case "599":  // Mattentyp
                            string v = xNode[STRING_VALUE].InnerText;

                            var mtt = from m in d.Mattentypen
                                      where m.Name == v
                                      select m;

                            if (mtt.Count() < 1)
                            {
                                d.Mattentypen.InsertOnSubmit(new Mattentypen() { Name = v });
                                try
                                {
                                    d.SubmitChanges();
                                }
                                catch (Exception)
                                {

                                }
                            }

                            z.Mattentyp = v;

                            break;
                        case "313": //Maschine
                            try
                            {
                                z.Maschine = xNode[STRING_VALUE].InnerText;
                                // Convert.ToInt32(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {

                            }

                            break;

                        default:
                            break;
                    }

                }
            }
            z.RmRp = z.Rm / z.Rp;

            return z;

        }
        public Scherung ReadData_Scherung()
        {
            Scherung sc = new Scherung();

            XmlNodeList nList = x.SelectNodes(node_Name);

            if (nList.Count > 1)
            {

                foreach (XmlNode xNode in nList)
                {
                    string s = xNode[STRING_PARAMETER_TYPE].InnerText;
                    switch (s)
                    {
                        case "190":  //Durchmesser
                            sc.D = Parse_Double("Durchmesser", xNode, STRING_VALUE, file_Name, 0);
                            break;
                        case "602":  //Bediener

                            string p = xNode[STRING_VALUE].InnerText;

                            var pru = from k in d.Prüfer
                                      where k.Name == p
                                      select k.Id;

                            if (pru.Count() == 0)
                            {
                                d.Prüfer.InsertOnSubmit(new Prüfer { Name = p });

                                try
                                {
                                    d.SubmitChanges();
                                }
                                catch (Exception)
                                {

                                }
                            }

                            sc.Prüfer = p;
                            break;
                        case "607":  //Datum

                            try
                            {
                                sc.Datum = DateTime.Parse(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein Datum in der XML-Datei: " + file_Name + " gefunden werden!", "Fehler!");
                            }

                            break;
                        case "143": //Scherkraft 
                            sc.Fm = Parse_Double("Scherkraft", xNode, STRING_VALUE, file_Name, 2);
                            break;
                        case "609":  //Scherfaktor 
                            sc.Sw = Parse_Double("Scherfaktor", xNode, STRING_VALUE, file_Name, 2);
                            break;
                        case "599":  // Mattentyp
                            string o = xNode[STRING_VALUE].InnerText;

                            var mtt = from m in d.Mattentypen
                                      where m.Name == o
                                      select m;

                            if (mtt.Count() < 1)
                            {
                                d.Mattentypen.InsertOnSubmit(new Mattentypen() { Name = o });
                                try
                                {
                                    d.SubmitChanges();
                                }
                                catch (Exception)
                                {

                                }
                            }

                            sc.Mattentyp = o;

                            break;
                        case "313": //Maschine
                            try
                            {
                                sc.Maschine = xNode[STRING_VALUE].InnerText;
                                //Convert.ToInt32(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {

                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            return sc;
        }




        /// <summary>
        /// Parst String-Werte zu Double-Werten
        /// </summary>
        /// <param name="valueName">Name das Parameters</param>
        /// <param name="node">Name des XML Knotens</param>
        /// <param name="subNode">Name das XML Unterknotens</param>
        /// <param name="dataName">Name der XML Datei</param>
        /// <returns></returns>
        private double Parse_Double(string valueName, XmlNode node, string subNode, string dataName, int digits)
        {
            double d = 0;

            if (Double.TryParse(node[subNode].InnerText.Replace('.', ','), out d))
            {
                return Math.Round(d, digits);
            }
            else
            {
                MessageBox.Show("Es konnte kein " + valueName + " in der XML-Datei: " + dataName + " gefunden werden!", "Fehler!");
                return 0;
            }


        }
    }
}
