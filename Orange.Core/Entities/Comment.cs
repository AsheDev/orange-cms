using System;
using Orange.Core.Enums;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class Comment : IComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; } // this can be 0 if it's an anonymous comment
        public string ProvidedName { get; set; } // if not a user, this is what the commenter provided, or it's the user's name
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime ApprovalDate { get; set; } // this is the time the comment was approved/denied
        public Approval Approval { get; set; }
        public string EditKey { get; set; }
        public bool TopLevel { get; set; }
        public int ReplyCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CommentAdd : IComment, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; } // this can be 0 if it's an anonymous comment
        public int PostId { get; set; }
        public string ProvidedName { get; set; } // if not a user, this is what the commenter provided, or it's the user's name
        public string Body { get; set; }
        public bool TopLevel { get; set; }
    }

    public class CommentUpdate : IComment, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; } // this can be 0 if it's an anonymous comment
        public int Id { get; set; }
        public string ProvidedName { get; set; } // if not a user, this is what the commenter provided, or it's the user's name
        public string Body { get; set; }
        public string EditKey { get; set; }
    }

    public class CommentRemove : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public string EditKey { get; set; }
    }

    public class CommentApproval : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public Approval Approval { get; set; } // "ApprovalDate" field should be set by the stored procedure, not here
    }
}
