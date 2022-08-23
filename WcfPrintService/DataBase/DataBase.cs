using Logger;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WcfPrintService
{
    public class DataBase
    {
        public List<Printer> GetPrintersFromDB()
        {
            List<Printer> printers = null;

            try
            {
                using (UserContext db = new UserContext())
                {
                    printers = db.Printers.ToList();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THROW ON GetPrintersFromDB: {ex}");
            }

            return printers;
        }

        public List<PrinterQueue> GetPrintersQueuesFromDB()
        {
            List<PrinterQueue> queues = null;

            try
            {
                using (UserContext db = new UserContext())
                {
                    queues = db.PrintersQueues.ToList();
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