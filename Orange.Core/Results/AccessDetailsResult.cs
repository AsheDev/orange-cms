using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class AccessDetailsResult : Result, IResult
    {
        public AccessDetails Result { get; set; }

        public AccessDetailsResult()
        {
            Result = new AccessDetails();
        }
    }

    public class AccessDetailsResultList : Result, IResult
    {
        public List<AccessDetails> Results { get; set; }

        public AccessDetailsResultList()
        {
            Results = new List<AccessDetails>();
        }
    }
}
