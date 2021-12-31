using AutoMapper;
using Magazyn_API.Model.Excel.PL_1;
using Magazyn_API.Model.Mappers;
using Magazyn_API.Model.Order;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;



namespace Magazyn_API.Model.Excel
{
    public class ExcelReader
    {
        public string file { get; set; }

        public ExcelReader(string path)
        {
            file = path;
        }

        public async Task<IExcelOrder> LoadOrder(ExcelTypes type)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            FileStream stream = new(file, FileMode.Open, FileAccess.Read);

            await package.LoadAsync(stream);
            var ws = package.Workbook.Worksheets[0];

            ExcelFactory factory = new();
            var excelOrder = factory.GetOrder(type);

            LoadDates();
            LoadOrderInfo();
            LoadComponentsAndItems();

            closeConnection();
            return excelOrder;

            void LoadComponentsAndItems()
            {
                int row = factory.GetComponent(type).SapCell.Row;
                excelOrder.OrderItems = new List<IExcelOrderItem>();

                while (isRowNotNull(row))
                {
                    IExcelComponent c = factory.GetComponent(type);
                    IExcelOrderItem i = factory.GetOrderItem(type);
                    c.SAP = getCell(new Cell(row, c.SapCell.Col));
                    c.ArticleNumber = getCell(new Cell(row, c.ArticleNumberCell.Col));
                    c.Description = getCell(new Cell(row, c.DescriptonCell.Col));
                    c.OrderingNumber = getCell(new Cell(row, c.OrderingNumberCell.Col));
                    c.Supplier = getCell(new Cell(row, c.SupplierCell.Col));

                    i.Count = Int32.Parse(getCell(new Cell(row, i.CountCell.Col)));
                    i.ExcelComponent = c;
                    excelOrder.OrderItems.Add(i);
                    row++;
                }            
            }

            bool isRowNotNull(int row)
            {
                int colSAP = factory.GetComponent(type).SapCell.Col;
                int colSuppl = factory.GetComponent(type).SupplierCell.Col;
                int colArtNr = factory.GetComponent(type).ArticleNumberCell.Col;
                string SAP = getCell(new Cell(row, colSAP));
                string Suppl = getCell(new Cell(row, colSuppl));
                string artNr = getCell(new Cell(row, colArtNr));

                if (string.IsNullOrWhiteSpace(SAP) & string.IsNullOrWhiteSpace(Suppl) & string.IsNullOrWhiteSpace(artNr))
                    return false;
                return true;

            }

            void LoadOrderInfo()
            {
                excelOrder.ProjectName = getCell(excelOrder.ProjectNameCell);
                excelOrder.DeviceName = getCell(excelOrder.DeviceNameCell);
            }

            void LoadDates()
            {                
                if (excelOrder is IExcelOrderDates)
                {
                    if (!string.IsNullOrWhiteSpace(getCell(excelOrder.DateToEPCell)))
                        excelOrder.DateToEP = DateTime.Parse(getCell(excelOrder.DateToEPCell));

                    if (!string.IsNullOrWhiteSpace(getCell(excelOrder.DateToWarehouseCell)))
                        excelOrder.DateToWarehouse = DateTime.Parse(getCell(excelOrder.DateToWarehouseCell));
                }
            }

            string getCell(Cell cell)
            {
                return ws.Cells[cell.Row, cell.Col].Value?.ToString();
            }

            void closeConnection()
            {
                stream.Dispose();
                package.Dispose();
            }
        }
    }
}
