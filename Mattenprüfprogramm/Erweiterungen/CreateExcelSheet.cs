using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;


namespace Mattenprüfprogramm.Export
{
    class CreateExcelSheet
    {
        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
        /// <summary>
        /// Erstellt einen Excel Sheet
        /// </summary>
        /// <param name="d1">Datum 1</param>
        /// <param name="d2">Datum 2</param>
        /// <param name="visible">Sichtbar?</param>
        /// <param name="print">Drucken?</param>
        /// <param name="template">Vorlage?</param>
        public CreateExcelSheet(DateTime? d1, DateTime? d2, bool visible, bool print, bool template)
        {


            // Variablen deklarieren 
            Excel.Application myExcelApplication;
            Excel.Workbook myExcelWorkbook;
            Excel.Worksheet myExcelWorkSheet;
            myExcelApplication = null;

            //Ohne Vorlage
            if (!template)
            {
                try
                {
                    // First Contact: Excel Prozess initialisieren
                    myExcelApplication = new Excel.Application();

                    if (visible)
                    {
                        myExcelApplication.Visible = true;
                        myExcelApplication.ScreenUpdating = true;
                    }


                    // Excel Datei anlegen: Workbook
                    var myCount = myExcelApplication.Workbooks.Count;
                    myExcelWorkbook = (Excel.Workbook)(myExcelApplication.Workbooks.Add(System.Reflection.Missing.Value));
                    myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.ActiveSheet;

                    //Überschrift erstellen
                    myExcelWorkSheet = MakeHeaders(myExcelWorkSheet);


                    // myExcelWorkSheet.Name = "Prüfungen";
                    myExcelWorkSheet = MakeValues(myExcelWorkSheet, Convert.ToDateTime(d1), Convert.ToDateTime(d2), false);

                    if (print)
                    {
                        myExcelWorkSheet.Columns.Font.Size = 6;

                        myExcelWorkSheet.Columns.AutoFit();
                        //Print(myExcelWorkSheet, myExcelWorkbook);
                    }
                    else
                    {
                        myExcelWorkSheet.Columns.AutoFit();
                    }

                }
                catch (Exception ex)
                {
                    String myErrorString = "Drucken fehlgeschlagen: " + ex.Message;
                    MessageBox.Show(myErrorString);
                }
            }
            else //Mit Vorlage
            {
                Excel.Application excelApp = new Excel.Application();
                String fileString = Directory.GetCurrentDirectory() + @"\MATTE_Vorlage.xlsx";

                try
                {
                    Excel.Workbook wb = excelApp.Workbooks.Open(fileString,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);

                    if (visible)
                    {
                        excelApp.Visible = true;
                        //excelApp.ScreenUpdating = true;
                    }

                     wb = MakeValues(excelApp.ActiveSheet,Convert.ToDateTime(d1), Convert.ToDateTime( d2), true);

                }
                catch (Exception)
                {
                 //   System.Windows.MessageBox.Show("Es gibt Probleme mit dem Excel-Export!\n\n" + fileString + "\n\n" + Directory.GetCurrentDirectory() + "\n\n" + ex, "Nee!");
                }
            }
        }

        private void Print(Excel.Worksheet myExcelWorkSheet, Excel.Workbook myExcelWorkbook)
        {
            myExcelWorkSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            myExcelWorkSheet.PageSetup.LeftMargin = myExcelWorkSheet.PageSetup.LeftMargin / 4;
            myExcelWorkSheet.PrintOut(
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing);



            // Cleanup:
            GC.Collect();
            GC.WaitForPendingFinalizers();

            myExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            //Marshal.FinalReleaseComObject(myExcelWorkSheet);

            //myExcelWorkSheet.Close(false, Type.Missing, Type.Missing);
            //Marshal.FinalReleaseComObject(myExcelWorkSheet);

            //APP.Quit();
            //Marshal.FinalReleaseComObject(APP);
        }

