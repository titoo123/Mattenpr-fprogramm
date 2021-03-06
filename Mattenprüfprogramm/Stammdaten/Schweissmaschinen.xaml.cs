﻿using System;
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
    /// Interaktionslogik für Schweissmaschinen.xaml
    /// </summary>
    public partial class SchweissmaschinenWindow : Window
    {

        static bool neu;

        public SchweissmaschinenWindow()
        {
            InitializeComponent();
            LoadDatagrid();
        }

        private void dataGrid_schweissmaschinen_SelectionChanged(object sender, SelectionChangedEventArgs e)
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


            if (dataGrid_schweissmaschinen.SelectedItem != null)
            {
                textBox_Schweissmaschinen.Text = "" + ((TextBlock)dataGrid_schweissmaschinen.Columns[1].GetCellContent(dataGrid_schweissmaschinen.SelectedItem)).Text;
            }
            neu = false;
            textBox_Schweissmaschinen.IsEnabled = false;
        }

        private void button_neu_Click(object sender, RoutedEventArgs e)
        {

            neu = true;
            button_Speichern.IsEnabled = true;
            textBox_Schweissmaschinen.IsEnabled = true;

            textBox_Schweissmaschinen.Text = String.Empty;
        }

        private void button_bearbeiten_Click(object sender, RoutedEventArgs e)
        {

            textBox_Schweissmaschinen.IsEnabled = true;
            button_Speichern.IsEnabled = true;
        }

        private void button_Speichern_Click(object sender, RoutedEventArgs e)
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            if (neu)
            {
                try
                {
                    Schweissmaschine m = new Schweissmaschine() { Name = textBox_Schweissmaschinen.Text };
                    db.Schweissmaschine.InsertOnSubmit(m);
                }
                catch (Exception)
                {
                    MessageBox.Show("Eingabe muss eine Zahl sein!", "Falsche Eingabe!");
                }

                
            }
            else
            {
                var mat = from m in db.Schweissmaschine
                          where m.Id == Convert.ToInt32("" + ((TextBlock)dataGrid_schweissmaschinen.Columns[0].GetCellContent(dataGrid_schweissmaschinen.SelectedItem)).Text)
                          select m;

                try
                {
                    mat.First().Name = textBox_Schweissmaschinen.Text;
                }
                catch (Exception)
                {
                    MessageBox.Show("Eingabe muss eine Zahl sein!", "Falsche Eingabe!");
                }

            }
            db.SubmitChanges();
            textBox_Schweissmaschinen.Text = String.Empty;
            LoadDatagrid();
        }

        private void button_löschen_Click(object sender, RoutedEventArgs e)
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            string tmp = "" + ((TextBlock)dataGrid_schweissmaschinen.Columns[0].GetCellContent(dataGrid_schweissmaschinen.SelectedItem)).Text;

            var usr = from u in db.Schweissmaschine
                      where u.Id == Convert.ToInt32(tmp)
                      select u;

            db.Schweissmaschine.DeleteAllOnSubmit(usr);
            db.SubmitChanges();
            LoadDatagrid();
        }

        private void LoadDatagrid()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Schweissmaschine
                      select u;

            dataGrid_schweissmaschinen.ItemsSource = mat.ToList();
        }

        private void dataGrid_schweissmaschinen_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }
    }
}
