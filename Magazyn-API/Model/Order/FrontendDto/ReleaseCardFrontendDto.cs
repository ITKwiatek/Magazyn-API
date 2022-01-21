using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class ReleaseCardFrontendDto : IRelease
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DeviceName { get; set; }
        public string GroupName { get; set; }
        public string ProjectName { get; set; }
        public int ReleasedItemsCount { get; set; }
        public int ReleasedComponentsCount { get; set; }
        public DateTime ReleasedDate { get; set; }
        public PersonInfo Issuer { get; set; }
        public string ReceiverInfo { get ; set; }
    }
}
