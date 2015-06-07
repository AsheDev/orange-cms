using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Core.Results
{
    public class PasswordResetResult : Result, IResult
    {
        public PasswordReset Result { get; set; }

        public PasswordResetResult()
        {
            Result = new PasswordReset();
        }
    }
}
