using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mattenprüfprogramm.Erweiterungen
{
    class MattenMatcher
    {

       // List<Matten> m = new List<Matten>();

        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public MattenMatcher(List<Scherung> scherung_List)
        {
            foreach (Scherung scherung in scherung_List)
            {
                //Speichert Scherung in der Datenbank

                d.Scherung.InsertOnSubmit(scherung);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }

                var mte = from a in d.Matten
                          where
                          a.Datum.Day == scherung.Datum.Value.Day &&
                          a.Datum.Month == scherung.Datum.Value.Month &&
                          a.Datum.Year == scherung.Datum.Value.Year &&
                          //a.Datum.Hour == scherung.Datum.Value.Hour
                          //&&

                          a.Schweissmaschine.Nummer == Convert.ToInt32(scherung.Maschine) &&
                          a.Mattentypen.Name == scherung.Mattentyp &&
                          a.Prüfer.Name == scherung.Prüfer

                          select a;

              

                //Wenn passende Matten vorhanden dann verknüpfe Matte mit Scherung
                if (mte.Count() == 0)
                {
                    int? p_Id = (from p in d.Prüfer
                                 where p.Name == scherung.Prüfer
                                 select p).First().Id;

                    int? s_Id = (from k in d.Schweissmaschine
                                 where k.Nummer == scherung.Maschine
                                 select k).First().Id;

                    int? m_Id = (from r in d.Mattentypen
                                 where r.Name == scherung.Mattentyp
                                 select r).First().Id;

                    Matten mat = new Matten()
                    {
                        Prüfnummer = 0,
                        Datum = Convert.ToDateTime(scherung.Datum),
                        Id_Prüfer = Convert.ToInt32(p_Id),
                        Id_Maschine = Convert.ToInt32(s_Id),
                        Id_Mattentyp = Convert.ToInt32(m_Id)
                    };

                    d.Matten.InsertOnSubmit(mat);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }

                    scherung.Id_Matten = mat.Id;



                }
                else
                {
                    if (mte.Count() > 0)
                    {
                        scherung.Id_Matten = mte.First().Id;
                    }

                }


                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }





            }
        }
        public MattenMatcher(List<Zug> zug_List)
        {

            foreach (Zug zug in zug_List)
            {
                //Speichert Zugs in der Datenbank

                d.Zug.InsertOnSubmit(zug);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }

                var mte = from a in d.Matten
                          where
                          a.Datum.Day == zug.Datum.Value.Day &&
                          a.Datum.Month == zug.Datum.Value.Month &&
                          a.Datum.Year == zug.Datum.Value.Year &&
                          //a.Datum.Hour == scherung.Datum.Value.Hour
                          //&&

                          a.Schweissmaschine.Nummer == Convert.ToInt32(zug.Maschine) &&
                          a.Mattentypen.Name == zug.Mattentyp &&
                          a.Prüfer.Name == zug.Prüfer

                          select a;


                //Wenn passende Matten vorhanden dann verknüpfe Matte mit Scherung
                if (mte.Count() == 0)
                {
                    int? p_Id = (from p in d.Prüfer
                                 where p.Name == zug.Prüfer
                                 select p).First().Id;

                    int? s_Id = (from k in d.Schweissmaschine
                                 where k.Nummer == zug.Maschine
                                 select k).First().Id;

                    int? m_Id = (from r in d.Mattentypen
                                 where r.Name == zug.Mattentyp
                                 select r).First().Id;

                    Matten mat = new Matten()
                    {
                        Prüfnummer = 0,
                        Datum = Convert.ToDateTime(zug.Datum),
                        Id_Prüfer = Convert.ToInt32(p_Id),
                        Id_Maschine = Convert.ToInt32(s_Id),
                        Id_Mattentyp = Convert.ToInt32(m_Id)
                    };

                    d.Matten.InsertOnSubmit(mat);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }

                    zug.Id_Matten = mat.Id;



                }
                else
                {
                    if (mte.Count() > 0)
                    {
                        zug.Id_Matten = mte.First().Id;
                    }

                }


                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }

            }
        }
    }
}
