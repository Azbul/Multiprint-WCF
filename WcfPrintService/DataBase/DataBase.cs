using Logger;
using System;
using System.Linq;

namespace WcfPrintService
{
    public class DataBase
    {
        public IQueryable<Printer> GetPrintersFromDB()
        {
            IQueryable<Printer> printers = null;

            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    printers = db.Printers;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THROW ON GetPrintersFromDB: {ex}");
            }

            return printers;
        }

        public Printer GetPrinterFromDBByName(string name)
        {
            Printer printer = null;

            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    printer = db.Printers.First(x => x.PrinterName == name);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THROW ON GetPrinterFromDBByName: {ex}");
            }

            return printer;
        }

        public Printer GetPrinterFromDBById(int id)
        {
            Printer printer = null;

            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    printer = db.Printers.First(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THROW ON GetPrinterFromDBById: {ex}");
            }

            return printer;
        }

        public IQueryable<PrinterQueue> GetPrintersQueuesFromDB()
        {
            IQueryable<PrinterQueue> queues = null;

            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    queues = db.PrintersQueues;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THOW ON GetPrintersQueuesFromDB: {ex}");
            }

            return queues;
        }
    }
}