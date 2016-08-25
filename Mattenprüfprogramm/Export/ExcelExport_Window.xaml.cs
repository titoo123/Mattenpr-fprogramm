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

using ElmMQDNetLib;

namespace Mattenprüfprogramm.Export
{
    /// <summary>
    /// Interaktionslogik für AS400Export.xaml
    /// </summary>
    public partial class ExcelExport_Window : Window
    {
        //von
        DateTime? d1;
        //bis
        DateTime? d2;

        public ExcelExport_Window()
        {
            InitializeComponent();
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            //Sind die Datepicker ausgefüllt?
            if (D1_Picker.SelectedDate != null && D2_Picker.SelectedDate != null && D1_Picker.SelectedDate < D2_Picker.SelectedDate)
            {
                //Konvertieren des Datums
                d1 = D1_Picker.SelectedDate;
                d2 = D2_Picker.SelectedDate;

                //Datenbankabfrage der Matten mit betreffenden Datum die noch nicht exportiert wurden
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                if (((ComboBoxItem)comboBox_excel_export.SelectedItem).Content.ToString() =="Einfach")
                {
                    new CreateExcelSheet(d1, d2, true, false,false);
                    this.Close();
                }
                else if (((ComboBoxItem)comboBox_excel_export.SelectedItem).Content.ToString() == "Vorlage")
                {
                    new CreateExcelSheet(d1, d2, true, false,true);
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("Bitte wählen sie für beide Datumsfelder ein Datum aus!", "Achtung!");
            }
        }

   
    }
}
