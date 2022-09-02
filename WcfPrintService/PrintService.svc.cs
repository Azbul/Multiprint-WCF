using System;
using System.Linq;
using PrintService.Model;
using System.Collections.Generic;
using PrintService.Model.Interfaces;

namespace WcfPrintService
{
    public class PrintService : IPrintService
    {
        private DataBase _db;
        private PrintFramework _printFramework;

        public PrintService()
        {
            _db = new DataBase();
            _printFramework = new SpirePrint();
        }

        public Printer GetPrinterByName(string name)
        {
            return _db.GetPrinterFromDbByName(name);
        }

        public List<Printer> GetPrinters()
        {
            return _db.GetPrintersFromDB().ToList();
        }

        public List<PrintQueue> GetPrintQueues()
        {
            return _db.GetPrintQueuesFromDB().ToList();
        }

        public void Print(string fileName, string printerName, string pages)
        {
            _printFramework.Print(fileName, printerName, pages);
        }

        public void AddPrintQueue(PrintQueue queue)
        {
            _db.AddPrintQueueToDB(queue);
        }

        public bool Upload(PrintDocData metadata)
        {
            throw new NotImplementedException();
        }
    }
}
