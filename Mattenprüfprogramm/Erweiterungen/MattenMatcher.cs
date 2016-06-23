using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mattenprüfprogramm.Erweiterungen
{
    class MattenMatcher
    {
        List<Scherung> s = new List<Scherung>();
        List<Zug> z = new List<Zug>();

        List<Matten> m = new List<Matten>();

        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public MattenMatcher(List<Scherung> s) {

            foreach (Scherung se in s)
            {
                foreach (Matten ma in m)
                {
                    if (se.Prüfer == ma.Prüfer.Name && se.Maschine == ma.Schweissmaschine.Nummer && se.Datum == ma.Datum && se.Mattentyp == ma.Mattentypen.Name)
                    {
                        se.Id_Matten = ma.Id;
                        d.SubmitChanges();
                    }
                    else
                    {
                        var pru = from p in d.Prüfer
                                  where p.Name == se.Prüfer
                                  select p.Id;

                        var mas = from k in d.Schweissmaschine
                                  where k.Nummer == se.Maschine
                                  select k.Id;

                        var matt = from r in d.Mattentypen
                                   where r.Name == se.Mattentyp
                                   select r.Id;

                        Matten mat = new Matten() { Datum = Convert.ToDateTime( se.Datum ), Id_Prüfer = pru.First(), Id_Maschine = mas.First(), Id_Mattentyp =  matt.First()};
                        d.Matten.InsertOnSubmit(mat);
                        d.SubmitChanges();

                        var mate = from n in d.Matten
                                   where n.Datum == Convert.ToDateTime(se.Datum) && n.Id_Prüfer == pru.First() && n.Id_Maschine == mas.First() && n.Id_Mattentyp == matt.First()
                                   select n;

                        se.Id_Matten = mate.First().Id;
                        d.SubmitChanges();
                    }

                }
            }
        }
        public MattenMatcher(List<Zug> z) {

            foreach (Zug zu in z)
            {
                foreach (Matten ma in m)
                {
                    if (zu.Prüfer == ma.Prüfer.Name && zu.Maschine == ma.Schweissmaschine.Nummer && zu.Datum == zu.Datum && zu.Mattentyp == ma.Mattentypen.Name)
                    {
                        zu.Id_Matten = ma.Id;
                        d.SubmitChanges();
                    }
                    else
                    {
                        var pru = from p in d.Prüfer
                                  where p.Name == zu.Prüfer
                                  select p.Id;

                        var mas = from k in d.Schweissmaschine
                                  where k.Nummer == zu.Maschine
                                  select k.Id;

                        var matt = from r in d.Mattentypen
                                   where r.Name == zu.Mattentyp
                                   select r.Id;

                        Matten mat = new Matten() { Datum = Convert.ToDateTime(zu.Datum), Id_Prüfer = pru.First(), Id_Maschine = mas.First(), Id_Mattentyp = matt.First() };
                        d.Matten.InsertOnSubmit(mat);
                        d.SubmitChanges();

                        var mate = from n in d.Matten
                                   where n.Datum == Convert.ToDateTime(zu.Datum) && n.Id_Prüfer == pru.First() && n.Id_Maschine == mas.First() && n.Id_Mattentyp == matt.First()
                                   select n;

                        zu.Id_Matten = mate.First().Id;

                        d.SubmitChanges();
                    }

                }
            }
        }
    }
}
