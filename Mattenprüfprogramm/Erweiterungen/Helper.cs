using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace Mattenprüfprogramm.Erweiterungen
{
    class Helper
    {
        public static string GetStringFromDataGrid(int l, DataGrid g) {

            try
            {
                return "" + ((TextBlock)g.Columns[l].GetCellContent(g.SelectedItem)).Text;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static int GetIntFromDataGrid(int l, DataGrid g){
            return Convert.ToInt32(GetStringFromDataGrid(l, g));
        }
        public static double GetDoubleFromDataGrid(int l, DataGrid g){
            return Convert.ToDouble(GetStringFromDataGrid(l, g));
        }

        public static ComboBox LoadPrüfer( ComboBox c ) {

            //Läd Standarddaten
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            //Prüfer
            var pru = from p in db.Prüfer
                      select new { p.Name };
            List<String> pList = new List<string>();
            foreach (var item in pru)
            {
                pList.Add(item.Name);
            }
            c.ItemsSource = pList;

            return c;
        }
        public static ComboBox LoadMattentypen(ComboBox c)
        {

            //Läd Standarddaten
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            //Prüfer
            var mat = from p in db.Mattentypen
                      select new { p.Name };
            List<String> rList = new List<string>();
            foreach (var item in mat)
            {
                rList.Add(item.Name);
            }
            c.ItemsSource = rList;

            return c;
        }
        public static ComboBox LoadSchweissmaschine(ComboBox c)
        {

            //Läd Standarddaten
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            //Prüfer
            var mas = from a in db.Schweissmaschine
                      select new { a.Name };
            List<String> mList = new List<string>();
            foreach (var item in mas)
            {
                mList.Add(item.Name);
            }
            c.ItemsSource = mList;

            return c;
        }

        public static int GetPrüferIdByName(String n) {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Prüfer
                      where p.Name == n
                      select p;
            
            return pru.First().Id;
        }
        public static int GetSchweissmaschinenIdByName(String n)
        {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Schweissmaschine
                      where p.Name == n 
                      select p;

            return pru.First().Id;
        }
        public static int GetMattentypIdByName(String n)
        {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Mattentypen
                      where p.Name == n
                      select p;

            return pru.First().Id;
        }

        public static Matten GetMatteByDataGrid(int l, DataGrid g)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from m in db.Matten
                      where m.Id == GetIntFromDataGrid(l, g)
                      select m;
            return mat.First();
        }
        public static List<Matten> GetMattenByDataGrid(int l, DataGrid g)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from m in db.Matten
                      where m.Id == GetIntFromDataGrid(l, g)
                      select m;
            return mat.ToList();
        }

      
    }
}
