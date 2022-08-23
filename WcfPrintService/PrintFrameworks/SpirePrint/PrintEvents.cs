using System.Linq;
using System.Drawing.Printing;

namespace WcfPrintService
{
    public class PrintEvents
    {
        string _printerName;

        public PrintEvents()
        {

        }

        private void BeginPrint(object sender, PrintEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                var printer = db.Printers.FirstOrDefault(p => p.Prn_name == _printerName);

                if (printer != null)
                {
                    printer.Status = "Идет печать...";
                    db.SaveChanges();
                }
            }
        }

        private void EndPrint(object sender, PrintEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                var printer = db.Printers.FirstOrDefault(p => p.Prn_name == _printerName);

                if (printer != null)
                {
                    printer.Status = "Готово";
                    db.SaveChanges();
                }
            }
        }

        public void SetEventsToPrintDocs(PrintDocument document, string printerName)
        {
            _printerName = printerName;
            document.BeginPrint += new PrintEventHandler(BeginPrint);
            document.EndPrint += new PrintEventHandler(EndPrint);
        }
    }
}