using Magazyn_API.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class ReleaseFrontendDto : IRelease
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PersonInfo Issuer { get; set; }
        public DateTime ReleasedDate { get; set; }
        public List<ReleaseItemFrontendDto> ReleaseItems { get; set; } = new();
        public DeviceFrontendDto Device { get; set; }
        public string ReceiverInfo { get; set; }
    }
}
