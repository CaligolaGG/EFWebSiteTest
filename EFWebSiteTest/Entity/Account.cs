using System;
using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class Account:EntityBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public byte AccountType { get; set; } 

        public User User { get; set; }
        public Brand Brand { get; set; }
        public ICollection<InfoRequestReply> InfoRequestReplies { get; set; }
    }
}
