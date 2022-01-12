using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class ReleaseCardFrontendDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ReleasedItemsCount { get; set; }
        public int ReleasedComponentsCount { get; set; }
        public DateTime ReleasedDate { get; set; }
        public PersonInfo Issuer { get; set; }
    }
}
