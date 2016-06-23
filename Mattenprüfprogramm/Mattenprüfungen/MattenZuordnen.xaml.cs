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
    /// Interaktionslogik für MattenZuordnen.xaml
    /// </summary>
    public partial class MattenZuordnen : Window
    {
        Matten m;
        MainWindow mv;

        static int cScherung;
        List<int> sList = new List<int>();

        static int cZug;
        List<int> zList = new List<int>();

        public MattenZuordnen(Matten m, MainWindow mv)
        {
            InitializeComponent();
            this.mv = mv;
            this.m = m;

            cZug = 0;
            cScherung = 0;

            textBox_date.Text = m.Datum.ToShortDateString();
            textBox_kommentar.Text = m.Kommentar;
            textBox_maschine.Text = Convert.ToString( m.Schweissmaschine.Nummer);
            textBox_mattentyp.Text = m.Mattentypen.Name;
            textBox_pnummer.Text = m.Prüfnummer.ToString();
            textBox_pru.Text = m.Prüfer.Name;
            textBox_temperatur.Text = Convert.ToString(m.Temperatur);

            LoadGridScherung();
            LoadGridZug();
        }

        void LoadGridScherung() {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var sch = from s in d.Scherung
                      where s.Id_Matten == null && !sList.Contains(s.Id)
                      select new { s.Id , s.Richtung , s.Höchstkraft, s.Durchmesser, s.Scherwert };

            dataGrid_scherung.ItemsSource = sch.ToList();
            
        }
        void LoadGridZug() {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var sch = from s in d.Zug
                      where s.Id_Matten == null && !zList.Contains(s.Id)
                      select s;

            dataGrid_zug.ItemsSource = sch.ToList();
            
        }

        private void button_SuS_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext   d = new DatabaseConnectionDataContext();
            try
            {
                foreach (int i in sList)
                {
                    var sel = from s in d.Scherung
                              where s.Id == i
                              select s;
                    sel.First().Id_Matten = m.Id;
                }
                foreach (int i in zList)
                {
                    var sel = from s in d.Zug
                              where s.Id == i
                              select s;
                    sel.First().Id_Matten = m.Id;
                }
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Keine Verindung zur Datenbank! Änderungen werden nicht übernommen!", "Nee!");
            }

            mv.label_infos_LoadData();
            mv.dataGrid_matten_LoadData();
            this.Close();
        }

        private void dataGrid_scherung_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_scherung_hinzufügen.IsEnabled = true;

                cScherung = Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_scherung);
            }
            else
            {
                button_scherung_hinzufügen.IsEnabled = false;  
            }
        }

        

        private void button_scherung_hinzufügen_Click(object sender, RoutedEventArgs e)
        {

            sList.Add(cScherung);
            LoadGridScherung();
        }

        private void button_zug_hinzufügen_Click(object sender, RoutedEventArgs e)
        {
            zList.Add(cZug);
            LoadGridZug();
        }

        private void dataGrid_zug_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_zug_hinzufügen.IsEnabled = true;

                cZug = Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_zug);
            }
            else
            {
                button_zug_hinzufügen.IsEnabled = false;
            }
        }

        private void dataGrid_scherung_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }

        private void dataGrid_zug_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }
    }

}
