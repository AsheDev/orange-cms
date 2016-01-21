using System;
using Orange.Core.Enums;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class Comment
    {
        public int Id { get; private set; }
        public int PostId { get; private set; }
        public int UserId { get; private set; } // this can be 0 if it's an anonymous comment
        public string ProvidedName { get; private set; } // if not a user, this is what the commenter provided, or it's the user's name
        public string Body { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime ApprovalDate { get; private set; } // this is the time the comment was approved/denied
        public Approval Approval { get; private set; }
        public string EditKey { get; private set; }
        public bool TopLevel { get; private set; }
        public int ReplyCount { get; private set; }
        public bool IsActive { get; private set; }

        private Comment() { }
    }
}
