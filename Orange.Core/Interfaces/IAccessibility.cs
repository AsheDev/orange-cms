namespace Orange.Core.Interfaces
{
    public interface IAccessibility
    {
        int PermissionId { get; set; }
        bool ManagePosts { get; set; }
        bool CreateNewUsers { get; set; }
        bool AccessSettings { get; set; }
        bool CanImpersonate { get; set; }
        bool ViewMetrics { get; set; }
        bool IsActive { get; set; }
    }
}
