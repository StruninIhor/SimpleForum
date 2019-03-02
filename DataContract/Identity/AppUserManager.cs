using DataContract.Identity.Models;
using Microsoft.AspNet.Identity;

namespace DataContract.Identity
{
    public class AppUserManager : UserManager<AppUser, int>
    {
        public AppUserManager(IUserStore<AppUser, int> store)
            :base (store) { }
    }
}
