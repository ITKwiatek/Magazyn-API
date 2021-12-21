using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FromExcelDto
{
    public class ComponentModelFromExcelDto : IComponentModel
    {
        public string SAP { get; set; }
        public string Supplier { get; set; }
        public string ArticleNumber { get; set; }
        public string OrderingNumber { get; set; }
        public string Description { get; set; }
    }
}
