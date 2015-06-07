using System;

namespace Orange.Core.Interfaces
{
    public interface IPassword
    {
        int UserId { get; set; }
        //string Password { get; set; }
        bool Expires { get; set; }
        DateTime Expiration { get; set; }
    }
}
