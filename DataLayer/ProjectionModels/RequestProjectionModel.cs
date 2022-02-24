using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RequestProjectionModel
    {
    }
    #region ProjectionModels

    public class InfoRequestBasic 
    { 
        public int Id { get; set; }
        public string UserFullName { get; set; }

    }


    /// <summary>
    ///  projection class that hold details of a InfoRequest, and the replies to that request
    /// </summary>
    /// 
    public class InfoRequestDetail : InfoRequestBasic
    {
        public int ProductId { get; set; }
        public string RequestText { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Email { get; set; }
        public string InfoUser { get; set; }

        public IEnumerable<RepliesTemp> Replies { get; set; }
    }

    /// <summary>
    /// projection  class of InfoRequestReply that holds some detail of a single request. Used for paging
    /// </summary>
    public class InfoRequestSelect : InfoRequestBasic
    {
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RequestText { get; set; }
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// projection class of infoRequest  for the ProductDetail class.
    /// Hold the id of the info request, name of the person who made the request, 
    /// number of replies to that request and the date of the last reply.
    /// </summary>
    public class InfoRequestTemp : InfoRequestBasic
    {
        public int RepliesCount { get; set; }
        public DateTime LastReply { get; set; }
    }






    /// <summary>
    /// projection class of InfoRequestReply for the RequestDetail class
    /// </summary>
    public class RepliesTemp
    {
        public int ReplyId { get; set; }
        public DateTime Date { get; set; }
        public string AccountName { get; set; }
        public string ReplyText { get; set; }

    }
    #endregion
}
