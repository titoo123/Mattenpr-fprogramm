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
    /// Interaktionslogik für Zugversuche.xaml
    /// </summary>
    public partial class Zugversuche : Window
    {
        Matten m;
        MainWindow mv;

        List<Tuple<int, string>> TgW = new List<Tuple<int, string>>();

        public Zugversuche(Matten m, MainWindow mv)
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
            //


            comboBox_pruefer.SelectedValue = m.Prüfer.Name;
            comboBox_mattentyp.SelectedValue = m.Mattentypen.Name;
            comboBox_maschine.SelectedValue = m.Schweissmaschine.Nummer.ToString();

            datePicker_matten.SelectedDate = m.Datum;

            textBox_kommentar.Text = m.Kommentar;

            comboBox_zugdaten_richtung.Items.Add("Quer");
            comboBox_zugdaten_richtung.Items.Add("Längs");
            comboBox_zugdaten_richtung.Items.Add("Rand");

            checkBox_tiefgerippt.IsChecked = m.Tiefgerippt.Value;

            textBox_langstab_anzahl.Text = Convert.ToString(m.Anzahl_l);
            textBox_randstab_anzahl.Text = Convert.ToString(m.Anzahl_r);
            textBox_querstab_anzahl.Text = Convert.ToString(m.Anzahl_q);

            textBox_langstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_l);
            textBox_randstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_r);
            textBox_querstab_fehlversuche.Text = Convert.ToString(m.Fehlversuche_q);

            datagrid_zugversuche_LoadData();
        }

        private void button_matte_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            datePicker_matten.IsEnabled = true;

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


            mat.First().Anzahl_l = Convert.ToInt32(textBox_langstab_anzahl.Text);
            mat.First().Anzahl_r = Convert.ToInt32(textBox_randstab_anzahl.Text);
            mat.First().Anzahl_q = Convert.ToInt32(textBox_querstab_anzahl.Text);

            mat.First().Fehlversuche_l = Convert.ToInt32(textBox_langstab_fehlversuche.Text);
            mat.First().Fehlversuche_r = Convert.ToInt32(textBox_randstab_fehlversuche.Text);
            mat.First().Fehlversuche_q = Convert.ToInt32(textBox_querstab_fehlversuche.Text);


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

        private void button_zug_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_zugdaten_durchmesser.IsEnabled = true;
            textBox_zugdaten_masse.IsEnabled = true;
            comboBox_zugdaten_richtung.IsEnabled = true;
            textBox_zugfestigkeit.IsEnabled = true;
            textBox_dehngrenze.IsEnabled = true;
            textBox_zugdaten_dehnhöchstkraft.IsEnabled = true;
            textBox_bruchdehnung.IsEnabled = true;
            textBox_streckengrenzverhältnis.IsEnabled = true;
            textBox_abweichungNenngewicht.IsEnabled = true;


            textBox_Hnm_1.IsEnabled = true;
            textBox_Hnm_2.IsEnabled = true;
            textBox_Hnm_3.IsEnabled = true;

            textBox_Hn25_1.IsEnabled = true;
            textBox_Hn25_2.IsEnabled = true;
            textBox_Hn25_3.IsEnabled = true;

            textBox_Hn75_1.IsEnabled = true;
            textBox_Hn75_2.IsEnabled = true;
            textBox_Hn75_3.IsEnabled = true;

            textBox_c1.IsEnabled = true;
            textBox_c2.IsEnabled = true;
            textBox_c3.IsEnabled = true;

            textBox_se_1.IsEnabled = true;
            textBox_se_2.IsEnabled = true;
            textBox_se_3.IsEnabled = true;

            textBox_fr.IsEnabled = true;
            button_zug_speichern.IsEnabled = true;
          //  button_matte_speichern.IsEnabled = true;
            button_fr_berechnen.IsEnabled = true;

        }
        private void button_zug_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var esr = from s in db.Zug
                      where s.Id == Convert.ToInt32(Erweiterungen.Helper.GetStringFromDataGrid(0, dataGrid_zugversuche ))
                      select s;


            esr.First().D = Convert.ToDouble( textBox_zugdaten_durchmesser.Text);
            esr.First().M = Convert.ToDouble(textBox_zugdaten_masse.Text);
            esr.First().Richtung = Convert.ToString(comboBox_zugdaten_richtung.SelectedValue);
            esr.First().Rm = Convert.ToDouble(textBox_zugfestigkeit.Text);
            esr.First().Rp = Convert.ToDouble(textBox_dehngrenze.Text);
            esr.First().Agt = Convert.ToDouble(textBox_zugdaten_dehnhöchstkraft.Text);
            esr.First().A = Convert.ToDouble(textBox_bruchdehnung.Text);
            esr.First().RmRp = Convert.ToDouble(textBox_streckengrenzverhältnis.Text);
            esr.First().Dgs = Convert.ToDouble(textBox_abweichungNenngewicht.Text);


            esr.First().H1m = Convert.ToDouble(textBox_Hnm_1.Text);
            esr.First().H2m = Convert.ToDouble(textBox_Hnm_2.Text);
            esr.First().H3m = Convert.ToDouble(textBox_Hnm_3.Text);

            esr.First().H125 = Convert.ToDouble(textBox_Hn25_1.Text);
            esr.First().H225 = Convert.ToDouble(textBox_Hn25_2.Text);
            esr.First().H325 = Convert.ToDouble(textBox_Hn25_3.Text);

            esr.First().H175 = Convert.ToDouble(textBox_Hn75_1.Text);
            esr.First().H275 = Convert.ToDouble(textBox_Hn75_2.Text);
            esr.First().H375 = Convert.ToDouble(textBox_Hn75_3.Text);

            esr.First().c1 = Convert.ToDouble(textBox_c1.Text);
            esr.First().c2 = Convert.ToDouble(textBox_c2.Text);
            esr.First().c3 = Convert.ToDouble(textBox_c3.Text);

            esr.First().se1 = Convert.ToDouble(textBox_se_1.Text);
            esr.First().se2 = Convert.ToDouble(textBox_se_2.Text);
            esr.First().se3 = Convert.ToDouble(textBox_se_3.Text);

            esr.First().fR = Convert.ToDouble(textBox_fr.Text);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
            }
            datagrid_zugversuche_LoadData();
            textBox_zugdaten_durchmesser.IsEnabled = false;
            textBox_zugdaten_masse.IsEnabled = false;
            comboBox_zugdaten_richtung.IsEnabled = false;
            textBox_zugfestigkeit.IsEnabled = false;
            textBox_dehngrenze.IsEnabled = false;
            textBox_zugdaten_dehnhöchstkraft.IsEnabled = false;
            textBox_bruchdehnung.IsEnabled = false;
            textBox_streckengrenzverhältnis.IsEnabled = false;
            textBox_abweichungNenngewicht.IsEnabled = false;


            textBox_Hnm_1.IsEnabled = false;
            textBox_Hnm_2.IsEnabled = false;
            textBox_Hnm_3.IsEnabled = false;

            textBox_Hn25_1.IsEnabled = false;
            textBox_Hn25_2.IsEnabled = false;
            textBox_Hn25_3.IsEnabled = false;

            textBox_Hn75_1.IsEnabled = false;
            textBox_Hn75_2.IsEnabled = false;
            textBox_Hn75_3.IsEnabled = false;

            textBox_c1.IsEnabled = false;
            textBox_c2.IsEnabled = false;
            textBox_c3.IsEnabled = false;

            textBox_se_1.IsEnabled = false;
            textBox_se_2.IsEnabled = false;
            textBox_se_3.IsEnabled = false;

            textBox_fr.IsEnabled = false;
            button_fr_berechnen.IsEnabled = false;
        }

        private void button_fr_berechnen_Click(object sender, RoutedEventArgs e)
        {
            double ds = Convert.ToDouble(textBox_zugdaten_durchmesser.Text);

            double se1 = Convert.ToDouble(textBox_se_1.Text);
            double se2 = Convert.ToDouble(textBox_se_2.Text);
            double se3 = Convert.ToDouble(textBox_se_3.Text);

            double es = se1 + se2 + se3;

            double H1m = Convert.ToDouble(textBox_Hnm_1.Text);
            double H2m = Convert.ToDouble(textBox_Hnm_2.Text);
            double H3m = Convert.ToDouble(textBox_Hnm_3.Text);

            double h = (H1m + H2m + H3m) / 3;

            double H125 = Convert.ToDouble(textBox_Hn25_1.Text);
            double H225 = Convert.ToDouble(textBox_Hn25_2.Text);
            double H325 = Convert.ToDouble(textBox_Hn25_3.Text);

            double h14 = (H125 + H225 + H325) / 3;

            double H175 = Convert.ToDouble(textBox_Hn75_1.Text);
            double H275 = Convert.ToDouble(textBox_Hn75_2.Text);
            double H375 = Convert.ToDouble(textBox_Hn75_3.Text);

            double h34 = (H175 + H275 + H375) / 3;



            double c = (Convert.ToDouble(textBox_c1.Text) + Convert.ToDouble(textBox_c2.Text) + Convert.ToDouble(textBox_c3.Text)) / 3;


            double fr = ((ds * Math.PI - es) * (h + 2 * (h14 + h34))) / (6 * ds * Math.PI * c);

            textBox_fr.Text = fr.ToString();
        }

        private void dataGrid_zugversuche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_zug_bearbeiten.IsEnabled = true;
            }
            else
            {
                button_zug_bearbeiten.IsEnabled = false;
            }


            if (dataGrid_zugversuche.SelectedItem != null)
            {
                textBox_zugdaten_durchmesser.Text =         Erweiterungen.Helper.GetStringFromDataGrid(2, dataGrid_zugversuche);
                textBox_zugdaten_masse.Text =               Erweiterungen.Helper.GetStringFromDataGrid(3, dataGrid_zugversuche);
                comboBox_zugdaten_richtung.SelectedValue =  Erweiterungen.Helper.GetStringFromDataGrid(4, dataGrid_zugversuche);
                textBox_zugfestigkeit.Text =                Erweiterungen.Helper.GetStringFromDataGrid(5, dataGrid_zugversuche);
                textBox_dehngrenze.Text =                   Erweiterungen.Helper.GetStringFromDataGrid(6, dataGrid_zugversuche);
                textBox_zugdaten_dehnhöchstkraft.Text =     Erweiterungen.Helper.GetStringFromDataGrid(7, dataGrid_zugversuche);
                textBox_bruchdehnung.Text =                 Erweiterungen.Helper.GetStringFromDataGrid(8, dataGrid_zugversuche);
                textBox_streckengrenzverhältnis.Text =      Erweiterungen.Helper.GetStringFromDataGrid(9, dataGrid_zugversuche);
                textBox_abweichungNenngewicht.Text =        Erweiterungen.Helper.GetStringFromDataGrid(10, dataGrid_zugversuche);


                textBox_Hnm_1.Text = Erweiterungen.Helper.GetStringFromDataGrid(11, dataGrid_zugversuche);
                textBox_Hnm_2.Text = Erweiterungen.Helper.GetStringFromDataGrid(12, dataGrid_zugversuche);
                textBox_Hnm_3.Text = Erweiterungen.Helper.GetStringFromDataGrid(13, dataGrid_zugversuche);

                textBox_Hn25_1.Text = Erweiterungen.Helper.GetStringFromDataGrid(17, dataGrid_zugversuche);
                textBox_Hn25_2.Text = Erweiterungen.Helper.GetStringFromDataGrid(18, dataGrid_zugversuche);
                textBox_Hn25_3.Text = Erweiterungen.Helper.GetStringFromDataGrid(19, dataGrid_zugversuche);

                textBox_Hn75_1.Text = Erweiterungen.Helper.GetStringFromDataGrid(20, dataGrid_zugversuche);
                textBox_Hn75_2.Text = Erweiterungen.Helper.GetStringFromDataGrid(21, dataGrid_zugversuche);
                textBox_Hn75_3.Text = Erweiterungen.Helper.GetStringFromDataGrid(22, dataGrid_zugversuche);

                textBox_c1.Text =       Erweiterungen.Helper.GetStringFromDataGrid(23, dataGrid_zugversuche);
                textBox_c2.Text =       Erweiterungen.Helper.GetStringFromDataGrid(24, dataGrid_zugversuche);
                textBox_c3.Text =       Erweiterungen.Helper.GetStringFromDataGrid(25, dataGrid_zugversuche);
                textBox_se_1.Text =     Erweiterungen.Helper.GetStringFromDataGrid(14, dataGrid_zugversuche);
                textBox_se_2.Text =     Erweiterungen.Helper.GetStringFromDataGrid(15, dataGrid_zugversuche);
                textBox_se_3.Text =     Erweiterungen.Helper.GetStringFromDataGrid(16, dataGrid_zugversuche);

                textBox_fr.Text =       Erweiterungen.Helper.GetStringFromDataGrid(26, dataGrid_zugversuche);
               
            }

            //textBox_scherdaten_durchmesser.IsEnabled = false;
            //comboBox_scherdaten_richtung.IsEnabled = false;
            //textBox_scherdaten_hoechstkraft.IsEnabled = false;
            //textBox_scherdaten_scherwert.IsEnabled = false;
        }
        private void dataGrid_zugversuche_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            foreach (var t in TgW)
            {
                if (t.Item1 == e.Row.GetIndex())
                {
                    e.Row.Background = new SolidColorBrush(Colors.Red);

                    foreach (var c in dataGrid_zugversuche.Columns)
                    {
                        if (c.Header.ToString() == t.Item2)
                        {
                            //DataGridCell el = c.GetCellContent(e.Row).Parent as DataGridCell;

                            //if (el != null)
                            //{
                            //   // DataGridCell cell = (DataGridCell)result;

                            //    el.Foreground = new SolidColorBrush(Colors.Blue);
                            //}
                        }
                    }

                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }
        private void dataGrid_zugversuche_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime) || e.Column.Header.ToString() == "Datum")
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }


        }

        private void datagrid_zugversuche_LoadData()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            //Läd Scherungen
            var sch = from s in db.Zug
                      where s.Id_Matten == m.Id
                      select new
                      {
                          s.Id,
                          s.Datum,
                          s.D,
                          s.M,
                          s.Richtung,
                          s.Rm,
                          s.Rp,
                          s.Agt,
                          s.A, 
                          s.RmRp,
                          s.Dgs, //10


                          s.H1m,
                          s.H2m,
                          s.H3m,

                          s.se1, //15
                          s.se2,
                          s.se3,

                          s.H125,
                          s.H225,
                          s.H325,
                          s.H175,
                          s.H275,
                          s.H375,

                          s.c1,
                          s.c2,
                          s.c3,
                          s.fR
                      };

            int i = 0;

            var dur = from d in db.Grenzwert
                      select d;



            TgW.Clear();
            foreach (var sc in sch)
            {
                foreach (var du in dur)
                {

                        if (du.Feld == "Durchmesser" && (sc.D < du.Min || sc.D > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Durchmesser"));
                        }
                        if (du.Feld == "Zugfestigkeit" && (sc.Rm < du.Min || sc.Rm > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Zugfestigkeit"));
                        }
                        if (du.Feld == "Dehngrenze" && (sc.Rp < du.Min || sc.Rp > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Dehngrenze"));
                        }
                        if (du.Feld == "Bruchdehnung" && (sc.A < du.Min || sc.A > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Bruchdehnung"));
                        }
                        if (du.Feld == "Streckengrenzverhältnis" && (sc.RmRp < du.Min || sc.RmRp > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Streckgrenzverhältnis"));
                        }
                        if (du.Feld == "AbweichungNenngewicht" && (sc.Dgs < du.Min || sc.Dgs > du.Max))
                        {
                            TgW.Add(new Tuple<int, string>(i, "Abweichung Nenngewicht"));
                        }
                    
                }
                i++;
            }

            dataGrid_zugversuche.ItemsSource = sch.ToList();
        }


    }
}
