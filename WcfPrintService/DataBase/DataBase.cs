using Logger;
using System;
using System.Linq;
using PrintService.Model;
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
                using (PrinterContext db = new PrinterContext())
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

        public Printer GetPrinterFromDbByName(string name)
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
                FileLogger.Log($"AN EXCEPTION THROW ON GetPrinterFromDbById: {ex}");
            }

            return printer;
        }

        public void UpdatePrintersInDB(List<Printer> printers)
        {
            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Printers]");
                    db.Printers.AddRange(printers);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THROW ON AddPrintersToDB: {ex}");
            }
        }

        public List<PrintQueue> GetPrintQueuesFromDB()
        {
            List<PrintQueue> queues = null;

            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    queues = db.PrintQueues.ToList();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THOW ON GetPrintersQueuesFromDB: {ex}");
            }

            return queues;
        }

        public void AddPrintQueueToDB(PrintQueue queue)
        {
            try
            {
                using (PrinterContext db = new PrinterContext())
                {
                    db.PrintQueues.Add(queue);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log($"AN EXCEPTION THOW ON AddPrintQueueToDB: {ex}");
            }
        }
    }
}