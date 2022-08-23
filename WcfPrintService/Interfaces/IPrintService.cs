using System.ServiceModel;
using System.Collections.Generic;

namespace WcfPrintService
{
    [ServiceContract]
    public interface IPrintService
    {
        [OperationContract]
        bool Upload(FileMetaData metadata);

        [OperationContract]
        List<Printer> GetPrinters();

        [OperationContract]
        void SetQueueDataToDb(PrinterQueue pqueue);

        [OperationContract]
        List<PrinterQueue> GetPrintersQueues();

        [OperationContract]
        void Print(string fileOrPath, int printerId, string pages);
    }
}