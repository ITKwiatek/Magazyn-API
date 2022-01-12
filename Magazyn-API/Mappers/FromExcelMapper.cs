using AutoMapper;
using Magazyn_API.Model.Excel;
using Magazyn_API.Model.Mappers;
using Magazyn_API.Model.Order.FromExcelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Mappers
{
    public class FromExcelMapper
    {
        private readonly IMapper _mapper;

        public FromExcelMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public OrderItemFromExcelDto OrderItem(IExcelOrderItem excelItem)
        {
            OrderItemFromExcelDto orderItem = new();
            orderItem.Component = _mapper.Map<ComponentModelFromExcelDto>(excelItem.ExcelComponent);
            orderItem.Quantity = excelItem.Count;

            return orderItem;
        }

        public OrderModelFromExcelDto Order(IExcelOrder eOrder)
        {
            OrderModelFromExcelDto order = new();
            order.DateToEP = eOrder.DateToEP;
            order.DateToWarehouse = eOrder.DateToWarehouse;
            order.DeviceName = eOrder.DeviceName;
            order.ProjectName = eOrder.ProjectName;
            foreach(var eI in eOrder.OrderItems)
            {
                var item = OrderItem(eI);
                order.OrderItems.Add(item);
            }
            return order;
        }
    }
}
