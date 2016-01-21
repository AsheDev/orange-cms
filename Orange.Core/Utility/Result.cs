using System;
using System.Data;
using System.Linq;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using Orange.Core.Interfaces;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace Orange.Core.Utility
{
    public class Result
    {
        public string Message { get; set; }
        public Severity Severity { get; set; }
        public string SeverityAlertColor { get; set; }
        public DatabaseError ErrorDetails { get; set; } // in process for replacement with interface
        //public IError ErrorTest { get; set; }

        private static Repository _repo = new Repository(); // TODO: HARD CODED

        public Result()
        {
            Message = General.Empty.GetDescription();
            Severity = Severity.Empty;
            SeverityAlertColor = Severity.Empty.GetDescription();
            ErrorDetails = new DatabaseError();
            //ErrorTest = new DefaultError(); // I'm testing this out
            //_repo = new Repository(); // TODO: HARD CODED
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
            // 9/3/2015: can no longer assume this is a database error
            //result.ErrorDetails = (DatabaseError)ConstructSingleObject(new DatabaseError(), errorDetails);
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

        /// <summary>
        /// This checks for a row count of 0 as well as identifying any exception generated from the database call.
        /// </summary>
        /// <param name="returnedTable"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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

        //private static object ConstructSingleObject<T>(T objectType, DataTable table)
        //{
        //    return ObjectBuilder.PopulateBasicObject(objectType, table);
        //}

        //private static List<object> ConstructMultipleObjects<T>(T objectType, DataTable table)
        //{
        //    return ObjectBuilder.PopulateMulitpleBasicObjects(objectType, table);
        //}

        /// <summary>
        /// Construct a single object that has a list of other objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        //private static object ConstructSingleComplexObject<T>(T objectType, List<DataTable> tables)
        //{
        //    return ObjectBuilder.PopulateObjectWithNestedObject(objectType, tables).First();
        //}

        /// <summary>
        /// Construct a list of objects that contain a list of other objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        //private static List<object> ConstructMultipleComplexObjects<T>(T objectType, List<DataTable> tables)
        //{
        //    return ObjectBuilder.PopulateObjectWithNestedObject(objectType, tables);
        //}

        // TODO: I think these PopulateResult overloads need to be somewhere else...

        //public static void PopulateResult(PermissionResult result, DataTable returnedTable)
        //{
        //    result.Result = (Permission)Result.ConstructSingleObject(new Permission(_repo), returnedTable);
        //    result = (PermissionResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(AccessDetailsResult result, DataTable returnedTable)
        //{
        //    result.Result = (AccessDetails)Result.ConstructSingleObject(new AccessDetails(), returnedTable);
        //    result = (AccessDetailsResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(NavigationDetailsResult result, DataTable returnedTable)
        //{
        //    result.Result = (NavigationDetails)Result.ConstructSingleObject(new NavigationDetails(), returnedTable);
        //    result = (NavigationDetailsResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(NavigationDetailsResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new NavigationDetails(), returnedTable);
        //    result.Results = new List<NavigationDetails>(objectList.Cast<NavigationDetails>());
        //    result = (NavigationDetailsResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(AccessDetailsResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new AccessDetails(), returnedTable);
        //    result.Results = new List<AccessDetails>(objectList.Cast<AccessDetails>());
        //    result = (AccessDetailsResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PasswordResult result, DataTable returnedTable)
        //{
        //    result.Result = (Entities.Password)Result.ConstructSingleObject(new Entities.Password(), returnedTable);
        //    result = (PasswordResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PasswordResetResult result, DataTable returnedTable)
        //{
        //    result.Result = (PasswordReset)Result.ConstructSingleObject(new PasswordReset(), returnedTable);
        //    result = (PasswordResetResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(UserResult result, DataTable returnedTable)
        //{
        //    result.Result = (User)Result.ConstructSingleObject(new User(_repo), returnedTable);
        //    result = (UserResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(UserResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new User(_repo), returnedTable);
        //    result.Results = new List<User>(objectList.Cast<User>());
        //    result = (UserResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PostResult result, List<DataTable> returnedTables)
        //{
        //    result.Result = (Post)ConstructSingleComplexObject(new Post(_repo), returnedTables);
        //    result = (PostResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PostResultList result, List<DataTable> returnedTables)
        //{
        //    List<object> objectList = ConstructMultipleComplexObjects(new Post(_repo), returnedTables);
        //    result.Results = new List<Post>(objectList.Cast<Post>());
        //    result = (PostResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PostHistoryResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new PostHistory(), returnedTable);
        //    result.Results = new List<PostHistory>(objectList.Cast<PostHistory>());
        //    result = (PostHistoryResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(RoleResult result, DataTable returnedTable)
        //{
        //    result.Result = (Role)Result.ConstructSingleObject(new Role(_repo), returnedTable);
        //    result = (RoleResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(RoleResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new Role(_repo), returnedTable);
        //    result.Results = new List<Role>(objectList.Cast<Role>());
        //    result = (RoleResultList)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(PasswordSettingsResult result, DataTable returnedTable)
        //{
        //    result.Result = (PasswordSettings)Result.ConstructSingleObject(new PasswordSettings(), returnedTable);
        //    result = (PasswordSettingsResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(CommentResult result, DataTable returnedTable)
        //{
        //    result.Result = (Comment)Result.ConstructSingleObject(new Comment(), returnedTable);
        //    result = (CommentResult)SetResultAsSuccess(result);
        //}

        //public static void PopulateResult(CommentResultList result, DataTable returnedTable)
        //{
        //    List<object> objectList = Result.ConstructMultipleObjects(new Comment(), returnedTable);
        //    result.Results = new List<Comment>(objectList.Cast<Comment>());
        //    result = (CommentResultList)SetResultAsSuccess(result);
        //}
    }

    //public abstract class TestResult
    //{
    //    public string Message { get; set; }
    //    public Severity Severity { get; set; }
    //    public string SeverityAlertColor { get; set; }
    //    public IError ErrorTest { get; set; }
    //}

    // rename to severity
    //public interface HolyCrapSeverity
    //{
    //    string Description { get; set; }
    //    string ColorCode { get; set; } // hex color
    //}


    //public interface HolyCrapError
    //{

    //}

    //public interface PopulateResults
    //{
    //    void PopulateSingleResult(IResult result, IResult returnedTables);
    //    void PopulateMultipleResults(IResult result, List<IResult> returnedTables);
    //}

    //public abstract class ResultTest
    //{
    //    public string Message { get; set; } // just sample filler


    //    public abstract PopulateSingleResult();
    //}

    //public interface ResultFactory
    //{
    //    public string CreateResult();
    //}

    //public 
}
