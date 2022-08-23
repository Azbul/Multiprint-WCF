using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;

namespace WcfPrintService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        ReturnValue Upload(FileMetaData metadata);

        [OperationContract]
        void InitializePrintersToDb();

        [OperationContract]
        List<Printer> GetPrintersFromDb();

        [OperationContract]
        void SetQueueDataToDb(PrinterQueue pqueue);

        [OperationContract]
        List<PrinterQueue> GetPqueuesFromDb();

        [OperationContract]
        void Print(string fileOrPath, int printerId, string pages);     
    }

    [MessageContract]
    public class FileMetaData
    {
        [MessageHeader(MustUnderstand =true)]
        public string FileName { get; set; }

        [MessageBodyMember(Order =1)]
        public Stream Stream { get; set; }
    }

    [MessageContract]
    public class ReturnValue
    {
        [MessageBodyMember]
        public bool UploadSucceed { get; set; }
    }

}