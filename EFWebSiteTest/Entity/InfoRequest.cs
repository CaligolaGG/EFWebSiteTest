using System;
using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class InfoRequest : EntityBase
    {
        public int? UserId { get; set; }
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public int NationId {get;set;}
        public string PhoneNumber { get; set; }
        public string Cap { get; set; }
        public string RequestText { get; set; }
        public DateTime InsertDate  { get; set; }

        public ICollection< InfoRequestReply> InfoRequestReplies { get; set; }
        public Nation Nation{ get; set; }
        public Product Product{ get; set; }
        public User User{ get; set; }

    }
}