        private Excel.Worksheet MakeHeaders(Excel.Worksheet w) {

            // Überschriften eingeben
            w.Cells[2, 2] = "Prüfer";
            w.Cells[2, 3] = "Mattentyp";
            w.Cells[2, 4] = "Herstellungsdatum";
            w.Cells[2, 5] = "Nenndurchmesser";

            w.Cells[2, 6] = "Querschnittsabweichung";        
            w.Cells[2, 7] = "Festigkeit Rp";       
            w.Cells[2, 8] = "Festigkeit Rm";      
            w.Cells[2, 9] = "Verhältnis Rm/Rp";
            w.Cells[2, 10] = "Verhältnis Rp ist/Rp nenn";


            w.Cells[2, 11] = "Agt %";     
            w.Cells[2, 12] = "Fs kN";
            w.Cells[2, 13] = "Fs %";

            w.Cells[2, 14] = "fR";
            w.Cells[2, 15] = "fR Delta %";

            //w.Cells[2, 14] = "se1";
            //w.Cells[2, 15] = "se2";
            //w.Cells[2, 16] = "se3";
            //w.Cells[2, 17] = "se4";

            //w.Cells[2, 18] = "a1-m";
            //w.Cells[2, 19] = "a2-m";
            //w.Cells[2, 20] = "a3-m";
            //w.Cells[2, 21] = "a4-m";

            //w.Cells[2, 22] = "a1-0,25";
            //w.Cells[2, 23] = "a2-0,25";
            //w.Cells[2, 24] = "a3-0,25";
            //w.Cells[2, 25] = "a4-0,25";

            //w.Cells[2, 26] = "a1-0,75";
            //w.Cells[2, 27] = "a2-0,75";
            //w.Cells[2, 28] = "a3-0,75";
            //w.Cells[2, 29] = "a4-0,75";

            //w.Cells[2, 30] = "C1";
            //w.Cells[2, 31] = "C2";
            //w.Cells[2, 32] = "C3";
            //w.Cells[2, 33] = "C4";

            // Formatieren der Überschrift
            Excel.Range myRangeHeadline;
            myRangeHeadline = w.get_Range("B2", "AA2");
            myRangeHeadline.Font.Bold = true;
            myRangeHeadline.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            myRangeHeadline.Borders.Weight = Excel.XlBorderWeight.xlThick;

            return w;
        }

