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
    
    public partial class CommentApproval_Result
    {
        public int Id { get; set; }
        public Nullable<int> FK_PostId { get; set; }
        public Nullable<int> FK_UserId { get; set; }
        public string ProvidedName { get; set; }
        public string Body { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime ApprovalDate { get; set; }
        public Nullable<byte> Approval { get; set; }
        public string EditKey { get; set; }
        public Nullable<bool> TopLevel { get; set; }
        public int ReplyCount { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}