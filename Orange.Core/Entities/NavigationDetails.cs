using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class NavigationDetails : PageDetails, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int NavigationId { get; set; } // unique Id from the database
        //public int PageId { get; set; } // this should be handled by PageDetails' Id property
        public DateTime TimeStamp { get; set; }
    }
}