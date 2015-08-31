using Connections;
using System.Data;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using System.Data.SqlClient;
using Orange.Core.Interfaces;

namespace Orange.Business
{
    public class CommentOps : Operations
    {
        public CommentOps() { }

        public CommentOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        // not sure how useful this one will be just yet
        public CommentResult Get(int commentId)
        {
            CommentResult result = new CommentResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.CommentGet", IdParameter(commentId));

            result = (CommentResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public CommentResultList GetAll(int postId)
        {
            CommentResultList results = new CommentResultList();
            IsDataSourceNull(results);
            if (results.Severity != Severity.Success) return results;

            DataTable returnedTable = DataSource.Crud("o.CommentGetAll", GetAllParameters(postId));

            results = (CommentResultList)Result.PostDatabaseCallErrorChecking(returnedTable, results);
            if (results.Severity != Core.Enums.Severity.Success) return results;

            Result.PopulateResult(results, returnedTable);
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns></returns>
        public CommentResult Add(IComment newComment)
        {
            CommentResult result = new CommentResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)newComment, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            CommentAdd comment = (CommentAdd)newComment;
            IsUserAnonymous(ref comment);

            DataTable returnedTable = DataSource.Crud("o.CommentAdd", AddParameters(comment));

            result = (CommentResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);

            // TODO: on success this needs to email the admin to let them know a new comment was posted and needs approval
            AdminApproval(result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateComment"></param>
        /// <returns></returns>
        public CommentResult Update(IComment updateComment)
        {
            CommentResult result = new CommentResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)updateComment, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            CommentUpdate errorCheck = (CommentUpdate)updateComment;
            if (IsEditkeyValid(errorCheck.Id, errorCheck.EditKey))
            {
                result = (CommentResult)Result.SetResultAsWarning(result, Comments.InvalidEditKey.GetDescription());
            }
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.CommentUpdate", UpdateParameters((CommentUpdate)updateComment));

            result = (CommentResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public BoolResult Remove(CommentRemove remove)
        {
            BoolResult result = new BoolResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)remove, result);
            if (result.Severity != Severity.Success) return result;

            if (IsEditkeyValid(remove.Id, remove.EditKey))
            {
                result = (BoolResult)Result.SetResultAsWarning(result, Comments.InvalidEditKey.GetDescription());
            }
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.CommentRemove", RemoveParameters(remove));

            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="approval"></param>
        /// <returns></returns>
        public BoolResult CommentApproval(CommentApproval approval)
        {
            BoolResult result = new BoolResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)approval, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.CommentApproval", ApprovalParameters(approval));

            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            // is this returning successfully?
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        private SqlParameter[] IdParameter(int commentId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@CommentId", commentId);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        private SqlParameter[] GetAllParameters(int postId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@PostId", postId);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        private SqlParameter[] AddParameters(CommentAdd add)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserId", add.UserId);
            parameters[1] = new SqlParameter("@CallingUserId", add.CallingUserId);
            parameters[2] = new SqlParameter("@PostId", add.PostId);
            parameters[3] = new SqlParameter("@ProvidedName", add.ProvidedName);
            parameters[4] = new SqlParameter("@Body", add.Body);
            parameters[5] = new SqlParameter("@EditKey", new PasswordOps(DataSource).GetUniqueWebSafeString(8));
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        private SqlParameter[] UpdateParameters(CommentUpdate update)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@UserId", update.UserId);
            parameters[1] = new SqlParameter("@CallingUserId", update.CallingUserId);
            parameters[2] = new SqlParameter("@CommentId", update.Id);
            parameters[3] = new SqlParameter("@ProvidedName", update.ProvidedName);
            parameters[4] = new SqlParameter("@Body", update.Body);
            //parameters[6] = new SqlParameter("@Approval", comment.Approval);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        private SqlParameter[] RemoveParameters(CommentRemove remove)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserId", remove.UserId);
            parameters[1] = new SqlParameter("@CallingUserId", remove.CallingUserId);
            parameters[2] = new SqlParameter("@CommentId", remove.Id);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="approval"></param>
        /// <returns></returns>
        private SqlParameter[] ApprovalParameters(CommentApproval approval)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@UserId", approval.UserId);
            parameters[1] = new SqlParameter("@CallingUserId", approval.CallingUserId);
            parameters[2] = new SqlParameter("@CommentId", approval.Id);
            parameters[3] = new SqlParameter("@Approval", approval.Approval);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="editKey"></param>
        /// <returns></returns>
        private bool IsEditkeyValid(int commentId, string editKey)
        {
            CommentResult commentInfo = Get(commentId);
            return (commentInfo.Result.EditKey == editKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        private void IsUserAnonymous(ref CommentAdd comment)
        {
            if (!string.IsNullOrWhiteSpace(comment.ProvidedName)) return;
            UserResult userInfo = new UserOps(DataSource).Get(comment.UserId);
            comment.ProvidedName = userInfo.Result.Name;
        }

        /// <summary>
        /// When an admin comments immediately approve the comment
        /// </summary>
        /// <param name="comment">The comment after having hit the database</param>
        private void AdminApproval(CommentResult comment)
        {
            UserResult userInfo = new UserOps(DataSource).Get(comment.Result.UserId);
            if(userInfo.Result.PermissionId <= 2) // TODO
            {
                CommentApproval approval = new CommentApproval
                {
                    UserId = userInfo.Result.Id,
                    Id = comment.Result.Id,
                    Approval = Approval.Approved
                };
                CommentApproval(approval);
            }
        }
    }
}
