using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Core.Results
{
    public class StringResult : Result, IResult
    {
        public string Results { get; set; }

        public StringResult()
        {
            Results = string.Empty;
        }
    }
}
