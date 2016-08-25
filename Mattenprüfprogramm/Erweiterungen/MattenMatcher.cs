using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mattenprüfprogramm.Erweiterungen
{
    class MattenMatcher
    {
        //List<Scherung> s = new List<Scherung>();
        //List<Zug> z = new List<Zug>();

        List<Matten> m = new List<Matten>();

        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public MattenMatcher(List<Scherung> scherung_List) {

            foreach (Scherung scherung in scherung_List)
            {
                bool isInMatten = false;

                //Speichert Scherung in der Datenbank

                var pru = from p in d.Prüfer
                          where p.Name == scherung.Prüfer
                          select p.Id;

                var mas = from k in d.Schweissmaschine
                          where k.Nummer == scherung.Maschine
                          select k.Id;

                var matt = from r in d.Mattentypen
                           where r.Name == scherung.Mattentyp
                           select r.Id;

                if (pru.Count() == 0)
                {
                    d.Prüfer.InsertOnSubmit(new Prüfer { Name = scherung.Prüfer });
                }
                if (mas.Count() == 0)
                {
                    d.Schweissmaschine.InsertOnSubmit(new Schweissmaschine { Nummer = Convert.ToInt32(scherung.Maschine) });
                }
                if (matt.Count() == 0)
                {
                    d.Mattentypen.InsertOnSubmit(new Mattentypen { Name = scherung.Mattentyp });
                }

                d.Scherung.InsertOnSubmit(scherung);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }

                var mam = from a in d.Matten
                          where a.Datum == scherung.Datum   &&
                          a.Schweissmaschine.Nummer == scherung.Maschine   &&
                          a.Mattentypen.Name == scherung.Mattentyp &&
                          a.Prüfer.Name == scherung.Prüfer 
                          select a;

                int h = mam.Count();
                //Wenn passende Matten vorhanden dann verknüpfe Matte mit Scherung
                if (mam.Count() != 0)
                {
                    //scherung.Id_Matten = mam.First().Id;
                    //try
                    //{
                    //    d.SubmitChanges();
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
                else
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

                    Matten mat = new Matten() { Prüfnummer = 0, Datum = Convert.ToDateTime(scherung.Datum), Id_Prüfer = Convert.ToInt32( p_Id),
                        Id_Maschine = Convert.ToInt32(s_Id), Id_Mattentyp = Convert.ToInt32(m_Id) };

                    d.Matten.InsertOnSubmit(mat);
                    
                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }

                    scherung.Id_Matten = mat.Id;

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
        public MattenMatcher(List<Zug> z) {

            //foreach (Zug zu in z)
            //{
            //    foreach (Matten ma in m)
            //    {
            //        if (zu.Prüfer == ma.Prüfer.Name && zu.Maschine == ma.Schweissmaschine.Nummer && zu.Datum == zu.Datum && zu.Mattentyp == ma.Mattentypen.Name)
            //        {
            //            zu.Id_Matten = ma.Id;
            //            try
            //            {
            //                d.SubmitChanges();
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        }
            //        else
            //        {
            //            var pru = from p in d.Prüfer
            //                      where p.Name == zu.Prüfer
            //                      select p.Id;

            //            var mas = from k in d.Schweissmaschine
            //                      where k.Nummer == zu.Maschine
            //                      select k.Id;

            //            var matt = from r in d.Mattentypen
            //                       where r.Name == zu.Mattentyp
            //                       select r.Id;

            //            Matten mat = new Matten() { Datum = Convert.ToDateTime(zu.Datum), Id_Prüfer = pru.First(), Id_Maschine = mas.First(), Id_Mattentyp = matt.First() };
            //            d.Matten.InsertOnSubmit(mat);
            //            try
            //            {
            //                d.SubmitChanges();
            //            }
            //            catch (Exception)
            //            {
            //            }

            //            var mate = from n in d.Matten
            //                       where n.Datum == Convert.ToDateTime(zu.Datum) && n.Id_Prüfer == pru.First() && n.Id_Maschine == mas.First() && n.Id_Mattentyp == matt.First()
            //                       select n;

            //            zu.Id_Matten = mate.First().Id;

            //            try
            //            {
            //                d.SubmitChanges();
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        }

            //    }
            //}
        }
    }
}
