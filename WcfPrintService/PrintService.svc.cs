using System;
using System.Collections.Generic;
using System.Linq;

namespace WcfPrintService
{
    public class PrintService : IPrintService
    {
        private DataBase _db;
        private PrintFramework _printFramework;

        public PrintService()
        {
            _db = new DataBase();
        }

        public PrintService(PrintFramework printFramework)
        {
            _db = new DataBase();
            _printFramework = printFramework;
        }

        public List<Printer> GetPrinters()
        {
            return _db.GetPrintersFromDB().ToList();
        }

        public List<PrinterQueue> GetPrintersQueues()
        {
            return _db.GetPrintersQueuesFromDB().ToList();
        }

        public void Print(string fileOrPath, int printerId, string pages)
        {
            throw new NotImplementedException();
        }

        public void SetQueueDataToDb(PrinterQueue pqueue)
        {
            throw new NotImplementedException();
        }

        public bool Upload(PrintDocData metadata)
        {
            throw new NotImplementedException();
        }
    }
}
