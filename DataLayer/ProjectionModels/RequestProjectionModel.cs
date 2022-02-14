using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RequestProjectionModel
    {
    }
    #region ProjectionModels
    /// <summary>
    ///  projection class that hold details of a InfoRequest, and the replies to that request
    /// </summary>
    public class RequestDetail
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public string RequestText { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string InfoUser { get; set; }

        public IEnumerable<RepliesTemp> Replies { get; set; }
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

    /// <summary>
    /// projection  class of InfoRequestReply that holds some detail of a single request
    /// </summary>
    public class RequestSelect
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RequestText { get; set; }
        public DateTime Date { get; set; }
    }
    #endregion
}
