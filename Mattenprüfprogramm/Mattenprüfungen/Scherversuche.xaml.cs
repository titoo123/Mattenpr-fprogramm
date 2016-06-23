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
using System.Collections;
using System.Collections.Specialized;

namespace Mattenprüfprogramm.Mattenprüfungen
{
    /// <summary>
    /// Interaktionslogik für Zugversuche.xaml
    /// </summary>
    public partial class Scherversuche : Window
    {
        Matten m;
        MainWindow mv;

        //Dictionary<int, string> gW = new Dictionary<int, string>();
        //Hashtable gW = new Hashtable();
        //ListDictionary gW = new ListDictionary();
        //List<List<String>> gWl = new List<List<String>>();
        //Dictionary<int, List<String>> gW = new Dictionary<int, List<string>>();
       List<Tuple<int, string>> TgW = new List<Tuple<int,string>>();

        public Scherversuche(Matten m, MainWindow mv)
        {
            InitializeComponent();
            this.m = m;
            this.mv = mv;

            //Läd Standarddaten
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            //Prüfer
            comboBox_pruefer = Erweiterungen.Helper.LoadPrüfer(comboBox_pruefer);
            //Mattentyp
            comboBox_mattentyp = Erweiterungen.Helper.LoadMattentypen(comboBox_mattentyp);
            //Maschienen
            comboBox_maschine = Erweiterungen.Helper.LoadSchweissmaschine(comboBox_maschine);

            //Wählt betroffene Daten aus           
            comboBox_pruefer.SelectedValue = m.Prüfer.Name;
            comboBox_mattentyp.SelectedValue = m.Mattentypen.Name;
            comboBox_maschine.SelectedValue = m.Schweissmaschine.Nummer.ToString();

            datePicker_matten.SelectedDate = m.Datum;
            textBox_gluetemp.Text = Convert.ToString(m.Temperatur);
            textBox_kommentar.Text = m.Kommentar;

            //Füllt Richtung
            comboBox_scherdaten_richtung.Items.Add("Quer");
            comboBox_scherdaten_richtung.Items.Add("Längs");
            comboBox_scherdaten_richtung.Items.Add("Rand");
            comboBox_scherdaten_richtung.Items.Add("");

            checkBox_tiefgerippt.IsChecked = m.Tiefgerippt.Value;

            textBox_langstab_anzahl.Text = Convert.ToString(m.Anzahl_l);
            textBox_randstab_anzahl.Text = Convert.ToString(m.Anzahl_r);
            textBox_querstab_anzahl.Text = Convert.ToString(m.Anzahl_q);

            textBox_langstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_l);
            textBox_randstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_r);
            textBox_querstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_q);

            dataGrid_scherversuche_LoadData();
            
        }

        private void button_matte_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            datePicker_matten.IsEnabled = true;
            textBox_gluetemp.IsEnabled = true;
            textBox_kommentar.IsEnabled = true;
            comboBox_maschine.IsEnabled = true;
            comboBox_mattentyp.IsEnabled = true;
            comboBox_pruefer.IsEnabled = true;
            checkBox_tiefgerippt.IsEnabled = true;
            button_matte_speichern.IsEnabled = true;

            textBox_langstab_anzahl.IsEnabled = true;
            textBox_randstab_anzahl.IsEnabled = true;
            textBox_querstab_anzahl.IsEnabled = true;

            textBox_langstab_fehlversuche.IsEnabled = true;
            textBox_randstab_fehlversuche.IsEnabled = true;
            textBox_querstab_fehlversuche.IsEnabled = true;
        }
        private void button_matte_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var mat = from b in d.Matten
                      where b.Id == m.Id
                      select b;


            mat.First().Kommentar = textBox_kommentar.Text;
            mat.First().Datum = Convert.ToDateTime(datePicker_matten.SelectedDate);
            mat.First().Id_Maschine = Erweiterungen.Helper.GetSchweissmaschinenIdByName(Convert.ToString(comboBox_maschine.SelectedItem));
            mat.First().Id_Mattentyp = Erweiterungen.Helper.GetMattentypIdByName(Convert.ToString(comboBox_mattentyp.SelectedItem));
            mat.First().Id_Prüfer = Erweiterungen.Helper.GetPrüferIdByName(Convert.ToString(comboBox_pruefer.SelectedItem));
            mat.First().Tiefgerippt = checkBox_tiefgerippt.IsChecked;

            mat.First().Anzahl_l = Convert.ToInt32( textBox_langstab_anzahl.Text);
            mat.First().Anzahl_r = Convert.ToInt32(textBox_randstab_anzahl.Text);
            mat.First().Anzahl_q = Convert.ToInt32(textBox_querstab_anzahl.Text);

            mat.First().Fehlversuche_l = Convert.ToInt32(textBox_langstab_fehlversuche.Text);
            mat.First().Fehlversuche_r = Convert.ToInt32(textBox_randstab_fehlversuche.Text);
            mat.First().Fehlversuche_q = Convert.ToInt32(textBox_querstab_fehlversuche.Text);

            if (textBox_gluetemp.Text != String.Empty)
            {
                mat.First().Temperatur = Convert.ToDouble(textBox_gluetemp.Text);
            }
            else
            {
                mat.First().Temperatur = null;
            }

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            mv.dataGrid_matten_LoadData();

            datePicker_matten.IsEnabled = false;
            comboBox_maschine.IsEnabled = false;
            comboBox_mattentyp.IsEnabled = false;
            comboBox_pruefer.IsEnabled = false;
            textBox_gluetemp.IsEnabled = false;
            textBox_kommentar.IsEnabled = false;
            checkBox_tiefgerippt.IsEnabled = false;
            button_matte_speichern.IsEnabled = false;


            textBox_langstab_anzahl.IsEnabled = false;
            textBox_randstab_anzahl.IsEnabled = false;
            textBox_querstab_anzahl.IsEnabled = false;

            textBox_langstab_fehlversuche.IsEnabled = false;
            textBox_randstab_fehlversuche.IsEnabled = false;
            textBox_querstab_fehlversuche.IsEnabled = false;
        }

        private void button_scherung_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            button_matte_speichern.IsEnabled = true;
            textBox_scherdaten_scherwert.IsEnabled = true;
            textBox_scherdaten_hoechstkraft.IsEnabled = true;
            textBox_scherdaten_durchmesser.IsEnabled = true;
            comboBox_scherdaten_richtung.IsEnabled = true;
            button_scherung_speichern.IsEnabled = true;
        }
        private void button_scherung_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var esr = from s in db.Scherung
                      where s.Id == Convert.ToInt32(Erweiterungen.Helper.GetStringFromDataGrid(0, dataGrid_scherversuche))
                      select s;
            esr.First().Höchstkraft = Convert.ToDouble(textBox_scherdaten_hoechstkraft);
            esr.First().Scherwert = Convert.ToDouble(textBox_scherdaten_scherwert);
            esr.First().Richtung = comboBox_scherdaten_richtung.SelectedValue.ToString();
            esr.First().Durchmesser = Convert.ToDouble(textBox_scherdaten_durchmesser);

            textBox_scherdaten_durchmesser.IsEnabled = false;
            comboBox_scherdaten_richtung.IsEnabled = false;
            textBox_scherdaten_hoechstkraft.IsEnabled = false;
            textBox_scherdaten_scherwert.IsEnabled = false;


        }

        private void dataGrid_scherversuche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_scherung_bearbeiten.IsEnabled = true;
            }
            else
            {
                button_scherung_bearbeiten.IsEnabled = false;
            }


            if (dataGrid_scherversuche.SelectedItem != null)
            {
                textBox_scherdaten_durchmesser.Text = Erweiterungen.Helper.GetStringFromDataGrid(2, dataGrid_scherversuche);
                comboBox_scherdaten_richtung.SelectedValue = Erweiterungen.Helper.GetStringFromDataGrid(3, dataGrid_scherversuche);
                textBox_scherdaten_hoechstkraft.Text = Erweiterungen.Helper.GetStringFromDataGrid(4, dataGrid_scherversuche);
                textBox_scherdaten_scherwert.Text = Erweiterungen.Helper.GetStringFromDataGrid(5, dataGrid_scherversuche);
            }

            textBox_scherdaten_durchmesser.IsEnabled = false;
            comboBox_scherdaten_richtung.IsEnabled = false;
            textBox_scherdaten_hoechstkraft.IsEnabled = false;
            textBox_scherdaten_scherwert.IsEnabled = false;
        }
        private void dataGrid_scherversuche_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime) || e.Column.Header.ToString() == "Datum")
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "Id")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }


        }
        private void dataGrid_scherversuche_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            foreach (var t in TgW)
            {
                if (t.Item1 == e.Row.GetIndex())
                {
                    e.Row.Background = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }

        }
        private void dataGrid_scherversuche_LoadData()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            int i = 0;

            //Läd Scherungen
            var sch = from s in db.Scherung
                      where s.Id_Matten == m.Id
                      select new { s.Id,s.Datum, s.Durchmesser, s.Richtung, s.Höchstkraft, s.Scherwert };

            var dur = from d in db.Grenzwert
                      select d;

            TgW.Clear();
            foreach (var sc in sch)
            {
                foreach (var du in dur)
                {
                        if (du.Feld == "Durchmesser" && (sc.Durchmesser < du.Min || sc.Durchmesser > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Durchmesser"));
                        }
                        if (du.Feld == "Höchstkraft" && (sc.Höchstkraft < du.Min || sc.Höchstkraft > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Höchstkraft"));
                        }
                        if (du.Feld == "Scherwert" && (sc.Scherwert < du.Min || sc.Scherwert > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Scherwert"));
                        } 
	            }
                i++;
            }

            dataGrid_scherversuche.ItemsSource = sch.ToList();

        }




    }
}
