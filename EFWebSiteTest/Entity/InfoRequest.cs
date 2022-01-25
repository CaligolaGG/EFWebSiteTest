using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFWebSiteTest
{
    /// <summary>
    /// Represent a request of informations for a specific product from a user that can be either a guest
    /// or a registered user
    /// </summary>
    public class InfoRequest : EntityBase
    {
        /// <summary>
        /// id of the user who made the request. A null value represent a guest user.
        /// </summary>
        public int? UserId { get; set; }
        public int ProductId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string City { get; set; }
        public int NationId {get;set;}

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(5)]
        public string Cap { get; set; }
        public string RequestText { get; set; }
        public DateTime InsertDate  { get; set; }

        public ICollection< InfoRequestReply> InfoRequestReplies { get; set; }
        public Nation Nation{ get; set; }
        public Product Product{ get; set; }
        public User User{ get; set; }

    }
}
