using Orange.Core.Enums;

namespace Orange.Core.Models
{
    public class Authentication
    {
        public AuthenticationStatus Status { get; set; }

        public Authentication()
        {
            Status = AuthenticationStatus.Default;
        }
    }
}
