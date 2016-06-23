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
using System.Text.RegularExpressions;

namespace Mattenprüfprogramm.Stammdaten
{
    /// <summary>
    /// Interaktionslogik für Grenzwerte.xaml
    /// </summary>
    public partial class GrenzwerteWindow : Window
    {
        static bool feld_neu;
        static bool rippen_neu;

        public GrenzwerteWindow()
        {
            InitializeComponent();

            //comboBox_feld_feld.Items.Add("Durchmesser");

            //comboBox_feld_feld.Items.Add("Masse");
            comboBox_feld_feld.Items.Add("Zugfestigkeit");
            comboBox_feld_feld.Items.Add("Dehngrenze");
            //comboBox_feld_feld.Items.Add("Dehngrenze Höchstwert");
            comboBox_feld_feld.Items.Add("Bruchdehnung");
            comboBox_feld_feld.Items.Add("Streckgrenzverhältnis");
            comboBox_feld_feld.Items.Add("Abweichung Nenngewicht");
            comboBox_feld_feld.Items.Add("Höchstkraft");
            comboBox_feld_feld.Items.Add("Scherwert");

            LoadDataGridFeld();
            LoadDatagridRippen();
        }

        private void dataGrid_feld_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_feld_bearbeiten.IsEnabled = true;
                button_feld_löschen.IsEnabled = true;
            }
            else
            {
                button_feld_bearbeiten.IsEnabled = false;
                button_feld_löschen.IsEnabled = false;
            }

            if (dataGrid_feld.SelectedItem != null)
            {
                try
                {
                    comboBox_feld_feld.SelectedItem = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_feld);
                }
                catch (Exception)
                {

                }

