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

namespace Mattenprüfprogramm.Stammdaten
{
    /// <summary>
    /// Interaktionslogik für Nenngewicht.xaml
    /// </summary>
    public partial class NenngewichtWindow : Window
    {
        static bool neu;

        public NenngewichtWindow()
        {
            InitializeComponent();
            LoadDatagrid();
        }

        private void dataGrid_Nenngewicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_löschen.IsEnabled = true;
                button_bearbeiten.IsEnabled = true;
            }
            else
            {
                button_löschen.IsEnabled = false;
                button_bearbeiten.IsEnabled = false;
            }


            if (dataGrid_Nenngewicht.SelectedItem != null)
            {
                textBox_Durchmesser.Text = "" + ((TextBlock)dataGrid_Nenngewicht.Columns[1].GetCellContent(dataGrid_Nenngewicht.SelectedItem)).Text;
                textBox_Nenngewicht.Text = "" + ((TextBlock)dataGrid_Nenngewicht.Columns[2].GetCellContent(dataGrid_Nenngewicht.SelectedItem)).Text;
            }
            neu = false;
            textBox_Nenngewicht.IsEnabled = false;
            textBox_Durchmesser.IsEnabled = false;
        }

        private void button_neu_Click(object sender, RoutedEventArgs e)
        {

            neu = true;
            button_Speichern.IsEnabled = true;

            textBox_Nenngewicht.IsEnabled = true;
            textBox_Durchmesser.IsEnabled = true;

            textBox_Durchmesser.Text = String.Empty;
            textBox_Nenngewicht.Text = String.Empty;
        }

        private void button_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_Durchmesser.IsEnabled = true;
            textBox_Nenngewicht.IsEnabled = true;
            button_Speichern.IsEnabled = true;
        }

        private void button_Speichern_Click(object sender, RoutedEventArgs e)
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            if (neu)
            {
                try
                {
                    Nenngewichte m = new Nenngewichte() { Nenngewicht = Convert.ToDouble(textBox_Nenngewicht.Text), Durchmesser = Convert.ToDouble(textBox_Durchmesser.Text) };
                    db.Nenngewichte.InsertOnSubmit(m);
                }
                catch (Exception)
                {
                    MessageBox.Show("Eingabe muss eine Zahl sein!", "Falsche Eingabe!");
                }

                
            }
            else
            {
                var mat = from m in db.Nenngewichte
                          where m.Id == Convert.ToInt32("" + ((TextBlock)dataGrid_Nenngewicht.Columns[0].GetCellContent(dataGrid_Nenngewicht.SelectedItem)).Text)
                          select m;

                try
                {
                    mat.First().Nenngewicht = Convert.ToDouble(textBox_Nenngewicht.Text);
                    mat.First().Durchmesser = Convert.ToDouble(textBox_Durchmesser.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Eingabe muss eine Zahl sein!", "Falsche Eingabe!");
                }

            }
            db.SubmitChanges();

            textBox_Nenngewicht.Text = String.Empty;
            textBox_Durchmesser.Text = String.Empty;
            textBox_Durchmesser.IsEnabled = false;
            textBox_Nenngewicht.IsEnabled = false;

            LoadDatagrid();
        }

        private void button_löschen_Click(object sender, RoutedEventArgs e)
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            string tmp = "" + ((TextBlock)dataGrid_Nenngewicht.Columns[0].GetCellContent(dataGrid_Nenngewicht.SelectedItem)).Text;

            var usr = from u in db.Nenngewichte
                      where u.Id == Convert.ToInt32(tmp)
                      select u;

            db.Nenngewichte.DeleteAllOnSubmit(usr);
            db.SubmitChanges();
            LoadDatagrid();
        }

        private void LoadDatagrid()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Nenngewichte
                      select u;

            dataGrid_Nenngewicht.ItemsSource = mat.ToList();
        }

        private void dataGrid_Nenngewicht_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }

       

    }
}
