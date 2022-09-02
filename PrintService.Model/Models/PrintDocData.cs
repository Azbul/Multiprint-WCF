using System.Runtime.Serialization;

namespace PrintService.Model
{
    [DataContract]
    public class PrintDocData
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public int BytesRead { get; set; }

        [DataMember]
        public byte[] Buffer{ get; set; }
    }
}