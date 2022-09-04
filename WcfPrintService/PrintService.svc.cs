using System;
using System.IO;
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
            AddPrintersFromWin32ToDB();
        }

        public Printer GetPrinterByName(string name)
        {
            return _db.GetPrinterFromDbByName(name);
        }

        public List<Printer> GetPrinters()
        {
            return _db.GetPrintersFromDB();
        }

        public List<PrintQueue> GetPrintQueues()
        {
            return _db.GetPrintQueuesFromDB();
        }

        public void Print(string fileName, string printerName, string pages)
        {
            _printFramework.Print(fileName, printerName, pages);
        }

        public void AddPrintQueue(PrintQueue queue)
        {
            _db.AddPrintQueueToDB(queue);
        }

        public bool Upload(string fileName, byte[] fileBytes)
        {
            bool success = false;
            try
            {
                string fullPath = Path.Combine(_printFramework.FilesPath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }
                success = true;
            }
            catch (Exception ex)
            {
                Logger.FileLogger.Log($"AN EXCEPTION THROW ON Upload: {ex}");
            }

            return success;
        }

        private void AddPrintersFromWin32ToDB()
        {
            var printersFromWin32 = _printFramework.GetAllPrintersFromWin32Printer();
            _db.UpdatePrintersInDB(printersFromWin32);
        }
    }
}
