using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public static class ExcelConverter
    {
        public static string ConvertXLS_XLSX(FileInfo file)
        {
            var app = new Microsoft.Office.Interop.Excel.Application();
            var xlsFile = "C:\\temp\\delete\\" + file.Name;
            var wb = app.Workbooks.Open(xlsFile);
            var xlsxFile = xlsFile + "x";
            wb.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            wb.Close();
            app.Quit();
            return xlsxFile;
        }
    }
}
