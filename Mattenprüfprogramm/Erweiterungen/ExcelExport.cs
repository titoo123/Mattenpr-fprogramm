using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;


namespace Mattenprüfprogramm.Erweiterungen
{
    class ExcelExport
    {
        private Matten matten;
        private MainWindow mainWindow;
        private Application excelApp;
        private String fileString = Directory.GetCurrentDirectory() + @"\Pruefbogen_V04.3.xlsx";
        
        public ExcelExport(Matten matten, MainWindow mainWindow)
        {
            // TODO: Complete member initialization
            this.matten = matten;
            this.mainWindow = mainWindow;

            excelApp = new Application();
            //excelApp.ScreenUpdating = false;

            OpenExcelWorkbook();

        }

        private void OpenExcelWorkbook()
        {

            try
            {
                Workbook wb = excelApp.Workbooks.Open(fileString,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

                WriteMatToExcel(wb);

                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Konnte Excel Mappe nicht öffnen!\n\n" + fileString + "\n\n" + Directory.GetCurrentDirectory() + "\n\n" + ex, "Nee!");
            }

            
        }

        private void WriteMatToExcel(Workbook wb)
        {
            try
            {
                Worksheet ws = excelApp.ActiveSheet;
                ws.Cells[4, 2] = matten.Prüfnummer;
                ws.Cells[5, 2] = matten.Schweissmaschine.Name;
                ws.Cells[6, 2] = matten.Mattentypen.Name;

                ws.Cells[4, 13] = matten.Datum;
                ws.Cells[5, 13] = matten.Prüfer.Name;

                ws.Cells[56, 5] = matten.Anzahl_l;
                ws.Cells[57, 5] = matten.Anzahl_r;
                ws.Cells[58, 5] = matten.Anzahl_q;

                ws.Cells[56, 7] = matten.Fehlversuche_l;
                ws.Cells[57, 7] = matten.Fehlversuche_r;
                ws.Cells[58, 7] = matten.Fehlversuche_q;

                
                for (int i = 0; i < matten.Zug.Count; i++)
                {
                    switch (matten.Zug[i].Richtung)
                    {
                        case "Längs":
                        
                            ws.Cells[11 + i, 3] = matten.Zug[i].D;
                            ws.Cells[11 + i, 4] = matten.Zug[i].RmRp;
                            ws.Cells[11 + i, 5] = matten.Zug[i].Rm;
                            ws.Cells[11 + i, 6] = (matten.Zug[i].Rm / matten.Zug[i].RmRp);
                            ws.Cells[11 + i, 7] = matten.Zug[i].Agt;
                            ws.Cells[11 + i, 8] = "In Arbeit!";
                            ws.Cells[11 + i, 9] = matten.Zug[i].Dgs;
                            ws.Cells[11 + i, 10] = matten.Zug[i].fR;

                            break;
                        case "Quer":

                            ws.Cells[36 + i, 3] = matten.Zug[i].D;
                            ws.Cells[36 + i, 4] = matten.Zug[i].RmRp;
                            ws.Cells[36 + i, 5] = matten.Zug[i].Rm;
                            ws.Cells[36 + i, 6] = (matten.Zug[i].Rm / matten.Zug[i].RmRp);
                            ws.Cells[36 + i, 7] = matten.Zug[i].Agt;
                            ws.Cells[36 + i, 8] = "In Arbeit!";
                            ws.Cells[36 + i, 9] = matten.Zug[i].Dgs;
                            ws.Cells[36 + i, 10] = matten.Zug[i].fR;

                            break;
                        case "Rand":
                            
                            ws.Cells[42 + i, 3] = matten.Zug[i].D;
                            ws.Cells[42 + i, 4] = matten.Zug[i].RmRp;
                            ws.Cells[42 + i, 5] = matten.Zug[i].Rm;
                            ws.Cells[42 + i, 6] = (matten.Zug[i].Rm / matten.Zug[i].RmRp);
                            ws.Cells[42 + i, 7] = matten.Zug[i].Agt;
                            ws.Cells[42 + i, 8] = "In Arbeit!";
                            ws.Cells[42 + i, 9] = matten.Zug[i].Dgs;
                            ws.Cells[42 + i, 10] = matten.Zug[i].fR;

                            break;
                        default:    
                            break;
                    }
                    
                }
                for (int j = 0; j < matten.Scherung.Count; j++)
                {

                    switch (matten.Scherung[j].Richtung)
                    {
                        case "Längs":
                   
                            ws.Cells[11 + j, 11] = matten.Scherung[j].D;
                            ws.Cells[11 + j, 13] = "In Arbeit!";
                            ws.Cells[11 + j, 15] = "In Arbeit!";

                            break;
                        case "Quer":

                            ws.Cells[36 + j, 11] = matten.Scherung[j].D;
                            ws.Cells[36 + j, 13] = "In Arbeit!";
                            ws.Cells[36 + j, 15] = "In Arbeit!";
                            break;
                        case "Rand":

                            ws.Cells[42 + j, 11] = matten.Scherung[j].D;
                            ws.Cells[42 + j, 13] = "In Arbeit!";
                            ws.Cells[42 + j, 15] = "In Arbeit!";
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Konnte Excel Mappe nicht bearbeiten!\n\nBitte schließen sie alle Excel Anwendungen!" + ex, "Nee!");
   
            }
        

        }
    }
}
