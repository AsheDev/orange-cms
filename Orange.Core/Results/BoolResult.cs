using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Core.Results
{
    public class BoolResult : Result, IResult
    {
        public bool Result { get; set; }

        public BoolResult()
        {
            Result = false;
        }
    }
}
