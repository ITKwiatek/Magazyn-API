using Magazyn_API.Model.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel
{
    public interface IExcelComponent : IComponentModel
    {
        public Cell SapCell { get; set; }
        public Cell SupplierCell { get; set; }
        public Cell ArticleNumberCell { get; set; }
        public Cell OrderingNumberCell { get; set; }
        public Cell DescriptonCell { get; set; }
    }
}
