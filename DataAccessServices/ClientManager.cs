using DataContract;
using DataContract.Models;

namespace DataAccessServices
{
    public class ClientManager : IProfileManager
    {
        public ApplicationContext Database;

        public ClientManager(ApplicationContext db)
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
