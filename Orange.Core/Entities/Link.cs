namespace Orange.Core.Entities
{
    // TODO: this may be an addon to the core
    public class Link
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string LinkText { get; private set; } // "Click HERE to watch"
        public string Url { get; private set; }

        private Link() { }
    }
}
