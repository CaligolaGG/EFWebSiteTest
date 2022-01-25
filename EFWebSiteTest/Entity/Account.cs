using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFWebSiteTest
{
    /// <summary>
    /// represents a basic account.
    /// </summary>
    public class Account:EntityBase
    {
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// specifies the type of account. 1 for User, 2 for Brand.
        /// </summary>
        public byte AccountType { get; set; } 

        public User User { get; set; }
        public Brand Brand { get; set; }
        public ICollection<InfoRequestReply> InfoRequestReplies { get; set; }
    }
}
