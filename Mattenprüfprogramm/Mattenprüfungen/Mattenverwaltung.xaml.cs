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
    /// Interaktionslogik für Mattenverwaltung.xaml
    /// </summary>
    public partial class Mattenverwaltung : Window
    {
        public Mattenverwaltung()
        {
            InitializeComponent();
            //LoadDatagrid();
            //LoadData();
        }

        //private void button_löschen_Click(object sender, RoutedEventArgs e)
        //{
        //    DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
       
        //    var usr = from u in db.Matten
        //              where u.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Matten)
        //              select u;

        //    db.Matten.DeleteAllOnSubmit(usr);
        //    db.SubmitChanges();
        //    LoadDatagrid();
        //}

        //public void LoadDatagrid()
        //{
        //    DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

        //    var mat = from u in db.Matten
        //              select new { u.Id , u.Datum , Prüfer = u.Prüfer.Name , Mattentyp = u.Mattentypen.Name , Scherungen = u.Scherung.Count , Züge = u.Zug.Count };

        //    dataGrid_Matten.ItemsSource = mat.ToList();
        //}

        //private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.PropertyType == typeof(System.DateTime))
        //        (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        //}

        //private void button_scherversuche_Click(object sender, RoutedEventArgs e)
        //{
        //    Window scherversuche = new Scherversuche(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten),this);
        //    scherversuche.Show();
        //}

        //private void button_zugversuche_Click(object sender, RoutedEventArgs e)
        //{
        //    Window zugversuche = new Zugversuche(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten),this);
        //    zugversuche.Show();
        //}

        //private void dataGrid_Matten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (((DataGrid)sender).SelectedIndex != -1)
        //    {
        //        button_scherversuche.IsEnabled = true;
        //        button_zugversuche.IsEnabled = true;
        //        button_löschen.IsEnabled = true;
        //        button_zuordnen.IsEnabled = true;
        //    }
        //    else
        //    {
        //        button_scherversuche.IsEnabled = false;
        //        button_zugversuche.IsEnabled = false;
        //        button_löschen.IsEnabled = false;
        //        button_zuordnen.IsEnabled = false;
        //    }

        //}

        //private void button_neu_Click(object sender, RoutedEventArgs e)
        //{
        //    Window matteNeu = new Mattenprüfungen.MatteNeu(this);
        //    matteNeu.Show();
        //}

        //private void button_zuordnen_Click(object sender, RoutedEventArgs e)
        //{
        //    Window mattenzuordnen = new MattenZuordnen(Erweiterungen.Helper.GetMatteByDataGrid(0,dataGrid_Matten),this);
        //    mattenzuordnen.Show();
        //}

        //private void button_löschen_datum_Click(object sender, RoutedEventArgs e)
        //{
        //    Window mattenloeschen = new MattenLoeschenDatum(this);
        //    mattenloeschen.Show();
        //}

        //private void button_drucken_Click(object sender, RoutedEventArgs e)
        //{
        //    PrintDialog p = new PrintDialog();
        //    if (p.ShowDialog() == true)
        //    {
        //        p.PrintVisual(dataGrid_Matten, "Print Button");
        //    }


        //}
        //public void LoadData() {

        //    DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        //    var daz = from t in d.Zug
        //              where t.Id_Matten == null
        //              select t;
        //    var das = from t in d.Scherung
        //              where t.Id_Matten == null
        //              select t;


        //    label_scherung.Content = "Es sind " + das.Count() + " Scherungen";
        //    label_zug.Content = "und " + daz.Count() + " Züge offen.";
        //}
    }
}
