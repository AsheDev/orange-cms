namespace Orange.Core.Entities
{
    public class PageDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public bool IsPublic { get; set; } // if you're not logged in you can't see this page
        public bool IsActive { get; set; }
    }
}
