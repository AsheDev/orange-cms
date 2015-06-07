using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class PasswordReset : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string AuthenticationURL { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
    }
}
