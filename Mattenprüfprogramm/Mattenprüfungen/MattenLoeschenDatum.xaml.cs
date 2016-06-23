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

namespace Mattenprüfprogramm.Mattenprüfungen
{
    /// <summary>
    /// Interaktionslogik für MattenLoeschenDatum.xaml
    /// </summary>
    public partial class MattenLoeschenDatum : Window
    {
        MainWindow m;
        public MattenLoeschenDatum(MainWindow m)
        {
            InitializeComponent();
            this.m = m;
        }

        private void button_LuS_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker_bis != null && datePicker_bis != null && datePicker_von.SelectedDate < datePicker_bis.SelectedDate)
	        {
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
                var dat = from a in d.Matten
                          where a.Datum < Convert.ToDateTime(datePicker_bis) && a.Datum > Convert.ToDateTime(datePicker_von)
                          select a;

                DeleteSZ(dat.ToList(), d);

                
                d.Matten.DeleteAllOnSubmit(dat);
                d.SubmitChanges();

                m.dataGrid_matten_LoadData();
                this.Close();
	        }
            else if(datePicker_von != null && datePicker_bis == null)
	        {
                MessageBox.Show("Es werden nur Matten dieses Datums gelöscht!", "Nee!");
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
                var dat = from a in d.Matten
                          where a.Datum == Convert.ToDateTime(datePicker_bis) 
                          select a;

                DeleteSZ(dat.ToList(), d);


                d.Matten.DeleteAllOnSubmit(dat);
                d.SubmitChanges();

                m.dataGrid_matten_LoadData();
                this.Close();
	        }
            else
	        {
                MessageBox.Show("Bitte füllen sie die Felder aus!", "Nee!"); 
	        }
        }

        private void DeleteSZ(List<Matten> m, DatabaseConnectionDataContext d) {

            foreach (var item in m)
            {
                var incz = from i in d.Zug
                           where i.Id_Matten == item.Id
                           select i;

                var incs = from l in d.Scherung
                           where l.Id == item.Id
                           select l;

                if (Convert.ToBoolean(checkBox_Incl.IsChecked))
                {
                    d.Zug.DeleteAllOnSubmit(incz);
                    d.Scherung.DeleteAllOnSubmit(incs);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    foreach (var z in incz)
                    {
                        z.Id_Matten = null;
                    }
                    foreach (var s in incs)
                    {
                        s.Id_Matten = null;
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
}
