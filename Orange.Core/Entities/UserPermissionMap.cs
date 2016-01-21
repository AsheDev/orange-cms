using System.ComponentModel.DataAnnotations;

namespace Orange.Core.Entities
{
    internal class UserPermissionMap
    {
        [Key]
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
