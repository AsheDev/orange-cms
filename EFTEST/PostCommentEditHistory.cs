//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFTEST
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostCommentEditHistory
    {
        public Nullable<int> FK_CommentId { get; set; }
        public Nullable<int> FK_EditTypeId { get; set; }
        public Nullable<int> FK_UserId { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string ProvidedName { get; set; }
        public string Body { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime ApprovalDate { get; set; }
        public Nullable<byte> Approval { get; set; }
        public string EditKey { get; set; }
        public Nullable<bool> TopLevel { get; set; }
        public Nullable<int> FK_CallerId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual EditType EditType { get; set; }
        public virtual PostComment PostComment { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
