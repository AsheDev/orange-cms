using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class DatabaseError : IError
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int LineNumber { get; set; }
        public string Source { get; set; }
        public string Procedure { get; set; }
        public string DataSource { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
