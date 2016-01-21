using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class PostResult : Result, IResult
    {
        private Post _post;

        public Post Post
        {
            get { return _post; }
            set
            {
                _post = value;
                if (!string.IsNullOrWhiteSpace(value._errorMessage)) SetResultAsCritical(this, value._errorMessage);
            }
        }

        public PostResult()
        {
            Post = new Post(null);
        }
    }

    public class PostResultList : Result, IResult
    {
        public List<Post> Posts { get; set; }

        public PostResultList()
        {
            Posts = new List<Post>();
        }
    }
}
