using Orange.Core.Results;

namespace BootstrapWeb.Models
{
    public class BlogPost
    {
        public UserResult UserDetails { get; set; }
        public PostResult PostDetails { get; set; }
        public CommentResultList TopLevelComments { get; set; }

        public enum CommentColorCodes
        {
            one,
            two,
            three,
            four,
            five
        }
    }
}