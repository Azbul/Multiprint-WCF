using System;
using Ext.Net;
using System.Web;
using PrintService.Model;
using System.Collections;
using System.ServiceModel;
using PrintService.Model.Interfaces;

namespace WebPrint
{
    public partial class PrintPage : System.Web.UI.Page
    {

        private IPrintService _printService;
        private string _selectedPrinterName { get; set; }
        private string _selectedFileName { get; set; }

        public PrintPage()
        {
            _printService = new PrintServiceClient().GetPrintService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                FillPrinterGrid(); 
                FillQueueGrids();  
                X.Msg.Info("Инфо", "Первая загрузка").Show();
            }
        }

        protected void OnComboBoxSelected(object sender, DirectEventArgs e)
        {
            SetPrintersStatusAndPcName();
        }

        protected void Print_Click(object sender, DirectEventArgs e)
        {
            if(IsInputDataCorrect)
            {
                bool uploadSucc = UploadFile();
                if (!uploadSucc) return;

                AddPrintQueue();   
                FillQueueGrids();
                string pages = X.GetCmp<TextField>("PagesField").Text;

                _printService.Print(_selectedFileName, _selectedPrinterName, pages);
            }
        }

        protected void Refresh_Click(object sender, DirectEventArgs e)
        {
            FillPrinterGrid();
            X.Msg.Info("Инфо", "Данные принтеров обновлены").Show();
        }

        protected void RefreshAllQueueTable_Click(object sender, DirectEventArgs e)
        {
            FillQueueGrids();
            X.Msg.Info("Инфо", "Данные таблиц обновлены").Show();

        }

        private void FillQueueGrids()
        {
            try
            {
                var queues = _printService.GetPrintQueues();

                var queuesArrayList = new ArrayList();
                var myFilesArrayList = new ArrayList();

                foreach (var queue in queues)
                {
                    queuesArrayList.Add(
                        new
                        {
                            qid = queue.Id,
                            printpages = queue.PagesToPrint,
                            printerid = queue.Printer.Id,
                            docname = queue.FileName,
                            filestatus = queue.FileStatus,
                            printedcomfim = queue.PrintedConfirm,
                            pcname = queue.QueueSystemName,
                            datetime = queue.PrintDateTime
                        });

                    if (queue.QueueSystemName == Environment.MachineName)
                    {
                        myFilesArrayList.Add(
                            new
                            {
                                docname = queue.FileName,
                                filestatus = queue.FileStatus,
                                prname = queue.Printer.PrinterName,
                                pagetoprint = queue.PagesToPrint,
                                pcname = queue.Printer.SystemName,
                                datetime = queue.PrintDateTime
                            });
                    }
                }

                BindArrayListToStore("QueueStore", queuesArrayList);
                BindArrayListToStore("MyFilesStore", myFilesArrayList);

            }
            catch (Exception ex)
            {
                X.Msg.Alert("Ошибка заполнения таблиц очередей файлов", ex.Message).Show();
            }
        }

        private void FillPrinterGrid()
        {
            try
            {
                var printers = _printService.GetPrinters();
                var printersArrayList = new ArrayList();

                printers.ForEach(p => printersArrayList.Add(
                    new
                    {
                        pid = p.Id,
                        prname = p.PrinterName,
                        pcname = p.SystemName,
                        status = p.PrinterStatus
                    }));

                BindArrayListToStore("PrinterStore", printersArrayList);
            }
            catch (Exception ex)
            {
                X.Msg.Alert("Ошибка заполнения таблицы принтеров", ex.Message).Show();
            }
        }

        private void BindArrayListToStore(string storeId, ArrayList array)
        {
            var store = X.GetCmp<Store>(storeId);
            store.DataSource = array;
            store.DataBind();
        }

        private void SetPrintersStatusAndPcName()
        {
            try
            {
                var printerStatusField = X.GetCmp<TextField>("StatusField");
                var pcNameField = X.GetCmp<TextField>("PcNameField");
                _selectedPrinterName = X.GetCmp<ComboBox>("SelectPrinterComboBox").SelectedItem.Value;
                var selectedPrinter = _printService.GetPrinterByName(_selectedPrinterName);

                printerStatusField.Text = selectedPrinter.PrinterStatus;
                pcNameField.Text = selectedPrinter.SystemName;
            }
            catch (Exception ex)
            {
                X.Msg.Alert("Ошибка установки имени принтера и ее системы", ex.Message).Show();
            }
        }

        private void AddPrintQueue()
        {
            var isAllPages = X.GetCmp<Radio>("AllPages").Checked;
            var pages = X.GetCmp<TextField>("PagesField").Text;
            var fileName = _selectedFileName;
            var printerName = _selectedPrinterName;

            _printService.AddPrintQueue(
                new PrintQueue
                {
                    Printer = _printService.GetPrinterByName(printerName),
                    PagesToPrint = isAllPages ? null : pages,
                    FileName = fileName,
                    FileStatus = 0, // на очереди - 0 / печать... - 1 / распечатан - 2 
                    QueueSystemName = Environment.MachineName,
                    PrintDateTime = DateTime.Now.ToString()
                });
        }

        private bool UploadFile()
        {
            bool succ = false;

            try
            {
                HttpPostedFile file = X.GetCmp<FileUploadField>("UploadField").PostedFile;
                var buff = new byte[file.InputStream.Length];
                
                file.InputStream.Read(buff, 0, buff.Length);
                file.InputStream.Close();


                _selectedFileName = file.FileName;
                succ = _printService.Upload(_selectedFileName, buff);
            }
            catch (Exception ex)
            {
                X.Msg.Alert("Ошибка загрузки файла", ex.Message).Show();
            }

            return succ;
        }

        private bool HasSelectedFile => X.GetCmp<FileUploadField>("UploadField").HasFile;

        private bool IsPrinterSelected => !(X.GetCmp<ComboBox>("SelectPrinterComboBox").IsEmpty);

        private bool IsInputDataCorrect
        {
            get
            {
                var logField = X.GetCmp<TextField>("LogField");
                if (!HasSelectedFile)
                {
                    logField.Text = "Выберите файл!";
                    return false;
                }

                if (!IsPrinterSelected)
                {
                    logField.Text = "Выберите принтер!";
                    return false;
                }

                return true;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            ((ICommunicationObject)_printService).Close();
        }
    }
}