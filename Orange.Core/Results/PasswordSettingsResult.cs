using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Core.Results
{
    public class PasswordSettingsResult : Result, IResult
    {
        public PasswordSettings Result { get; set; }

        public PasswordSettingsResult()
        {
            Result = new PasswordSettings();
        }
    }
}
