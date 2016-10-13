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
        
        private string xml_pfad;
        private string csv_pfad;

        private MainWindow m;

        private List<Scherung> sList = new List<Scherung>();
        private List<Zug> zList = new List<Zug>();

        public ImportLoader(MainWindow m)
        {
            this.m = m;
            try
            {
                xml.Load(Benutzer.BenutzerVerwaltung.xml_Pfad);
                xml_pfad = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
                csv_pfad = xml.SelectSingleNode("Einstellungen").SelectSingleNode("CSV_Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }
        }
        public void Load(ProgressBar progressBar)
        {
            int i = 0;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(xml_pfad);
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

            CSV_Load(csv_pfad);


            //Meldet ob Import fertig
            MessageBox.Show("Import von " + i + " Dateien abgeschlossen!","Information");

            //Setzt Balken auf 0
            progressBar.Value = 0;

            //Refresh Mainwindow
            m.label_infos_LoadData();
            m.dataGrid_matten_LoadData();
        }

        internal void CSV_Load(string csv_Importpfad)
        {
            //Ließt Rippendaten aus
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            System.IO.DirectoryInfo csv_directory = new System.IO.DirectoryInfo(csv_Importpfad);

            var fpr = from x in d.Zug
                      where x.fR == 0 && x.Datum > DateTime.Today - TimeSpan.FromDays(700)
                      select x;

            foreach (System.IO.FileInfo f in csv_directory.GetFiles())
            {
                if (f.Name.Contains(".csv"))
                {

                    CSV_Reader csv_Reader = new CSV_Reader(f.FullName);

                    foreach (Zug p in fpr)
                    {
                        if (p != null)
                        {
                            Zug np = csv_Reader.Read(p);
                            if (np != null)
                            {
                                p.Maschine = np.Maschine;
                                p.H1m = np.H1m;
                                p.H2m = np.H2m;
                                p.H3m = np.H3m;
                                //p.a4m = np.a4m;
                                //10 Mittelwert
                                p.H125 = np.H125;
                                p.H225 = np.H225;
                                p.H325 = np.H325;
                                //p.a4_025 = np.a4_025;
                                //15 Mittelwert
                                p.H175 = np.H175;
                                p.H275 = np.H275;
                                p.H375 = np.H375;
                                //p.a4_075 = np.a4_075;
                                //20 Mittelwert
                                p.c1 = np.c1;
                                p.c2 = np.c2;
                                p.c3 = np.c3;
                                //p.c4 = np.c4;
                                //25 Mittelwert
                                p.se1 = np.se1;
                                p.se2 = np.se2;
                                p.se3 = np.se3;
                                //p.se4 = np.se4;
                                //30 Summe
                                //p.Beta = np.Beta;
                                //p.Alpha = np.Alpha;
                                p.fR = np.fR;

                                d.SubmitChanges();
                            }
                        }

                    }
                }
            }
        }


    }
}
