using System;
using System.IO;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace WcfPrintService
{
    public class WinspoolPrint : PrintFramework
    {
        public override void Print(string fileName, string printerName, string pages)
        {
            bool success = false;

            if (string.IsNullOrEmpty(pages))
                success = SendFileToPrinter(fileName, printerName);
            else
                success = PrintSelectedPages(fileName, printerName, pages);

            if(success == false)
            {
                int errCode = Marshal.GetLastWin32Error();
                Logger.FileLogger.Log($"При попытке печати файла {fileName} принтером {printerName} возникла ошибка. " +
                    $"Код ошибки (Win32ErrorCode): {errCode}");
            }
        }

        private bool PrintSelectedPages(string fileName, string printerName, string pages)
        {
            /*
              Repeat it for as many pages as necessary.
             if (StartPagePrinter(hPrinter))
                        {
                            success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }
             */
            return false;
        }

        #region Winspool methods

        private bool SendFileToPrinter(string fileName, string printerName)
        {
            try
            {
                PrintDocument pd = new PrintDocument();

                // TODO: Проверь как приходит имя принтера
                #region Get Connected Printer Name

                /* 
                StringBuilder dp = new StringBuilder(256);
                int size = dp.Capacity;
                 if (GetDefaultPrinter(dp, ref size))
                 {
                    pd.PrinterSettings.PrinterName = dp.ToString().Trim();
                 }
                */
                #endregion Get Connected Printer Name

                pd.PrinterSettings.PrinterName = printerName;

                string fullFilePath = Path.Combine(_filesPath, fileName);

                FileStream fs = new FileStream(fullFilePath, FileMode.Open);

                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = new Byte[fs.Length];
                bool success = false;

                IntPtr ptrUnmanagedBytes = new IntPtr(0);
                int nLength = Convert.ToInt32(fs.Length);
                bytes = br.ReadBytes(nLength);
                ptrUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
                Marshal.Copy(bytes, 0, ptrUnmanagedBytes, nLength);

                success = SendBytesToPrinter(pd.PrinterSettings.PrinterName, fileName, ptrUnmanagedBytes, nLength);
                Marshal.FreeCoTaskMem(ptrUnmanagedBytes);
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool SendBytesToPrinter(string szPrinterName, string fileName, IntPtr pBytes, int dwCount)
        {
            try
            {
                int dwWritten = 0;
                IntPtr hPrinter = new IntPtr(0);
                DOCINFOA di = new DOCINFOA();
                bool success = false; 

                di.pDocName = fileName;
                //di.pDataType = "RAW"; //Win7
                di.pDataType = "XPS_PASS"; //Win8+

                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        if (StartPagePrinter(hPrinter))
                        {
                            success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }
                        EndDocPrinter(hPrinter);
                    }
                    ClosePrinter(hPrinter);
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Winspool methods

        #region dll Wrappers
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int size);

        #endregion dll Wrappers

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
    }
}