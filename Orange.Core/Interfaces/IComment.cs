using System;

namespace Orange.Core.Interfaces
{
    public interface IComment
    {
        string ProvidedName { get; set; } // if not a user, this is what the commenter provided, or it's the user's name
        string Body { get; set; }
        //DateTime Created { get; set; }
    }
}
