using Connections;
using System.Data;
using System.Linq;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using System.Data.SqlClient;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Business
{
    public class PostOps : Operations
    {
        // is it more efficient to set this, in the case of adding or updating, or to pass the class (interface)
        // around as necessary?
        private IPost _post;

        public PostOps() { }

        public PostOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public PostResult Get(int postId)
        {
            PostResult result = new PostResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostGet", IdParameter(postId));
            result = (PostResult)Result.PostDatabaseCallErrorChecking(returnedTables, result);
            if (result.Severity != Severity.Success) return result;

            Result.PopulateResult(result, returnedTables);
            return result;
        }

        public PostResult GetLatest()
        {
            PostResult result = new PostResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostGetLatest");
            result = (PostResult)Result.PostDatabaseCallErrorChecking(returnedTables, result);
            if (result.Severity != Severity.Success) return result;

            Result.PopulateResult(result, returnedTables);
            return result;
        }

        public PostResultList GetAll()
        {
            PostResultList results = new PostResultList();
            IsDataSourceNull(results);
            if (results.Severity != Severity.Success) return results;

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostGetAll");
            results = (PostResultList)Result.PostDatabaseCallErrorChecking(returnedTables, results);
            if (results.Severity != Core.Enums.Severity.Success) return results;
            
            Result.PopulateResult(results, returnedTables);
            return results;
        }

        // alternative, I could just do a link statement to grab what I'm looking for
        public PostResultList GetAllByUserId(int userId)
        {
            PostResultList results = new PostResultList();
            IsDataSourceNull(results);
            if (results.Severity != Severity.Success) return results;

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostGetAllByUserId");
            results = (PostResultList)Result.PostDatabaseCallErrorChecking(returnedTables, results);
            if (results.Severity != Core.Enums.Severity.Success) return results;

            Result.PopulateResult(results, returnedTables);
            return results;
        }

        public PostResult Add(IPost newPost)
        {
            PostResult result = new PostResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)newPost, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            _post = newPost;

            // TODO: error checking

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostAdd", AddParameters());
            result = (PostResult)Result.PostDatabaseCallErrorChecking(returnedTables, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTables);
            return result;
        }

        public PostResult Update(IPost updatePost)
        {
            PostResult result = new PostResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)updatePost, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            _post = updatePost;

            // TODO: error checking

            List<DataTable> returnedTables = DataSource.CrudMultipleResults("o.PostUpdate", UpdateParameters());
            result = (PostResult)Result.PostDatabaseCallErrorChecking(returnedTables, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            
            Result.PopulateResult(result, returnedTables);
            return result;
        }

        public BoolResult Remove(PostRemove remove)
        {
            BoolResult result = new BoolResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)remove, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            
            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.PostRemove", RemoveParameters(remove));
            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            // is this returning successfully?
            return result;
        }

        public PostHistoryResultList GetPostHistoy(int postId)
        {
            PostHistoryResultList results = new PostHistoryResultList();
            IsDataSourceNull(results);
            if (results.Severity != Severity.Success) return results;

            DataTable returnedTable = DataSource.Crud("o.PostHistoryGetAll", PostHistoryParameter(postId));
            results = (PostHistoryResultList)Result.PostDatabaseCallErrorChecking(returnedTable, results);
            if (results.Severity != Core.Enums.Severity.Success) return results;

            Result.PopulateResult(results, returnedTable);
            return results;
        }

        private SqlParameter[] IdParameter(int postId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@PostId", postId);
            return parameters;
        }

        private SqlParameter[] AddParameters()
        {
            PostAdd post = (PostAdd)_post;
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserId", post.UserId);
            parameters[1] = new SqlParameter("@Subject", post.Subject);
            parameters[2] = new SqlParameter("@Body", post.Body);
            parameters[3] = new SqlParameter("@EffectiveDate", post.EffectiveDate.ToUniversalTime());
            parameters[4] = new SqlParameter("@IsPubliclyVisible", post.IsPubliclyVisible);
            parameters[5] = new SqlParameter("@CallingUserId", post.CallingUserId);
            return parameters;
        }

        private SqlParameter[] UpdateParameters()
        {
            PostUpdate post = (PostUpdate)_post;
            
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@PostId", post.Id);
            parameters[1] = new SqlParameter("@UserId", post.UserId);
            parameters[2] = new SqlParameter("@Subject", post.Subject);
            parameters[3] = new SqlParameter("@Body", post.Body);
            parameters[4] = new SqlParameter("@EffectiveDate", post.EffectiveDate.ToUniversalTime());
            parameters[5] = new SqlParameter("@IsPubliclyVisible", post.IsPubliclyVisible);
            parameters[6] = new SqlParameter("@CallingUserId", post.CallingUserId);
            return parameters;
        }

        private SqlParameter[] PostHistoryParameter(int postId)
        {
            PostUpdate post = (PostUpdate)_post;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@PostId", postId);
            return parameters;
        }

        private SqlParameter[] RemoveParameters(PostRemove remove)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@PostId", remove.Id);
            parameters[1] = new SqlParameter("@UserId", remove.UserId);
            parameters[2] = new SqlParameter("@CallingUserId", remove.CallingUserId);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private DataTable ConstructTagsTable(List<string> tagNames)
        {
            DataTable dtTags = new DataTable();
            dtTags.Columns.Add("Name", typeof(string));

            foreach (string tagName in tagNames)
            {
                DataRow row = dtTags.NewRow();
                row["Name"] = tagName;
                dtTags.Rows.Add(row);
            }
            return dtTags;
        }
    }
}
