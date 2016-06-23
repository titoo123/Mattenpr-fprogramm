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
    /// Interaktionslogik für Mattentypen.xaml
    /// </summary>
    public partial class MattentypenWindow : Window
    {
        static bool neu;

        public MattentypenWindow()
        {
            InitializeComponent();
            LoadDatagrid();
        }

        private void dataGrid_Mattentypen_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            

            if (dataGrid_Mattentypen.SelectedItem != null)
            {
                textBox_Mattentypen.Text = "" + ((TextBlock)dataGrid_Mattentypen.Columns[1].GetCellContent(dataGrid_Mattentypen.SelectedItem)).Text;
            }
            neu = false;
            textBox_Mattentypen.IsEnabled = false;
       
        }

        private void Button_Neu_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            button_Speichern.IsEnabled = true;
            textBox_Mattentypen.IsEnabled = true;

            textBox_Mattentypen.Text = String.Empty;
        }

        private void Button_Bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_Mattentypen.IsEnabled = true;
            button_Speichern.IsEnabled = true;
        }

        private void Button_Speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            if (neu)
            {
                Mattentypen m = new Mattentypen() { Name = textBox_Mattentypen.Text };
                db.Mattentypen.InsertOnSubmit(m);
            }
            else
            {
                var mat = from m in db.Mattentypen
                          where m.Id == Convert.ToInt32("" + ((TextBlock)dataGrid_Mattentypen.Columns[0].GetCellContent(dataGrid_Mattentypen.SelectedItem)).Text)
                          select m;

                mat.First().Name = textBox_Mattentypen.Text;
                
            }
            db.SubmitChanges();
            textBox_Mattentypen.Text = String.Empty;
            LoadDatagrid();
        }

        private void Button_Löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            string tmp = "" + ((TextBlock)dataGrid_Mattentypen.Columns[0].GetCellContent(dataGrid_Mattentypen.SelectedItem)).Text;

            var usr = from u in db.Mattentypen
                      where u.Id == Convert.ToInt32(tmp)
                      select u;

            db.Mattentypen.DeleteAllOnSubmit(usr);
            db.SubmitChanges();
            LoadDatagrid();
        
        }
        private void LoadDatagrid()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Mattentypen
                      select u;

            dataGrid_Mattentypen.ItemsSource = mat.ToList();
        }

        private void dataGrid_Mattentypen_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }
    }
}
