using System;
using System.Management;
using System.Collections.Generic;
using PrintService.Model;

namespace WcfPrintService
{
    public abstract class PrintFramework
    {
        protected string _filesPath => @"C:\Program Files (x86)\IIS Express\FilesToPrint\";

        public abstract void Print(string fileName, string printerName, string pages);

        public List<Printer> GetAllPrintersFromWin32Printer()
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
                        PrinterName = printer["Name"].ToString(),
                        SystemName = printer["SystemName"].ToString(),
                        PrinterStatus = printer.Properties["PrinterStatus"].Value.ToString() ?? "Unknown",
                        IslocalPrinter = Convert.ToBoolean(printer.Properties["Local"].Value)
                    });
                }
            }
            else
            {
                Logger.FileLogger.Log("The system did not detect connected printers");
            }

            return printers;
        }
    }
}
