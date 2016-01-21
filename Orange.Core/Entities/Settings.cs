using Orange.Core.Repositories;

namespace Orange.Core.Entities
{
    public class SiteSettings : Entity
    {
        public int Id { get; set; }
        public int InactivityTime { get; private set; }
        public bool UnderMaintenance { get; private set; }

        private SiteSettings() { }

        public SiteSettings(Repository repo) 
        {
            _repo = repo;
            InactivityTime = 3600; // one hour
            UnderMaintenance = false;
        }

        public bool Set(int inactivity, bool maintenance)
        {
            if(inactivity < 0) return false;

            InactivityTime = inactivity;
            UnderMaintenance = maintenance;

            _repo.SiteSettings.Add(this);
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }
    }
}
