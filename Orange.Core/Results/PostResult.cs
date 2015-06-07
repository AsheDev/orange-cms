using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class PostResult : Result, IResult
    {
        public Post Result { get; set; }

        public PostResult()
        {
            Result = new Post();
        }
    }

    public class PostResultList : Result, IResult
    {
        public List<Post> Results { get; set; }

        public PostResultList()
        {
            Results = new List<Post>();
        }
    }

    public class PostHistoryResultList : Result, IResult
    {
        public List<PostHistory> Results { get; set; }

        public PostHistoryResultList()
        {
            Results = new List<PostHistory>();
        }
    }
}
