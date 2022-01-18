using Magazyn_API.Model.Excel.GER_2;
using Magazyn_API.Model.Excel.GR_1;
using Magazyn_API.Model.Excel.PL_1;
using Magazyn_API.Model.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel
{
    public class ExcelFactory
    {
        public IExcelOrder GetOrder(ExcelTypes type)
        {
            switch(type)
            {
                case ExcelTypes.PL_1:
                    return new ExcelOrderPL_1();
                case ExcelTypes.GER_1:
                    return new ExcelOrderGER_1();
                case ExcelTypes.GER_2:
                    return new ExcelOrderGER_2();
            }
            throw new NotImplementedException();
        }

        public IExcelComponent GetComponent(ExcelTypes type)
        {
            switch (type)
            {
                case ExcelTypes.PL_1:
                    return new ExcelComponentPL_1();
                case ExcelTypes.GER_1:
                    return new ExcelComponentGER_1();
                case ExcelTypes.GER_2:
                    return new ExcelComponentGER_2();
            }
            throw new NotImplementedException();
        }

        public IExcelOrderItem GetOrderItem(ExcelTypes type)
        {
            switch (type)
            {
                case ExcelTypes.PL_1:
                    return new ExcelOrderItemPL_1();
                case ExcelTypes.GER_1:
                    return new ExcelOrderItemGER_1();
                case ExcelTypes.GER_2:
                    return new ExcelOrderItemGER_2();
            }
            throw new NotImplementedException();
        }
    }
}
