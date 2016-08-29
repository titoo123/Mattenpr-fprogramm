﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;


namespace Mattenprüfprogramm.Erweiterungen
{
    class ImportLoader
    {
        private XmlDocument xml = new XmlDocument();
        
        private string pfad;
        private MainWindow m;

        private List<Scherung> sList = new List<Scherung>();
        private List<Zug> zList = new List<Zug>();

        public ImportLoader(MainWindow m)
        {
            this.m = m;
            try
            {
                xml.Load(Benutzer.BenutzerVerwaltung.xml_Pfad);
                pfad = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }
        }
        public void Load(ProgressBar progressBar)
        {
            int i = 0;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(pfad);
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            
            //Setzt PBar Werte auf Dateienanzahl
            progressBar.Maximum = directory.GetFiles().Count();

            foreach (System.IO.FileInfo f in directory.GetFiles())
            {
                var s = from h in d.Import
                        where h.Name == f.Name
                        select h;

                if (s.Count() < 1)
                {
                    if (f.Name.Contains(".xml"))
                    {
                        xml.Load(f.FullName);
                        try
                        {
                            XMLParser x = new XMLParser(xml, "TestParameters/TestParameter", f.Name);

                            if (f.Name.Substring(5).Contains("S"))
                            {
                                Scherung ls = x.ReadData_Scherung();

                                if (s != null )
                                {
                                    sList.Add(ls);
                                }

                            }
                            else
                            {
                                Zug lz = x.ReadData_Zug();

                                if (lz != null)
                                {
                                    zList.Add(lz);
                                }

                            }
                            i++;
                            d.Import.InsertOnSubmit(new Import() { Name = f.Name });
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Import der Datei " + f.Name + " fehlgeschlagen!\n\n" + e, "Fehler!");
                        }
                    }
                    else if (f.Name.Contains(".dat"))
                    {
                       
                    }

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
                    }

                }
                progressBar.Value++;
            }

            //Erzeugt speichert Matten
            new MattenMatcher(zList);
            new MattenMatcher(sList);


            //Meldet ob Import fertig
            MessageBox.Show("Import von " + i + " Dateien abgeschlossen!","Information");

            //Setzt Balken auf 0
            progressBar.Value = 0;

            //Refresh Mainwindow
            m.label_infos_LoadData();
            m.dataGrid_matten_LoadData();
        }
    }
}
