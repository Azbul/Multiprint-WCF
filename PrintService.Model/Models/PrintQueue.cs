using System.ComponentModel.DataAnnotations;

namespace PrintService.Model
{
    public class PrintQueue
    {
        public int Id { get; set; }
        
        public string PagesToPrint { get; set; }

        [Required]
        public Printer Printer { get; set; }  

        [Required]
        [MaxLength(500)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(100)]
        public string QueueSystemName { get; set; }

        [Required]
        public int FileStatus { get; set; }

        public bool PrintedConfirm { get; set; }

        [Required]
        public string PrintDateTime { get; set; }
    }
}



