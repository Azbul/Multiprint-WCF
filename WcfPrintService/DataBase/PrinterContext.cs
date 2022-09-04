using System.Data.Entity;
using PrintService.Model;

namespace WcfPrintService
{
    public class PrinterContext : DbContext
    {
        public PrinterContext() 
            : base("PrinterDB")
        {

        }

        public DbSet<Printer> Printers { get; set; }

        public DbSet<PrintQueue> PrintQueues { get; set; }
    }
}