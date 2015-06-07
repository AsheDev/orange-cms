using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Results
{
    public class PermissionResult : Result, IResult
    {
        public Permission Result { get; set; }

        public PermissionResult()
        {
            Result = new Permission();
        }
    }

    public class PermissionResultList : Result, IResult
    {
        public List<Permission> Results { get; set; }

        public PermissionResultList()
        {
            Results = new List<Permission>();
        }
    }

}
