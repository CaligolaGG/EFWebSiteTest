using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// nationality of a person. Currently there are 20.
    /// </summary>
    public class Nation : EntityBase
    {
        public string Name { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
