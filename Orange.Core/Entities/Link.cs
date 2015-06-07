namespace Orange.Core.Entities
{
    // TODO: this may be an addon to the core
    public class Link
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string LinkText { get; set; } // "Click HERE to watch"
        public string Url { get; set; }
    }
}
