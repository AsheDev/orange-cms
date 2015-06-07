using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Orange.Core.Enums
{
    public enum Severity
    {
        [Description("#319331")] // green
        Success = 1,
        [Description("#000000")] // black
        Normal = 0,
        [Description("#FF6200")] // orange
        Warning = -1,
        [Description("#CC0018")] // red
        Critical = -2,
        [Description("#000000")] // black
        Empty = -3
    }

    public enum General
    {
        [Description("Operation completed successfully.")]
        Success = 0,
        [Description("Operation encountered an error.")]
        Error = -1,
        [Description("Operation failed at the database level.")]
        SqlError = -2,
        [Description("No results were returned.")]
        Empty = -3,
        [Description("Operation cannot be completed while impersonating a user.")]
        IsImpersonating = -4,
        [Description("No data source can be found.")]
        DataSourceIsNull = -5
    }

    public enum Password
    {
        [Description("Password is too short.")]
        TooShort = 0,
        [Description("Password is too long.")]
        TooLong = 1,
        [Description("Password requires at least one upper case letter.")]
        NoUpper = 2,
        [Description("Password requires at least one lower case letter.")]
        NoLower = 3,
        [Description("Password requires at least one number.")]
        NoNumbers = 4,
        [Description("Password requires at least one alpha character.")]
        NoLetters = 5,
        [Description("Password requires at least one special character.")]
        NoSpecialCharacters = 6,
        [Description("Too many attempts have been made to login. You will receive an email with instructions on resetting your password.")]
        TooManyAttempts = 7,
        [Description("Your password has expired. To login you will need to reset your password.")]
        Expired = 8,
        [Description("Incorrect password.")]
        NoMatch = 9,
        [Description("The email provided already has an associated account.")]
        PasswordExists = 9
    }

    public enum Operations
    {
        Add = 0,
        Update = 1,
        Remove = 2,
        Get = 3, // do we care?
        GetAll = 4 // do we care?
    }
}
