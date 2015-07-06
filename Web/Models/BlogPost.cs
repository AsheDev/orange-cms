using Orange.Core.Entities;
using System.Collections.Generic;

namespace Web.Models
{
    public class BlogPost
    {
        public Post Post { get; set; }
        public List<Comment> PendingComments { get; set; }
        public List<Comment> ApprovedComments { get; set; }
        public bool AllowComments { get; set; }
        public bool ViewPendingComments { get; set; }

        public BlogPost()
        {
            PendingComments = new List<Comment>();
            ApprovedComments = new List<Comment>();
            AllowComments = true; // TODO: this will be set by PostSettings from the database
        }
    }
}