using DataContract.Interfaces;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Identity
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
