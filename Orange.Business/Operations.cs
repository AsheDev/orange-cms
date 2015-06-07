using Orange.Core.Enums;
using Orange.Connections;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Business
{
    public abstract class Operations
    {
        protected IDataSource DataSource { get; set; }

        protected void IsImpersonating(IImpersonation details, IResult result)
        {
            if (details.CallingUserId == 0)
            {
                details.CallingUserId = details.UserId; // this is for the benefit of the database
                result = Result.SetResultAsSuccess(result);
                return;
            }
            // TODO: this is temporary until we can log the activity
            if (details.UserId != details.CallingUserId)
            {
                result = Result.SetResultAsCritical(result, General.IsImpersonating.GetDescription());
                return;
            }
            result = Result.SetResultAsSuccess(result);
        }

        protected void IsDataSourceNull(IResult result)
        {
            // TODO: this needs to be an actual test to hit the database provided
            if (DataSource == null)
            {
                result = Result.SetResultAsCritical(result, General.DataSourceIsNull.GetDescription());
                return;
            }
            result = Result.SetResultAsSuccess(result);
        }
    }
}
