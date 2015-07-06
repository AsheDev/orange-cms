using Orange.Core.Enums;
using Ripley.Connections;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;

namespace Orange.Business
{
    public abstract class Operations
    {
        protected IDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="result"></param>
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

        /// <summary>
        /// Determines whether or not the datasource has been populated and is reachable.
        /// </summary>
        /// <param name="result"></param>
        protected void IsDataSourceNull(IResult result)
        {
            if (DataSource == null | !DataSource.IsConnectionLive())
            {
                result = Result.SetResultAsCritical(result, General.DataSourceIsNull.GetDescription());
                return;
            }
            result = Result.SetResultAsSuccess(result);
        }
    }
}
