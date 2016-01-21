using Orange.Core.Repositories;

namespace Orange.Core.Entities
{
    public class Entity
    {
        protected Repository _repo = null;
        internal string _errorMessage = string.Empty;

        protected bool RepositoryIsNotValid()
        {
            return (ReferenceEquals(_repo, null));
        }

        protected void SetRepo(Repository repository)
        {
            _repo = repository;
        }
    }
}
