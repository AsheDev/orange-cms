using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class Permission
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public bool Removable { get; set; }
        public bool IsHidden { get; set; }
        public bool IsActive { get; set; }
    }

    public class PermissionTest : Permission
    {
        public PermissionTest()
        {
            Id = 1;
            Name = "Orange";
            Removable = false;
            IsHidden = false;
            IsActive = true;
        }
    }

    public class PermissionAdd : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public PermissionAdd()
        {
            CallingUserId = 0;
        }
    }

    public class PermissionUpdate : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public PermissionUpdate()
        {
            CallingUserId = 0;
        }
    }

    public class PermissionRemove : IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; } // this is an int, right?

        public PermissionRemove()
        {
            CallingUserId = 0;
        }
    }
}
