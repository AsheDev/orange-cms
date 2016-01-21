using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class AccessDetails //: IImpersonation
    {
        public int CallingUserId { get; private set; }
        public int UserId { get; private set; }
        public int Id { get; private set; }
        public bool Action { get; private set; } // was it a log-in or a log-out?
        public bool Success { get; private set; } // was the login-in/out successful?
        public DateTime TimeStamp { get; private set; }
        public string OperatingSystem { get; private set; }
        public string IPAddress { get; private set; }

        private AccessDetails() { }
    }
}
