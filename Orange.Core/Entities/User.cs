using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsVisible { get; set; }
        public int PermissionId { get; set; }
        public bool InSystem { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserAdd : IUser, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PermissionId { get; set; }
    }

    public class UserUpdate : IUser, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PermissionId { get; set; }
    }

    public class UserRemove : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
    }

    public class UserAddTest : UserAdd, IUser, IImpersonation
    {
        public UserAddTest()
        {
            UserId = 1;
            Name = "Tester McTesterson";
            Email = "Test@Test.com";
            PermissionId = 2; // Admin
        }
    }

    public class UserUpdateTest : UserUpdate, IUser, IImpersonation
    {
        public UserUpdateTest()
        {
            UserId = 1;
            Id = 4;
            Name = "Tester McTesterson";
            Email = "Test@Test.com";
            PermissionId = 3; // Basic
        }
    }
}
