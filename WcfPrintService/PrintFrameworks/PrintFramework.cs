using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WcfPrintService
{
    abstract class PrintFramework
    {
        public abstract void PrintAllPages(string fileName, string printerName);

        public abstract void PrintSelectedPages(string fileName, string printerName, string pages);

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
