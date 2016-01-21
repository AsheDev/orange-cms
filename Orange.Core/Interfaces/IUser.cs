namespace Orange.Core.Interfaces
{
    public interface IUser
    {
        string Name { get; set; }
        string Email { get; set; }
        //bool IsVisible { get; set; }
        int RoleId { get; set; }
        //bool IsActive { get; set; }
    }
}
