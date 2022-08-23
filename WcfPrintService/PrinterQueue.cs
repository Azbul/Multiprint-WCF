namespace WcfPrintService
{
    public class PrinterQueue
    {
        public int Id { get; set; }

        public int FromPage { get; set; }

        public int ToPage { get; set; }

        public string PagesToPrint { get; set; } //пользовательские границы печати

        public int PrinterId { get; set; }  

        public string FileName { get; set; }

        public int? FileStatus { get; set; }

        public int? PapersPrinting { get; set; }

        public int? PrintedConfirm { get; set; }

        public string PcName { get; set; }

        public string DateOfPrinting { get; set; }
    }
}



