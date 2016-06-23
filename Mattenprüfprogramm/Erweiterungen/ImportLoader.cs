using System;
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

        private MattenMatcher mmz;
        private MattenMatcher mms;

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
                            // XMLParser x = new XMLParser(xml, "Testknoten", f.Name);

                            //if (x.S != null)
                            //{
                            //    sList.Add(x.S);
                            //}
                            //if (x.Z != null)
                            //{
                            //    zList.Add(x.Z);
                            //}

                            // d.Import.InsertOnSubmit(new Import() { Name = f.Name });
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Import der Datei " + f.Name + " fehlgeschlagen!\n\n" + e, "Fehler!");
                        }
                    }
                    else if (f.Name.Contains(".dat"))
                    {
                        try
                        {
                            DATParser p = new DATParser( f.FullName, f.Name);

                            if (p.S != null)
                            {
                                sList.Add(p.S);
                            }
                            if (p.Z != null)
                            {
                                zList.Add(p.Z);
                            }

                            d.Import.InsertOnSubmit(new Import() { Name = f.Name });
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Import der Datei " + f.Name + " fehlgeschlagen!\n\n" + e, "Fehler!");
                        }
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
            mmz = new MattenMatcher(zList);
            mms = new MattenMatcher(sList);

            //Setzt Balken auf 0
            progressBar.Value = 0;

            //Meldet ob Import fertig
            MessageBox.Show("Import abgeschlossen!","Information");

            //Refresh Mainwindow
            m.label_infos_LoadData();
        }
    }
}
