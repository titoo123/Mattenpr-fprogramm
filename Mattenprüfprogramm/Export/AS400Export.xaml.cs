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

using ElmMQDNetLib;

namespace Mattenprüfprogramm.Export
{
    /// <summary>
    /// Interaktionslogik für AS400Export.xaml
    /// </summary>
    public partial class AS400Export : Window
    {
        //von
        DateTime? d1;
        //bis
        DateTime? d2;

        public AS400Export()
        {
            InitializeComponent();
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            ////Sind die Datepicker ausgefüllt?
            //if (D1_Picker.SelectedDate != null && D2_Picker.SelectedDate != null && D1_Picker.SelectedDate < D2_Picker.SelectedDate)
            //{
            //    //Konvertieren des Datums
            //    d1 = D1_Picker.SelectedDate;
            //    d1 = D2_Picker.SelectedDate;

            //    //Datenbankabfrage der Matten mit betreffenden Datum die noch nicht exportiert wurden
            //    DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            //    var ase = from a in d.Matten
            //              where a.Datum >= d1 && a.Datum <= d2 && ( a.E_AS400 == false || a.E_AS400 == null)
            //              select a;
            //    int i = ase.Count();
            //    //Schleife durchfährt alle betreffenden Matten
            //    foreach (var m in ase)
            //    {
            //        //Scherungen
            //        var sch = from s in d.Scherung
            //                  where s.Id_Matten == m.Id 
            //                  select s;
            //        //Züge
            //        var zug = from z in d.Zug
            //                  where z.Id_Matten == m.Id
            //                  select z;
            //        //Senden...
            //        //Scherungen
            //        foreach (var s in sch)
            //        {
            //            //SendToAS400(...);
            //        }
            //        //Züge
            //        foreach (var z in zug)
            //        {
            //            //SendToAS400(...);
            //        }

            //        //Speichert in Datenbank dasd Datensatz exportiert wurde
            //        m.E_AS400 = true;

            //        try
            //        {
            //            d.SubmitChanges();
            //        }
            //        catch (Exception)
            //        {
            //        }


            //    }


            //}
            //else
            //{
            //    MessageBox.Show("Bitte wählen sie für beide Datumsfelder ein Datum aus!","Achtung!");
            //}
        }

        /// <summary>
        /// Sendet Daten-String zum AS400
        /// </summary>
        /// <param name="m">Daten-String/Message</param>
        //private void SendToAS400(string m) {

        //    int reasonCode = 0;
        //    int compCode = 0;
        //    ElmMQDNet ty = new ElmMQDNet("MQConfig.xml");
        //    ty.Put(false,"Put00", "Test message", "", "USR", ref compCode, ref reasonCode);
        //    ty.Close();
        //}
    }
}
