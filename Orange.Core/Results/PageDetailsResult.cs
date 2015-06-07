using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class PageDetailsResult : Result, IResult
    {
        public PageDetails Result { get; set; }

        public PageDetailsResult()
        {
            Result = new PageDetails();
        }
    }

    public class PageDetailsResultList : Result, IResult
    {
        public List<PageDetails> Results { get; set; }

        public PageDetailsResultList()
        {
            Results = new List<PageDetails>();
        }
    }
}
