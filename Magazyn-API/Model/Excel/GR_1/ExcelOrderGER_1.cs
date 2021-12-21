﻿using Magazyn_API.Model.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel.GR_1
{
    public class ExcelOrderGER_1 : IExcelOrder, ITypeGER_1
    {
        public string ProjectName { get; set; }
        public string DeviceName { get; set; }
        public List<IExcelOrderItem> OrderItems { get; set; }
        public DateTime DateToEP { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Cell ProjectNameCell { get; set; } = new(2, 5);
        public Cell DeviceNameCell { get; set; } = new(2, 7);
        public Cell DateToEPCell { get; set; }
        public Cell DateToWarehouseCell { get; set; }
        public Cell ReleaseDateCell { get; set; }
    }
}