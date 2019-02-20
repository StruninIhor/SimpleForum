using DataAccessServices.Infrastructure;
using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using DataContract.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    public class UserService : IUserService
    {
        IUnitOfWork Database;

        public UserService(IUnitOfWork unit)
        { Database = unit; }

        public async Task<ClaimsIdentity> Authenticate(User userDto)
        {
            ClaimsIdentity claim = null;

            var user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            return claim;

        }

        public async Task<OperationDetails> Create(User userDto)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new DataContract.Identity.Models.AppUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                if (await Database.RoleManager.FindByNameAsync("user") == null)
                {
                    await Database.RoleManager.CreateAsync(new DataContract.Identity.Models.CustomRole("user"));
                    await Database.SaveAsync();
                }

                await Database.UserManager.AddToRoleAsync(user.Id, "user");

                //TODO Some actions with user profile (in future)
                await Database.SaveAsync();
                return new OperationDetails(true, "Sucessful registration", "");
            }
            else
            {
                return new OperationDetails(false, "User with the same email exists", "");
            }
        }

        public async Task SetInitialData(User adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new DataContract.Identity.Models.CustomRole(roleName);
                    await Database.RoleManager.CreateAsync(role);
                }
                await Create(adminDto);

                var admin = await Database.UserManager.FindByEmailAsync(adminDto.Email);
                if (admin != null)
                {
                    await Database.UserManager.AddToRoleAsync(admin.Id, "admin");
                }
                await Database.SaveAsync();
            }
        }

        public List<string> GetUsers()
        {
            var users = Database.UserManager.Users;

            List<string> result = new List<string>();
            foreach (var user in users)
            {
                result.Add(user.Email);
            }
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Database.Dispose();
                }



                disposedValue = true;
            }
        }



        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
