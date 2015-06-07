using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class AccessDetails : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public bool Action { get; set; } // was it a log-in or a log-out?
        public bool Success { get; set; } // was the login-in/out successful?
        public DateTime TimeStamp { get; set; }
        public string OperatingSystem { get; set; }
        public string IPAddress { get; set; }
    }

    public class AccessDetailsTest : AccessDetails, IImpersonation
    {
        public AccessDetailsTest()
        {
            Id = 0;
            UserId = 1;
            Action = true;
            Success = true;
            OperatingSystem = "WINDOWS!";
            IPAddress = "192.168.1.1";
        }
    }
}
