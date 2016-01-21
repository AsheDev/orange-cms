using Orange.Core.Enums;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using Orange.Core.Repositories;

namespace Orange.Core.Results
{
    public class PermissionResult : Result, IResult
    {
        public Permission Result { get; set; }

        public PermissionResult(Repository repo)
        {
            Result = new Permission(repo);
        }
    }
}
