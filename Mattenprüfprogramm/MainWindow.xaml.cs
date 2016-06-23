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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Mattenprüfprogramm.Erweiterungen;

namespace Mattenprüfprogramm
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User user;
        private XmlDocument xml = new XmlDocument();
        public static bool anmeldung;

        //public static bool lesen;
        //public static bool schreiben;
        //public static bool admin = true;

        public MainWindow()
        {
            InitializeComponent();

            xml.Load(Benutzer.BenutzerVerwaltung.xml_Pfad);
            
            anmeldung = Convert.ToBoolean(xml.SelectSingleNode("Einstellungen").SelectSingleNode("Anmeldung").InnerText);

            if (!anmeldung)
            {
                Window BenutzerAnmeldung = new Benutzer.BenutzerAnmelden(this);
                BenutzerAnmeldung.Show();
            }
            else
            {
                MainMenu.IsEnabled = true;
                user = new User() { Name = "super", lesen = true, schreiben = true, admin = true };

                ActivateMenu(user, true);
            }

            try
            {
                dataGrid_matten_LoadData();
                label_infos_LoadData();
            }
            catch (Exception e)
            {
                MessageBox.Show("Es konnte keine Verbindung zur Datenbank hergestellt werden!\n "+ e,"Nee!");
            }

        }

        public void ActivateMenu(User user, bool value)
        {

            MenuItemStammdaten.IsEnabled = value;
            MenuItemEinstellungen.IsEnabled = value;

            button_neu.IsEnabled = value;
            button_drucken.IsEnabled = value;


            MainMenu_Abmelden.IsEnabled = value;


            dataGrid_Matten.IsEnabled = value;

            if (user.admin)
            {
                MainMenu_Benutzerverwaltung.IsEnabled = value;
            }
            if (user.schreiben || user.admin)
            {
                button_löschen_datum.IsEnabled = value;
            }

        }

        //Benutzer
        private void Benutzerverwaltung_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                if (Convert.ToBoolean(user.admin))
                {
                    Window BenutzerVerwaltung = new Benutzer.BenutzerVerwaltung();
                    BenutzerVerwaltung.Show();
                }
                else
                {
                    MessageBox.Show("Keine Berechtigung!","Fehler!");
                }
            }
          
           
        }
        private void BenutzerAnmelden_Click(object sender, RoutedEventArgs e)
        {
            Window BenutzerAnmelden = new Benutzer.BenutzerAnmelden(this);
            BenutzerAnmelden.Show();
        }
        private void BenutzerAbmelden_Click(object sender, RoutedEventArgs e)
        {
            ActivateMenu(user, false);
        }

        //Werte
        private void Mattentyp_Click(object sender, RoutedEventArgs e)
        {
            Window Mattentypen = new Stammdaten.MattentypenWindow();
            Mattentypen.Show();
        }
        private void Prüfer_Click(object sender, RoutedEventArgs e)
        {
            Window Prüfer = new Stammdaten.PrüferWindow();
            Prüfer.Show();
        }
        private void Schweissmaschinen_Click(object sender, RoutedEventArgs e)
        {
            Window Schweissmaschinen = new Stammdaten.SchweissmaschinenWindow();
            Schweissmaschinen.Show();
        }
        private void Nenngewicht_Click(object sender, RoutedEventArgs e)
        {
            Window Nenngewicht = new Stammdaten.NenngewichtWindow();
            Nenngewicht.Show();
        }
        private void Grenzwerte_Click(object sender, RoutedEventArgs e)
        {
            Window Grenzwerte = new Stammdaten.GrenzwerteWindow();
            Grenzwerte.Show();
        }
        
        //Nicht mehr vorhanden
        private void Mattenprüfungen_Click(object sender, RoutedEventArgs e)
        {
            Window Mattenprüfung = new Mattenprüfungen.Mattenverwaltung();
            Mattenprüfung.Show();
        }

        //Exporte
        private void Excel_Click(object sender, RoutedEventArgs e)
        { 
            ExcelExport eex = new ExcelExport(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten), this);
        }
        private void AS400_Click(object sender, RoutedEventArgs e)
        {
            Window as400 = new Export.AS400Export();
            as400.Show();
        }

        //Extras
        private void Informationen_Click(object sender, RoutedEventArgs e)
        {
            Window informationen = new Einstellungen.Informationen();
            informationen.Show();
        }
        private void Optionen_Click(object sender, RoutedEventArgs e)
        {
            Window optionen = new Einstellungen.Optionen(this);
            optionen.Show();
        }

        //DataGrid
        private void dataGrid_Matten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_scherversuche.IsEnabled = true;
                button_zugversuche.IsEnabled = true;
                button_löschen.IsEnabled = true;
                button_zuordnen.IsEnabled = true;
                MenuItemExport.IsEnabled = true;
            }
            else
            {
                button_scherversuche.IsEnabled = false;
                button_zugversuche.IsEnabled = false;
                button_löschen.IsEnabled = false;
                button_zuordnen.IsEnabled = false;
                MenuItemExport.IsEnabled = false;
            }

        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "Id")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }

        }
        public void dataGrid_matten_LoadData()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Matten
                      select new { u.Id, u.Datum, Prüfer = u.Prüfer.Name, Mattentyp = u.Mattentypen.Name, Scherungen = u.Scherung.Count, Züge = u.Zug.Count };

            dataGrid_Matten.ItemsSource = mat.ToList();
        }

        //Sidebar
        private void button_löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var usr = from u in db.Matten
                      where u.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Matten)
                      select u;

            db.Matten.DeleteAllOnSubmit(usr);
            db.SubmitChanges();
            dataGrid_matten_LoadData();
        }
        private void button_scherversuche_Click(object sender, RoutedEventArgs e)
        {
            Window scherversuche = new Mattenprüfungen.Scherversuche(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten), this);
            scherversuche.Show();
        }
        private void button_zugversuche_Click(object sender, RoutedEventArgs e)
        {
            Window zugversuche = new Mattenprüfungen.Zugversuche(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten), this);
            zugversuche.Show();
        }
        private void button_neu_Click(object sender, RoutedEventArgs e)
        {
            Window matteNeu = new Mattenprüfungen.MatteNeu(this);
            matteNeu.Show();
        }
        private void button_zuordnen_Click(object sender, RoutedEventArgs e)
        {
            Window mattenzuordnen = new Mattenprüfungen.MattenZuordnen(Erweiterungen.Helper.GetMatteByDataGrid(0, dataGrid_Matten), this);
            mattenzuordnen.Show();
        }
        private void button_löschen_datum_Click(object sender, RoutedEventArgs e)
        {
            Window mattenloeschen = new Mattenprüfungen.MattenLoeschenDatum(this);
            mattenloeschen.Show();
        }
        private void button_drucken_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog p = new PrintDialog();
            if (p.ShowDialog() == true)
            {
                p.PrintVisual(dataGrid_Matten, "Print Button");
            }


        }
        
        public void label_infos_LoadData()
        {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var daz = from t in d.Zug
                      where t.Id_Matten == null
                      select t;
            var das = from t in d.Scherung
                      where t.Id_Matten == null
                      select t;


            label_scherung.Content = "Es sind " + das.Count() + " Scherungen";
            label_zug.Content = "und " + daz.Count() + " Züge offen.";
        }



    }
}
