using System.Data.Entity;

namespace WcfPrintService
{
    public class UserContext : DbContext
    {
        public UserContext() 
            : base("UserDB")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UserContext>());
        }

        public DbSet<Printer> Printers { get; set; }

        public DbSet<PrinterQueue> PrintersQueues { get; set; }
    }
}