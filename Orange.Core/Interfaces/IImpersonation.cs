namespace Orange.Core.Interfaces
{
    public interface IImpersonation
    {
        int CallingUserId { get; set; }
        int UserId { get; set; }
    }
}
