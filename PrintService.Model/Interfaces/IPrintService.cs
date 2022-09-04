using System.ServiceModel;
using System.Collections.Generic;

namespace PrintService.Model.Interfaces
{
    [ServiceContract]
    public interface IPrintService
    {
        [OperationContract]
        bool Upload(string fileName, byte[] fileBytes);

        [OperationContract]
        List<Printer> GetPrinters();

        [OperationContract]
        Printer GetPrinterByName(string name);

        [OperationContract]
        void AddPrintQueue(PrintQueue queue);

        [OperationContract]
        List<PrintQueue> GetPrintQueues();

        [OperationContract]
        void Print(string fileName, string printerName, string pages);
    }
}