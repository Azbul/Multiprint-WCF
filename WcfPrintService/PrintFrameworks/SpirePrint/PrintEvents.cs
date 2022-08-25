using System.Linq;
using Spire.Pdf.Print;
using System.Drawing.Printing;

namespace WcfPrintService
{
    public class PrintEvents
    {
        string _printerName;

        public PrintEvents()
        {

        }

        private void PrintingInProgress(object sender, PrintEventArgs e)
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

        private void PrinterReady(object sender, PrintEventArgs e)
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

        public void SetEventsToPrintDocs(PdfPrintSettings settings, string printerName)
        {
            _printerName = printerName;
            settings.BeginPrint += new PrintEventHandler(PrintingInProgress);
            settings.EndPrint += new PrintEventHandler(PrinterReady);
        }
    }
}