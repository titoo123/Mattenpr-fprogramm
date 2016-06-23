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
using System.Data;
using System.Reflection;
using System.Xml;

namespace Mattenprüfprogramm.Benutzer
{
    /// <summary>
    /// Interaktionslogik für BenutzerVerwaltung.xaml
    /// </summary>
    public partial class BenutzerVerwaltung : Window
    {
        static bool neu;
        static int value;

        public static string xml_Pfad = "LocalData.xml";
        XmlDocument xml = new XmlDocument();

        public BenutzerVerwaltung()
        {
            InitializeComponent();
            LoadDatagrid();

            try
            {
                xml.Load(xml_Pfad);
                checkBox_Anmeldung_deaktivieren.IsChecked = Convert.ToBoolean(xml.SelectSingleNode("Einstellungen").SelectSingleNode("Anmeldung").InnerText);
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!","Fehler!");
            }
        }

        private void Button_Speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            if (neu)
            {
                User u = new User() { Name = textBox_Name.Text, Passwort = textBox_Passwort.Text, lesen = checkBox_lesen.IsChecked.Value, schreiben = checkBox_schreiben.IsChecked.Value, admin = checkBox_admin.IsChecked.Value };
                db.User.InsertOnSubmit(u);
            }
            else
            {
                var usr = from u in db.User
                          where u.Id == Convert.ToInt32("" + ((TextBlock)dataGrid_BenutzerVerwaltung.Columns[0].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem)).Text)
                          select u;

                User us = usr.First();
                us.Name = textBox_Name.Text;
                us.lesen = checkBox_lesen.IsChecked.Value;
                us.schreiben = checkBox_schreiben.IsChecked.Value;
                us.admin = checkBox_admin.IsChecked.Value;

            }
            db.SubmitChanges();

            LoadDatagrid();

            button_Speichern.IsEnabled = false;
            checkBox_admin.IsEnabled = false;
            checkBox_lesen.IsEnabled = false;
            checkBox_schreiben.IsEnabled = false;

            textBox_Name.Text = String.Empty;
            textBox_Passwort.Text = String.Empty;

            checkBox_lesen.IsChecked = false;
            checkBox_schreiben.IsChecked = false;
            checkBox_admin.IsChecked = false;

        }
        private void Button_Neu_Click(object sender, RoutedEventArgs e)
        {
            textBox_Name.Text = String.Empty;
            checkBox_lesen.IsChecked = false;
            checkBox_schreiben.IsChecked = false;
            checkBox_admin.IsChecked = false;

            textBox_Name.IsEnabled = true;
            checkBox_lesen.IsEnabled = true;
            checkBox_schreiben.IsEnabled = true;
            checkBox_admin.IsEnabled = true;

            neu = true;
            button_Speichern.IsEnabled = true;
        }
        private void Button_Bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_Passwort.IsEnabled = true;
            textBox_Name.IsEnabled = true;
            checkBox_admin.IsEnabled = true;
            checkBox_lesen.IsEnabled = true;
            checkBox_schreiben.IsEnabled = true;


            button_Speichern.IsEnabled = true;
          
        }
        private void Button_Löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            string tmp = "" + ((TextBlock)dataGrid_BenutzerVerwaltung.Columns[0].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem)).Text;

            var usr = from u in db.User
                      where u.Id == Convert.ToInt32( tmp)
                      select u;
            db.User.DeleteAllOnSubmit(usr);
            db.SubmitChanges();
            LoadDatagrid();
        }

        private void dataGrid_BenutzerVerwaltung_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }

        private void checkBox_schreiben_Checked(object sender, RoutedEventArgs e)
        {
        }
        private void checkBox_lesen_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void checkBox_admin_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LoadDatagrid()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var usr = from u in db.User
                      select new { u.Id, u.Name, u.lesen, u.schreiben, u.admin };

            dataGrid_BenutzerVerwaltung.ItemsSource = usr.ToList();
        }

        private void dataGrid_BenutzerVerwaltung_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            value = ((DataGrid)sender).SelectedIndex;

            if (dataGrid_BenutzerVerwaltung.SelectedItem != null)
            {               
                textBox_Name.Text = "" + ((TextBlock)dataGrid_BenutzerVerwaltung.Columns[1].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem)).Text;

                CheckBox ck_tmp1 = (CheckBox)dataGrid_BenutzerVerwaltung.Columns[2].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem);
                CheckBox ck_tmp2 = (CheckBox)dataGrid_BenutzerVerwaltung.Columns[3].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem);
                CheckBox ck_tmp3 = (CheckBox)dataGrid_BenutzerVerwaltung.Columns[4].GetCellContent(dataGrid_BenutzerVerwaltung.SelectedItem);

                if (Convert.ToBoolean(ck_tmp1.IsChecked))
                {
                    checkBox_lesen.IsChecked = true;
                }
                else
                {
                    checkBox_lesen.IsChecked = false;
                }


                if (Convert.ToBoolean(ck_tmp2.IsChecked))
                {
                    checkBox_schreiben.IsChecked = true;
                }
                else
                {
                    checkBox_schreiben.IsChecked = false;
                }

                if (Convert.ToBoolean(ck_tmp3.IsChecked))
                {
                    checkBox_admin.IsChecked = true;
                }
                else
                {
                    checkBox_admin.IsChecked = false;
                }
                
            }
            neu = false;
            textBox_Name.IsEnabled = false;
            checkBox_lesen.IsEnabled = false;
            checkBox_schreiben.IsEnabled = false;
            checkBox_admin.IsEnabled = false;
        }

        private void button_passwort_zurücksetzen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Anmeldung_deaktivieren_Checked(object sender, RoutedEventArgs e)
        {
            xml.SelectSingleNode("Einstellungen").SelectSingleNode("Anmeldung").InnerText = "true";
            xml.Save(xml_Pfad);
        }
        private void checkBox_Anmeldung_deaktivieren_Unchecked(object sender, RoutedEventArgs e)
        {
            xml.SelectSingleNode("Einstellungen").SelectSingleNode("Anmeldung").InnerText = "false";
            xml.Save(xml_Pfad);
        }
    }
}
