using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class Password : IPassword
    {
        public int UserId { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public int Attempts { get; set; }
        public bool Expires { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsLocked { get; set; }
    }

    public class PasswordAdd : IPassword, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public bool Expires { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class PasswordAddTest : PasswordAdd, IPassword, IImpersonation
    {
        public PasswordAddTest()
        {
            UserId = 1;
            Password = "!Orange_2015!";
            Expires = false;
            Expiration = DateTime.Now.AddYears(5);
        }
    }

    public class PasswordUpdateNonSensitive : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Attempts { get; set; }
        public bool Expires { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsLocked { get; set; }
    }

    public class PasswordUpdateNonSensitiveTest : PasswordUpdateNonSensitive, IPassword, IImpersonation
    {
        public PasswordUpdateNonSensitiveTest()
        {
            UserId = 1;
            Attempts = 2;
            Expires = false;
            Expiration = DateTime.Now.AddYears(5);
            IsLocked = false;
        }
    }

    public class PasswordUpdateSensitive : Password, IPassword, IImpersonation
    {
        public int CallingUserId { get; set; }
        // same shit, just a different name to help things along
    }
}
