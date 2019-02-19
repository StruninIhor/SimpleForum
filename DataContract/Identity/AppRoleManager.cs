using DataContract.Identity.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Identity
{
    public class AppRoleManager : RoleManager<CustomRole, int>
    {
        public AppRoleManager(IRoleStore<CustomRole, int> store) 
            :base(store) { }
    }
}