                textBox_feld_min.Text = Erweiterungen.Helper.GetStringFromDataGrid(2, dataGrid_feld);
                textBox_feld_max.Text = Erweiterungen.Helper.GetStringFromDataGrid(3, dataGrid_feld);

            }

            textBox_feld_min.IsEnabled = false;
            textBox_feld_max.IsEnabled = false;
            comboBox_feld_feld.IsEnabled = false;

        }

        private void button_feld_neu_Click(object sender, RoutedEventArgs e)
        {
            feld_neu = true;
            LoadDataGridFeld();
            textBox_feld_max.IsEnabled = true;
            textBox_feld_min.IsEnabled = true;
            comboBox_feld_feld.IsEnabled = true;
            button_feld_Speichern.IsEnabled = true;
           
        }

        private void button_feld_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_feld_max.IsEnabled = true;
            textBox_feld_min.IsEnabled = true;
            comboBox_feld_feld.IsEnabled = true;
            button_feld_Speichern.IsEnabled = true;
        }

        private void button_feld_Speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            if (feld_neu)
            {
                Grenzwert g = new Grenzwert() { Feld = comboBox_feld_feld.SelectedItem.ToString(),
                                                Min = Convert.ToDouble(textBox_feld_min.Text),
                                                Max = Convert.ToDouble(textBox_feld_max.Text)
                };
                d.Grenzwert.InsertOnSubmit(g);
            }
            else
            {
                var gre = from g in d.Grenzwert
                          where g.Id == Erweiterungen.Helper.GetIntFromDataGrid(0,dataGrid_feld)
                          select g;
                gre.First().Feld = comboBox_feld_feld.SelectedItem.ToString();
                gre.First().Min = Convert.ToDouble(textBox_feld_min.Text);
                gre.First().Max = Convert.ToDouble(textBox_feld_max.Text);
            }

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            LoadDataGridFeld();
        }

        private void button_feld_löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var gre = from g in d.Grenzwert
                      where g.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_feld)
                      select g;
            d.Grenzwert.DeleteOnSubmit(gre.First());

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            LoadDataGridFeld();
        }

        private void LoadDataGridFeld() {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var ger = from s in db.Grenzwert
                      select s;

            dataGrid_feld.ItemsSource = ger.ToList();



            textBox_feld_max.Text = String.Empty;
            textBox_feld_min.Text = String.Empty;
            comboBox_feld_feld.SelectedIndex = -1;

            textBox_feld_min.IsEnabled = false;
            textBox_feld_max.IsEnabled = false;
            comboBox_feld_feld.IsEnabled = false;

            button_feld_bearbeiten.IsEnabled = true;
            button_feld_löschen.IsEnabled = true;
            button_feld_Speichern.IsEnabled = true;
        }

        private void dataGrid_rippen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_feld_bearbeiten.IsEnabled = true;
                button_feld_löschen.IsEnabled = true;
            }
            else
            {
                button_feld_bearbeiten.IsEnabled = false;
                button_feld_löschen.IsEnabled = false;
            }

            if (dataGrid_rippen.SelectedItem != null)
            {
                textBox_rippen_nenndurchmesser.Text = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_rippen);
                textBox_rippen_min.Text = Erweiterungen.Helper.GetStringFromDataGrid(2, dataGrid_rippen);
                textBox_rippen_max.Text = Erweiterungen.Helper.GetStringFromDataGrid(3, dataGrid_rippen);

            }

            textBox_rippen_nenndurchmesser.IsEnabled = false;
            textBox_rippen_min.IsEnabled = false;
            textBox_rippen_max.IsEnabled = false;
        }

        private void button_rippen_neu_Click(object sender, RoutedEventArgs e)
        {
            rippen_neu = true;
            LoadDatagridRippen();
            textBox_rippen_nenndurchmesser.IsEnabled = true;
            textBox_rippen_min.IsEnabled = true;
            textBox_rippen_max.IsEnabled = true;
            button_feld_Speichern.IsEnabled = true;

            textBox_rippen_nenndurchmesser.Text = String.Empty;
            textBox_rippen_min.Text = String.Empty;
            textBox_rippen_max.Text = String.Empty;

        }

        private void button_rippen_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_rippen_nenndurchmesser.IsEnabled = true;
            textBox_rippen_min.IsEnabled = true;
            textBox_rippen_max.IsEnabled = true;
            button_rippen_speichern.IsEnabled = true;
        }

        private void button_rippen_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            if (rippen_neu)
            {
                Rippenflächen g = new Rippenflächen()
                {
                    Nenndurchmesser = Convert.ToDouble(textBox_rippen_nenndurchmesser.Text),
                    Min = Convert.ToDouble(textBox_rippen_min.Text),
                    Max = Convert.ToDouble(textBox_rippen_max.Text)
                };
                d.Rippenflächen.InsertOnSubmit(g);
            }
            else
            {
                var gre = from g in d.Rippenflächen
                          where g.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_rippen)
                          select g;
                gre.First().Nenndurchmesser = Convert.ToDouble(textBox_rippen_nenndurchmesser.Text);
                gre.First().Min = Convert.ToDouble(textBox_rippen_min.Text);
                gre.First().Max = Convert.ToDouble(textBox_rippen_max.Text);
            }

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            LoadDatagridRippen();
        }

        private void button_rippen_löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var gre = from g in d.Rippenflächen
                      where g.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_rippen)
                      select g;
            d.Rippenflächen.DeleteOnSubmit(gre.First());

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            LoadDatagridRippen();
        }
        void LoadDatagridRippen() {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var ger = from s in db.Rippenflächen
                      select s;

            dataGrid_rippen.ItemsSource = ger.ToList();



            textBox_rippen_max.Text = String.Empty;
            textBox_rippen_min.Text = String.Empty;
            textBox_rippen_nenndurchmesser.Text = String.Empty;

            textBox_rippen_max.IsEnabled = false;
            textBox_rippen_min.IsEnabled = false;
            textBox_rippen_nenndurchmesser.IsEnabled = false;

            button_rippen_bearbeiten.IsEnabled = true;
            button_rippen_löschen.IsEnabled = true;
            button_rippen_speichern.IsEnabled = true;
        }

        private void textBox_feld_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_feld_min.Text = Regex.Replace(textBox_feld_min.Text, "[A-Za-z]", "");
        }

        private void dataGrid_feld_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }

        private void dataGrid_rippen_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }
    }
}
