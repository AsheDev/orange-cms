using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using OE = Orange.Core.Enums;

namespace Orange.Core.Results
{
    public class PasswordResult : Result, IResult
    {
        public Password Result { get; set; }

        public PasswordResult()
        {
            Result = new Password();
        }
    }
}
