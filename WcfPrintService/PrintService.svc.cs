using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
            return _db.GetPrintersFromDB();
        }

        public List<PrinterQueue> GetPrintersQueues()
        {
            return _db.GetPrintersQueuesFromDB();
        }

        public void Print(string fileOrPath, int printerId, string pages)
        {
            throw new NotImplementedException();
        }

        public void SetQueueDataToDb(PrinterQueue pqueue)
        {
            throw new NotImplementedException();
        }

        public bool Upload(FileMetaData metadata)
        {
            throw new NotImplementedException();
        }
    }
}
