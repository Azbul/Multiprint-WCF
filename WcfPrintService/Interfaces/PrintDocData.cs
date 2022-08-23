using System.IO;
using System.Runtime.Serialization;

namespace WcfPrintService
{
    [DataContract]
    public class PrintDocData
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public Stream Stream { get; set; }
    }
}