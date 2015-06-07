using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Core.Results
{
    public class AccessibilityResult : Result, IResult
    {
        public Accessibility Result { get; set; }

        public AccessibilityResult()
        {
            Result = new Accessibility();
        }
    }
}
