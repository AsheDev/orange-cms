using Orange.Core.Interfaces;

namespace Orange.Core.Entities
{
    public class Accessibility : IAccessibility, IImpersonation
    {
        public int CallingUserId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool ManagePosts { get; set; }
        public bool CreateNewUsers { get; set; }
        public bool AccessSettings { get; set; }
        public bool CanImpersonate { get; set; }
        public bool ViewMetrics { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccessibilityUpdate : Accessibility, IAccessibility, IImpersonation
    {
        // a bit empty, but I want to specify that all the fields inherited need to be used for updating it
    }

    public class AccessibilityUpdateTest : Accessibility, IAccessibility, IImpersonation
    {
        public AccessibilityUpdateTest()
        {
            UserId = 1;
            PermissionId = 3;
            ManagePosts = true;
            CreateNewUsers = false;
            AccessSettings = false;
            CanImpersonate = false;
            ViewMetrics = true;
            IsActive = true;
        }
    }
}
