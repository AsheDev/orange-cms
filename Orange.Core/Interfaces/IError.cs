using System;

namespace Orange.Core.Interfaces
{
    public interface IError
    {
        bool HasError { get; set; }
        string Message { get; set; }
        string StackTrace { get; set; }
        int LineNumber { get; set; }
        string Source { get; set; }
        string Procedure { get; set; }
        string DataSource { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
