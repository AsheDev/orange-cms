using System;
using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class DefaultError //: IError
    {
        public bool HasError { get; private set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public int LineNumber { get; private set; }
        public string Source { get; private set; }
        public string Procedure { get; private set; }
        public string DataSource { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public DefaultError() { }
    }
}