        private Excel.Worksheet MakeValues(Excel.Worksheet w, DateTime d1, DateTime d2, bool template)
        {
            var exe = from x in d.Matten
                      where x.Datum <= d2 && x.Datum >= d1
                      select x;

            // Daten eingeben
            int j = 0;
            foreach (var i in exe)
            {
                //Sucht passende Züge
                var zug = from z in d.Zug
                          where z.Id_Matten == i.Id
                          select z;
                //Sucht passende Scherungen
                var sch = from s in d.Scherung
                          where s.Id_Matten == i.Id
                          select s;

                int z_n = zug.Count();
                int s_n = sch.Count();

                if (!template) //Ohne Vorlage
                {
                    foreach (Zug u in zug)
                    {
                        if (i.Prüfer != null)
                        {
                            w.Cells[j + 3, 2] = i.Prüfer.Name;
                        }
                        else
                        {
                            w.Cells[j + 3, 2] = "Unbekannt";
                        }

                        w.Cells[j + 3, 3] = i.Mattentypen.Name;
                        w.Cells[j + 3, 4] = i.Datum;
                        w.Cells[j + 3, 5] = u.D;
                        w.Cells[j + 3, 6] = u.A;
                        w.Cells[j + 3, 7] = u.Rp;
                        w.Cells[j + 3, 8] = u.Rm;
                        w.Cells[j + 3, 9] = u.Rm / u.Rp;
                        w.Cells[j + 3, 10] = u.Rp / 500;
                        w.Cells[j + 3, 11] = u.Agt;
                        w.Cells[j + 3, 14] = u.fR;
                        j = j + 1;
                    }
                    foreach (Scherung e in sch)
                    {
                        if (i.Prüfer != null)
                        {
                            w.Cells[j + 3, 2] = i.Prüfer.Name;
                        }
                        else
                        {
                            w.Cells[j + 3, 2] = "Unbekannt";
                        }
                        w.Cells[j + 3, 3] = i.Mattentypen.Name;
                        w.Cells[j + 3, 4] = i.Datum;
                        w.Cells[j + 3, 5] = e.D;

                        w.Cells[j + 3, 12] = e.Fm;
                        w.Cells[j + 3, 13] = e.Sw;
                        j = j + 1;
                    }


                }
                else //Mit Vorlage
                {
                    foreach (Zug u in zug)
                    {
                        w.Cells[j + 6, 6] = i.Mattentypen.Name;
                        w.Cells[j + 6, 7] = i.Datum;
                        w.Cells[j + 6, 8] = u.D;
                        w.Cells[j + 6, 9] = u.A;
                        w.Cells[j + 6, 10] = u.Rp;
                        w.Cells[j + 6, 11] = u.Rm;
                        w.Cells[j + 6, 12] = u.Rm / u.Rp;
                        w.Cells[j + 6, 13] = u.Rp / 500;
                        w.Cells[j + 6, 14] = u.Agt;
                        w.Cells[j + 6, 17] = u.fR;
                        j = j + 1;
                    }
                    foreach (Scherung e in sch)
                    {
                        w.Cells[j + 6, 6] = i.Mattentypen.Name;
                        w.Cells[j + 6, 7] = i.Datum;
                        w.Cells[j + 6, 8] = e.D;

                        w.Cells[j + 6, 15] = e.Fm;
                        w.Cells[j + 6, 16] = e.Sw;
                        j = j + 1;
                    }
                }


                //myExcelWorkSheet.Cells[j + 3, 10] = i.RmRe;
                //myExcelWorkSheet.Cells[j + 3, 11] = (i.Re/500);
                //myExcelWorkSheet.Cells[j + 3, 11] = i.A;
                //myExcelWorkSheet.Cells[j + 3, 12] = i.Agt;
                //myExcelWorkSheet.Cells[j + 3, 13] = i.fR;

                //myExcelWorkSheet.Cells[j + 3, 14] = i.se1;
                //myExcelWorkSheet.Cells[j + 3, 15] = i.se2;
                //myExcelWorkSheet.Cells[j + 3, 16] = i.se3;
                //myExcelWorkSheet.Cells[j + 3, 17] = i.se4;

                //myExcelWorkSheet.Cells[j + 3, 18] = i.a1m;
                //myExcelWorkSheet.Cells[j + 3, 19] = i.a2m;
                //myExcelWorkSheet.Cells[j + 3, 20] = i.a3m;
                //myExcelWorkSheet.Cells[j + 3, 21] = i.a4m;

                //myExcelWorkSheet.Cells[j + 3, 22] = i.a1_025;
                //myExcelWorkSheet.Cells[j + 3, 23] = i.a2_025;
                //myExcelWorkSheet.Cells[j + 3, 24] = i.a3_025;
                //myExcelWorkSheet.Cells[j + 3, 25] = i.a4_025;

                //myExcelWorkSheet.Cells[j + 3, 26] = i.a1_075;
                //myExcelWorkSheet.Cells[j + 3, 27] = i.a2_075;
                //myExcelWorkSheet.Cells[j + 3, 28] = i.a3_075;
                //myExcelWorkSheet.Cells[j + 3, 29] = i.a4_075;

                //myExcelWorkSheet.Cells[j + 3, 30] = i.c1;
                //myExcelWorkSheet.Cells[j + 3, 31] = i.c2;
                //myExcelWorkSheet.Cells[j + 3, 32] = i.c3;
                //myExcelWorkSheet.Cells[j + 3, 33] = i.c4;

                //myExcelWorkSheet.Cells[j + 3, 26] = i.C;
                //myExcelWorkSheet.Cells[j + 3, 27] = i.AgtM;
                
            }
            return w;
        }
    }
}
