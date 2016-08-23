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
    /// Interaktionslogik für MatteNeu.xaml
    /// </summary>
    public partial class MatteNeu : Window
    {
        MainWindow mat;
        public MatteNeu(MainWindow mat)
        {
            InitializeComponent();
            this.mat = mat;

            comboBox_maschine = Erweiterungen.Helper.LoadSchweissmaschine(comboBox_maschine);
            comboBox_mattentyp = Erweiterungen.Helper.LoadMattentypen(comboBox_mattentyp);
            comboBox_prüfer = Erweiterungen.Helper.LoadPrüfer(comboBox_prüfer);
        }

        private void button_SuS_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker_matte!= null && textBox_prüfnummer.Text != null && textBox_kommentar.Text != null 
                && comboBox_prüfer.SelectedIndex != -1 && comboBox_mattentyp.SelectedIndex != -1 && comboBox_maschine.SelectedIndex != -1)
            {
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                
                Matten m = new Matten(){ Datum = Convert.ToDateTime( datePicker_matte.SelectedDate), Kommentar = textBox_kommentar.Text,
                                         //Prüfnummer = Convert.to textBox_prüfnummer.Text, 
                                         Id_Maschine = Erweiterungen.Helper.GetSchweissmaschinenIdByName(Convert.ToString( comboBox_maschine.SelectedValue)),
                                         Id_Prüfer = Erweiterungen.Helper.GetPrüferIdByName(Convert.ToString(comboBox_prüfer.SelectedValue)),
                                         Id_Mattentyp = Erweiterungen.Helper.GetMattentypIdByName(Convert.ToString(comboBox_mattentyp.SelectedValue)),
                                         Tiefgerippt = Convert.ToBoolean( checkBox_tiefgerippt),
                };

                d.Matten.InsertOnSubmit(m);
                try
                {
                    d.SubmitChanges();
                    mat.dataGrid_matten_LoadData();
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Datenbankverbindung fehlgeschlagen!", "Nee!");
                }


            }
            else
            {
                MessageBox.Show("Bitte füllen sie alle Felder aus!","Nee!");
            }
        }
    }
}
