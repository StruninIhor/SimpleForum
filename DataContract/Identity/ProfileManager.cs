using DataContract;
using DataContract.Models;

namespace DataAccessServices.Identity
{
    public class ProfileManager : IProfileManager
    {
        public ApplicationContext Database { get; set; }

        public ProfileManager(ApplicationContext db)
        {
            Database = db;
        }


        public void Create(UserProfile item)
        {
            Database.UserProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
