using System;

namespace EFWebSiteTest
{
    public class InfoRequestReply : EntityBase
    {
        public int InfoRequestId { get; set; }
        public int? AccountId { get; set; }
        public string ReplyText{ get; set; }
        public DateTime InsertDate { get; set; }


        public Account Account { get; set; }
        public InfoRequest InfoRequest { get; set; }
    }
}
