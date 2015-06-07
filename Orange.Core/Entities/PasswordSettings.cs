using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class PasswordSettings
    {
        public int MaxPasswordAttempts { get; set; }
        public int ExpirationInDays { get; set; }
        public int ResetExpirationInMinutes { get; set; }
    }

    public class PasswordSettingsUpdate : PasswordSettings, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
    }
}
