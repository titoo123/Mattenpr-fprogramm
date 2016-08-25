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
using System.Xml;



namespace Mattenprüfprogramm.Einstellungen
{
    /// <summary>
    /// Interaktionslogik für Import.xaml
    /// </summary>
    /// 
    public partial class Pfad_Window : Window
    {

        private XmlDocument xml = new XmlDocument();
        public Pfad_Window()
        {
            InitializeComponent();

            try
            {
                xml.Load(Benutzer.BenutzerVerwaltung.xml_Pfad);

                textBox_Pfad.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }

        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText = textBox_Pfad.Text;

            try
            {
                xml.Save(Benutzer.BenutzerVerwaltung.xml_Pfad);
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen konnten nicht gespeichert werden!", "Fehler!");
            }

            this.Close();
        }

        private void button_Pfad_auswählen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Pfad.Text = f.SelectedPath;
            }
        }
    }

    //public partial class Pfad_Window : Window
    //{

    //private XmlDocument xml = new XmlDocument();
    //private MainWindow m;

    //public Pfad_Window(MainWindow m)
    //{
    //    InitializeComponent();
    //    this.m = m;

    //    try
    //    {
    //        xml.Load(Benutzer.BenutzerVerwaltung.xml_Pfad);

    //        //textBox_zugindex.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Zug").InnerText;
    //        //textBox_scherindex.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Scherung").InnerText;
    //        //textBox_messindex.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Messprotokoll").InnerText;
    //        textBox_pfad.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
    //    }
    //    catch (Exception)
    //    {
    //        MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
    //    }

    //}

    //private void button_import_bearbeiten_Click(object sender, RoutedEventArgs e)
    //{
    //    //textBox_messindex.IsEnabled = true;
    //    //textBox_scherindex.IsEnabled = true;
    //    //textBox_zugindex.IsEnabled = true;
    //    button_import_speichern.IsEnabled = true;

    //    textBox_pfad.IsEnabled = true;
    //    button_pfad_auswählen.IsEnabled = true;
    //}

    //private void button_import_speichern_Click(object sender, RoutedEventArgs e)
    //{
    //    //xml.SelectSingleNode("Einstellungen").SelectSingleNode("Zug").InnerText = textBox_zugindex.Text;
    //    //xml.SelectSingleNode("Einstellungen").SelectSingleNode("Scherung").InnerText = textBox_scherindex.Text;
    //    //xml.SelectSingleNode("Einstellungen").SelectSingleNode("Messprotokoll").InnerText = textBox_messindex.Text;
    //    xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText = textBox_pfad.Text;

    //    try
    //    {
    //        xml.Save(Benutzer.BenutzerVerwaltung.xml_Pfad);
    //    }
    //    catch (Exception)
    //    {
    //        MessageBox.Show("Einstellungen konnten nicht gespeichert werden!", "Fehler!");
    //    }



    //    //textBox_messindex.IsEnabled = false;
    //    //textBox_scherindex.IsEnabled = false;
    //    //textBox_zugindex.IsEnabled = false;
    //    button_import_speichern.IsEnabled = false;

    //    textBox_pfad.IsEnabled = false;
    //    button_pfad_auswählen.IsEnabled = false;
    //}

    //private void button_pfad_auswählen_Click(object sender, RoutedEventArgs e)
    //{
    //    System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();
    //    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //    {
    //        textBox_pfad.Text = f.SelectedPath;
    //    }
    //}

    //private void button_importieren_Click(object sender, RoutedEventArgs e)
    //{
    //    Erweiterungen.ImportLoader i = new Erweiterungen.ImportLoader(m);
    //    i.Load(pBar);
    //}

    //private void pBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    //{

    //}
    //}
}
