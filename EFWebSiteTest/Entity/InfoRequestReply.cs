using System;
using System.ComponentModel.DataAnnotations;

namespace EFWebSiteTest
{
    /// <summary>
    /// represents a reply to an InfoRequest. The replier can be a user or a brand.
    /// </summary>
    public class InfoRequestReply : EntityBase
    {
        public int InfoRequestId { get; set; }
        public int? AccountId { get; set; }
        [MaxLength(50)]
        public string ReplyText{ get; set; }
        public DateTime InsertDate { get; set; }


        public Account Account { get; set; }
        public InfoRequest InfoRequest { get; set; }
    }
}
