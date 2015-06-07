using Orange.Core.Entities;

namespace Orange.Core.Interfaces
{
    public interface IResult
    {
        string Message { get; set; }
        Enums.Severity Severity { get; set; }
        string SeverityAlertColor { get; set; }
        DatabaseError ErrorDetails { get; set; }
    }
}
