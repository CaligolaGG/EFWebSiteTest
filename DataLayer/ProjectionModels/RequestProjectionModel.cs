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
        public string AccountName { get; set; }
        public string ReplyText { get; set; }

    }
    #endregion
}
