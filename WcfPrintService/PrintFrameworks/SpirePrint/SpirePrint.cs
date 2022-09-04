using System;
using Spire.Pdf;
using System.Collections.Generic;

namespace WcfPrintService
{
    public class SpirePrint : PrintFramework
    {
        public override void Print(string fileName, string printerName, string pages)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            doc.PrintSettings.PrinterName = printerName;

            if (!string.IsNullOrEmpty(pages))
                SetPrintPages(pages, doc);

            new PrintEvents().SetEventsToPrintDocs(doc.PrintSettings, printerName);
            doc.Print();
        }

        //Валидация pages на стороне клиента
        private void SetPrintPages(string pages, PdfDocument doc)
        {
            int fromPage = 0;
            int toPage = 0;

            bool isFromToPages = pages.Contains("-");
            bool isSomePages = pages.Contains(",");

            if (isFromToPages && isSomePages) // пример: 1-5, 8, 10 или 5, 10, 15-20
            {
                int[] somePages = GetFromToAndSomePagesAsArray(pages);
                doc.PrintSettings.SelectSomePages(somePages);
            }
            else if (isFromToPages) // пример: 5-10
            {
                string[] pagesSplited = pages.Split('-');
                fromPage = int.Parse(pagesSplited[0]);
                toPage = int.Parse(pagesSplited[2]);
                doc.PrintSettings.SelectPageRange(fromPage, toPage);
            }
            else if(isSomePages) // пример: 2, 5, 7
            {
                var splited = pages.Split(',');
                int[] pagesToPrint = Array.ConvertAll(splited, int.Parse);
                doc.PrintSettings.SelectSomePages(pagesToPrint);
            }
            else //если указана только одна определенная стр
            {   
                fromPage = int.Parse(pages);
                toPage = int.Parse(pages);
                doc.PrintSettings.SelectPageRange(fromPage, toPage);
            }
        }

        private int[] GetFromToAndSomePagesAsArray(string pages)
        {
            List<int> somePages = new List<int>();

            var splitedByComma = pages.Split(',');

            foreach (var page in splitedByComma)
            {
                if (page.Contains("-"))
                {
                    var pageSplited = page.Replace(" ", "").Split('-');
                    int start = int.Parse(pageSplited[0]);
                    int end = int.Parse(pageSplited[2]);

                    int intermediatePage = start;

                    while (intermediatePage <= end)
                    {
                        somePages.Add(intermediatePage);
                        intermediatePage++;
                    }
                }
                else
                    somePages.Add(int.Parse(page));
            }
            somePages.Sort();

            return somePages.ToArray();
        }
    }
}