using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class UserResult : Result, IResult
    {
        public User Result { get; set; }

        public UserResult()
        {
            Result = new User();
        }
    }

    public class UserResultList : Result, IResult
    {
        public List<User> Results { get; set; }

        public UserResultList()
        {
            Results = new List<User>();
        }
    }
}
