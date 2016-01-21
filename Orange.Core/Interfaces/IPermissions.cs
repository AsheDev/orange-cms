namespace Orange.Core.Interfaces
{
    public interface IPermissions
    {
        int RoleId { get; set; }
        bool ManagePosts { get; set; }
        bool ManagePostComments { get; set; }
        bool CanComment { get; set; }
        bool ManageUsers { get; set; }
        bool AccessSettings { get; set; }
        bool CanImpersonate { get; set; }
        bool ViewMetrics { get; set; }
        bool IsActive { get; set; }
    }
}
