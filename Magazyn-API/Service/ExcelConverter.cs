//using Microsoft.Office.Interop.Excel;
using Spire.Xls;
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
            Workbook workbook = new Workbook();
            var xlsFile = "C:\\temp\\delete\\" + file.Name;
            var xlsxFile = xlsFile + "x";
            workbook.LoadFromFile(xlsFile);
            workbook.SaveToFile(xlsxFile, ExcelVersion.Version2013);

            return xlsxFile;   
        }

    }
}
