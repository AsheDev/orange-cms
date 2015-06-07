using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class IntResult : Result, IResult
    {
        public int Result { get; set; }

        public IntResult()
        {
            Result = -1;
        }
    }
}
