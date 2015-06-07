using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class NavigationDetailsResult : Result, IResult
    {
        public NavigationDetails Result { get; set; }

        public NavigationDetailsResult()
        {
            Result = new NavigationDetails();
        }
    }

    public class NavigationDetailsResultList : Result, IResult
    {
        public List<NavigationDetails> Results { get; set; }

        public NavigationDetailsResultList()
        {
            Results = new List<NavigationDetails>();
        }
    }
}
