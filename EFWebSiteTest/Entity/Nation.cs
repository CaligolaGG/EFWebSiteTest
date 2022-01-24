using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class Nation: EntityBase
    {
        public string Name { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
