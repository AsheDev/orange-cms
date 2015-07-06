using System;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Entities
{
    public class Post : IPost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int CommentCount { get; set; }
        public List<Tag> Tags { get; set; }
        public bool IsPubliclyVisible { get; set; }
        public bool IsActive { get; set; }
    }

    public class PostAdd : IPost, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsPubliclyVisible { get; set; }
    }

    public class PostUpdate : IPost, IImpersonation
    {
        public int Id { get; set; }
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsPubliclyVisible { get; set; }
    }

    public class PostRemove : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
    }

    public class PostAddTest : PostAdd, IPost, IImpersonation
    {
        public PostAddTest()
        {
            UserId = 2;
            Subject = "Testing the Add Post Functionality";
            Body = "This is the functionality for creating new posts!";
            EffectiveDate = DateTime.Now.AddDays(5);
            IsPubliclyVisible = true;
        }
    }

    public class PostUpdateTest : PostUpdate, IPost, IImpersonation
    {
        public PostUpdateTest()
        {
            Id = 4;
            UserId = 2;
            Subject = "Testing the Update Post Functionality";
            Body = "This is the functionality for updating posts!";
            EffectiveDate = DateTime.Now.AddDays(10);
            IsPubliclyVisible = true;
        }
    }

    public class PostHistory
    {
        public int Id { get; set; }
        public int EditTypeId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsPubliclyVisible { get; set; }
        public int CallerId { get; set; }
        public bool IsActive { get; set; }
    }
}
