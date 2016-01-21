using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class UserResponse : Result, IResult
    {
        private User _user;

        public User User
        { 
            get { return _user; }
            set 
            {
                _user = value;
                if (!string.IsNullOrWhiteSpace(value._errorMessage)) SetResultAsCritical(this, value._errorMessage);
            } 
        }

        public UserResponse()
        {
            User = new User(null);
        }
    }

    public class UserResponseList : Result, IResult
    {
        public List<User> Users { get; set; }

        public UserResponseList()
        {
            Users = new List<User>();
        }
    }
}
