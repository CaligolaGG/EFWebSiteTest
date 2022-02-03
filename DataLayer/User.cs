using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// represent a user registered to the website.
    /// </summary>
    public class User : EntityBase
    {
        public int AccountId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        public Account Account { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
