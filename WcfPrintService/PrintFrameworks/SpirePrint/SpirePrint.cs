using System;
using Spire.Pdf;

namespace WcfPrintService
{
    public class SpirePrint : PrintFramework
    {
        public override void PrintAllPages(string fileName, string printerName)
        {
            PdfDocument pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(fileName);
            pdfDoc.PrintSettings.PrinterName = printerName;
            new PrintEvents().SetEventsToPrintDocs(pdfDoc.PrintSettings, printerName);
            pdfDoc.Print();
        }

        public override void PrintSelectedPages(string fileName, string printerName, string pages)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            doc.PrintSettings.PrinterName = printerName;
            SetPrintPages(pages, doc);
            new PrintEvents().SetEventsToPrintDocs(doc.PrintSettings, printerName);
            doc.Print();
        }

        private void SetPrintPages(string pages, PdfDocument doc)
        {
            int fromPage = 0;
            int toPage = 0;

            bool isFromToPages = pages.Contains("-");
            bool isSomePages = pages.Contains(",");

            if (isFromToPages && isSomePages)
            {
                // + диапазон
               // doc.PrintSettings.SelectSomePages(new int[] { 1, 3, 5, 7 });
            }
            else if (isFromToPages)
            {
                string[] pagesSplited = pages.Split('-');
                fromPage = Convert.ToInt32(pagesSplited[0]);
                toPage = Convert.ToInt32(pagesSplited[2]);
                doc.PrintSettings.SelectPageRange(fromPage, toPage);
            }
            else if(isSomePages)
            {
                var splited = pages.Split(',');
                int[] pagesToPrint = Array.ConvertAll(splited, int.Parse);
                doc.PrintSettings.SelectSomePages(pagesToPrint);
            }
            else
            {
                //если указана только определенная стр
                fromPage = Convert.ToInt32(pages);
                toPage = Convert.ToInt32(pages);
                doc.PrintSettings.SelectPageRange(fromPage, toPage);
            }
        }
    }
}