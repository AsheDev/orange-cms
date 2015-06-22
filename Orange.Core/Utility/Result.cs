using System;
using System.Data;
using System.Linq;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Core.Utility
{
    public class Result
    {
        public string Message { get; set; }
        public Severity Severity { get; set; }
        public string SeverityAlertColor { get; set; }
        public DatabaseError ErrorDetails { get; set; } // this may be a great candidate for dependency injection

        public Result()
        {
            Message = General.Error.GetDescription();
            Severity = Severity.Critical;
            SeverityAlertColor = Severity.Critical.GetDescription();
            ErrorDetails = new DatabaseError();
        }

        public static IResult SetResultAsCritical(IResult result, string description)
        {
            result.Message = description;
            result.Severity = Severity.Critical;
            result.SeverityAlertColor = Severity.Critical.GetDescription();
            return result;
        }

        public static IResult SetResultAsException(IResult result, DataTable errorDetails)
        {
            result.Message = Convert.ToString(errorDetails.AsEnumerable().First().ItemArray[0]);
            result.Severity = Severity.Warning;
            result.SeverityAlertColor = Severity.Warning.GetDescription();
            result.ErrorDetails = (DatabaseError)ConstructSingleObject(new DatabaseError(), errorDetails);
            return result;
        }

        public static IResult SetResultAsWarning(IResult result, string description)
        {
            result.Message = description;
            result.Severity = Severity.Warning;
            result.SeverityAlertColor = Severity.Warning.GetDescription();
            return result;
        }

        public static IResult SetResultAsSuccess(IResult result)
        {
            result.Message = General.Success.GetDescription();
            result.Severity = Severity.Success;
            result.SeverityAlertColor = Severity.Success.GetDescription();
            return result;
        }

        // WAT DO?
        //public static IResult SetResultAsNormal(IResult result)
        //{
        //    result.Message = description;
        //    result.Severity = Severity.Normal;
        //    result.SeverityAlertColor = Severity.Normal.GetDescription();
        //    return result;
        //}

        public static IResult SetResultAsEmpty(IResult result)
        {
            result.Message = Severity.Empty.GetDescription();
            result.Severity = Severity.Empty;
            result.SeverityAlertColor = Severity.Empty.GetDescription();
            return result;
        }

        public static IResult PostDatabaseCallErrorChecking(DataTable returnedTable, IResult result)
        {
            if(returnedTable.Rows.Count == 0)
            {
                result = SetResultAsEmpty(result);
                return result;
            }
            // if row[3] == ".Net SqlClient Data Provider" // a database error was encountered
            if(returnedTable.AsEnumerable().First().Table.Columns[0].ColumnName == "ExceptionMessage")
            {
                result = SetResultAsException(result, returnedTable);
                return result;
            }
            return SetResultAsSuccess(result);
        }

        public static IResult PostDatabaseCallErrorChecking(List<DataTable> returnedTables, IResult result)
        {
            if (!returnedTables.Any())
            {
                result = SetResultAsEmpty(result);
                return result;
            }

            foreach (DataTable table in returnedTables)
            {
                if (table.Rows.Count == 0)
                {
                    result = SetResultAsEmpty(result);
                    return result;
                }
                // if row[3] == ".Net SqlClient Data Provider" // a database error was encountered
                if (table.AsEnumerable().First().Table.Columns[0].ColumnName == "ExceptionMessage")
                {
                    result = SetResultAsException(result, table);
                    return result;
                }
            }            
            return SetResultAsSuccess(result);
        }

        private static object ConstructSingleObject<T>(T objectType, DataTable table)
        {
            return ObjectBuilder.PopulateBasicObject(objectType, table);
        }

        private static List<object> ConstructMultipleObjects<T>(T objectType, DataTable table)
        {
            return ObjectBuilder.PopulateMulitpleBasicObjects(objectType, table);
        }

        public static void PopulateSingleResult(AccessibilityResult result, DataTable returnedTable)
        {
            result.Result = (Accessibility)Result.ConstructSingleObject(new Accessibility(), returnedTable);
            result = (AccessibilityResult)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(AccessDetailsResult result, DataTable returnedTable)
        {
            result.Result = (AccessDetails)Result.ConstructSingleObject(new AccessDetails(), returnedTable);
            result = (AccessDetailsResult)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(NavigationDetailsResult result, DataTable returnedTable)
        {
            result.Result = (NavigationDetails)Result.ConstructSingleObject(new NavigationDetails(), returnedTable);
            result = (NavigationDetailsResult)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(NavigationDetailsResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new NavigationDetails(), returnedTable);
            result.Results = new List<NavigationDetails>(objectList.Cast<NavigationDetails>());
            result = (NavigationDetailsResultList)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(AccessDetailsResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new AccessDetails(), returnedTable);
            result.Results = new List<AccessDetails>(objectList.Cast<AccessDetails>());
            result = (AccessDetailsResultList)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(PasswordResult result, DataTable returnedTable)
        {
            result.Result = (Entities.Password)Result.ConstructSingleObject(new Entities.Password(), returnedTable);
            result = (PasswordResult)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(PasswordResetResult result, DataTable returnedTable)
        {
            result.Result = (PasswordReset)Result.ConstructSingleObject(new PasswordReset(), returnedTable);
            result = (PasswordResetResult)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(UserResult result, DataTable returnedTable)
        {
            result.Result = (Orange.Core.Entities.User)Result.ConstructSingleObject(new User(), returnedTable);
            result = (UserResult)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(UserResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new User(), returnedTable);
            result.Results = new List<User>(objectList.Cast<User>());
            result = (UserResultList)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(PostResult result, DataTable returnedTable)
        {
            result.Result = (Post)Result.ConstructSingleObject(new Post(), returnedTable);
            result = (PostResult)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(PostResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new Post(), returnedTable);
            result.Results = new List<Post>(objectList.Cast<Post>());
            result = (PostResultList)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(PostHistoryResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new PostHistory(), returnedTable);
            result.Results = new List<PostHistory>(objectList.Cast<PostHistory>());
            result = (PostHistoryResultList)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(PermissionResult result, DataTable returnedTable)
        {
            result.Result = (Permission)Result.ConstructSingleObject(new Permission(), returnedTable);
            result = (PermissionResult)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(PermissionResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new Permission(), returnedTable);
            result.Results = new List<Permission>(objectList.Cast<Permission>());
            result = (PermissionResultList)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(PasswordSettingsResult result, DataTable returnedTable)
        {
            result.Result = (PasswordSettings)Result.ConstructSingleObject(new PasswordSettings(), returnedTable);
            result = (PasswordSettingsResult)SetResultAsSuccess(result);
        }

        public static void PopulateSingleResult(CommentResult result, DataTable returnedTable)
        {
            result.Result = (Comment)Result.ConstructSingleObject(new Comment(), returnedTable);
            result = (CommentResult)SetResultAsSuccess(result);
        }

        public static void PopulateMultipleResults(CommentResultList result, DataTable returnedTable)
        {
            List<object> objectList = Result.ConstructMultipleObjects(new Comment(), returnedTable);
            result.Results = new List<Comment>(objectList.Cast<Comment>());
            result = (CommentResultList)SetResultAsSuccess(result);
        }
    }
}
