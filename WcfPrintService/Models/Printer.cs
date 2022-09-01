using System.ComponentModel.DataAnnotations;

namespace WcfPrintService
{
    public class Printer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string PrinterName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PrinterStatus { get; set; }

        [Required]
        [MaxLength(100)]
        public string SystemName { get; set; }

        [Required]
        public bool IslocalPrinter { get; set; }
    }
}
