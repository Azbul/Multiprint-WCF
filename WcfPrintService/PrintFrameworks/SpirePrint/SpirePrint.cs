using System;
using Spire.Pdf;
using System.Management;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WcfPrintService
{
    public class SpirePrint
    {
        public void PrintAllPages(string fileName, string printerName)
        {
            PdfDocument pdfDoc = new PdfDocument();
            
            pdfDoc.LoadFromFile(fileName);
            pdfDoc.PrinterName = printerName;
            new PrintEvents().SetEventsToPrintDocs(pdfDoc.PrintDocument, printerName);
            pdfDoc.PrintDocument.Print();
        }

        public void PrintSelectedPages(string  fileName, string printerName, string pages)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            doc.PrinterName = printerName;
            SetPrintPages(pages, doc);
            new PrintEvents().SetEventsToPrintDocs(doc.PrintDocument, printerName);
            doc.PrintDocument.Print();
        }

        private void SetPrintPages(string pages, PdfDocument doc)
        {
            PrintDialog dialogPrint = new PrintDialog();
            dialogPrint.AllowPrintToFile = true;
            dialogPrint.AllowSomePages = true;
            dialogPrint.PrinterSettings.MinimumPage = 1;
            dialogPrint.PrinterSettings.MaximumPage = doc.Pages.Count;

            if (pages.Contains("-"))
            {
                string[] pagesSplited = pages.Split('-');
                dialogPrint.PrinterSettings.FromPage = Convert.ToInt32(pagesSplited[0]);
                dialogPrint.PrinterSettings.ToPage = Convert.ToInt32(pagesSplited[2]);
            }
            else
            {
                dialogPrint.PrinterSettings.FromPage = Convert.ToInt32(pages);
                dialogPrint.PrinterSettings.ToPage = Convert.ToInt32(pages);
            }

            doc.PrintFromPage = dialogPrint.PrinterSettings.FromPage;
            doc.PrintToPage = dialogPrint.PrinterSettings.ToPage;
        }

        public List<Printer> GetAllPrinters()
        {
            List<Printer> printers = new List<Printer>();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Printer");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection printersCollection = searcher.Get();

            if (printersCollection != null && printersCollection.Count > 0)
            {
                foreach (var printer in printersCollection)
                {
                    printers.Add(new Printer
                    {
                        Prn_name = printer["Name"].ToString(),
                        Pc_name = printer["SystemName"].ToString(),
                        Status = printer.Properties["PrinterStatus"].Value.ToString(),
                        Islocal = Convert.ToBoolean(printer.Properties["Local"].Value)
                    });
                }
            }

            return printers;
        }
    }
}