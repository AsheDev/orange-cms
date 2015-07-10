using System;

namespace Orange.Core.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public Tag()
        {
            Id = 0;
            Name = String.Empty;
            IsActive = false;
        }
    }
}
