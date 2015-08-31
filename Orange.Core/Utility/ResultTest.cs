using Orange.Core.Entities;
using Orange.Core.Enums;
using Orange.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.Core.Utility
{
    public abstract class ResultTest
    {
        public string Message { get; set; }
        public Severity Severity { get; set; }
        public string SeverityAlertColor { get; set; }
        public DatabaseError ErrorDetails { get; set; } // this may be a great candidate for dependency injection

        public ResultTest()
        {
            Message = General.Empty.GetDescription();
            Severity = Severity.Empty;
            SeverityAlertColor = Severity.Empty.GetDescription();
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
            if (returnedTable.Rows.Count == 0)
            {
                result = SetResultAsEmpty(result);
                return result;
            }
            // if row[3] == ".Net SqlClient Data Provider" // a database error was encountered
            if (returnedTable.AsEnumerable().First().Table.Columns[0].ColumnName == "ExceptionMessage")
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

        //public static abstract void PopulateResult(IResult result, DataTable returnedTable) { }

        //public static abstract void PopulateResult(IResult result, List<DataTable> returnedTables) { }
    }
}
