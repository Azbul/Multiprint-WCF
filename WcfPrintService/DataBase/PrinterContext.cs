using System.Data.Entity;

namespace WcfPrintService
{
    public class PrinterContext : DbContext
    {
        public PrinterContext() 
            : base("PrinterDB")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UserContext>());
        }

        public DbSet<Printer> Printers { get; set; }

        public DbSet<PrinterQueue> PrintersQueues { get; set; }
    }
}