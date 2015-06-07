using System;

namespace Orange.Core.Interfaces
{
    public interface IPost
    {
        int UserId { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        DateTime EffectiveDate { get; set; }
        bool IsPubliclyVisible { get; set; }
    }
}
