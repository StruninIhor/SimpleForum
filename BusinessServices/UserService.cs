using BusinessContract;
using BusinessContract.Infrastructure;
using BusinessContract.Models;
using DataContract;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class UserService : IUserService
    {
        IUnitOfWork Database;

        public UserService(IUnitOfWork unit)
        {
            Database = unit;
        }

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

                await CreateRoleIfNotExists("user");

                await Database.UserManager.AddToRoleAsync(user.Id, "user");

                Database.ProfileManager.Create(new DataContract.Models.UserProfile
                {
                    AppUser = user,
                    RegistrationDate = DateTime.Now,
                    UserName = userDto.UserName,
                    Status = userDto.Status,
                    EmailNotificationsEnabled = userDto.EmailNotificationsEnabled,
                    ForumNotificationsEnabled = userDto.ForumNotificationsEnabled,
                    SubscriptionEnabled = userDto.SubscriptionEnabled,
                    Rating = 0
                });


                //TODO Some actions with user profile (in future)
                await Database.SaveAsync();
                return new OperationDetails(true, "Sucessful registration", "");
            }
            else
            {
                return new OperationDetails(false, "User with the same email exists", "Email");
            }
        }

        async Task CreateRoleIfNotExists(string roleName)
        {
            var role = await Database.RoleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new DataContract.Identity.Models.CustomRole(roleName);
                await Database.RoleManager.CreateAsync(role);
            }
        }

        public async Task SetInitialData(User adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                await CreateRoleIfNotExists(roleName);
            }
            await Create(adminDto);
            var admin = await Database.UserManager.FindByEmailAsync(adminDto.Email);
            if (admin != null)
            {
                string adminRoleName = "admin";
                string superRoleName = "superadmin";
                await CreateRoleIfNotExists(adminRoleName);
                await CreateRoleIfNotExists(superRoleName);
                await Database.UserManager.AddToRolesAsync(admin.Id, adminRoleName, superRoleName);
            }
        }

        public Task<string> GenerateConfirmationTokenAsync(int userId) => Database.UserManager.GenerateEmailConfirmationTokenAsync(userId);

        public string GenerateConfirmationToken(int userId)
        {
            return Database.UserManager.GenerateEmailConfirmationToken(userId);
        }

        public async Task<User> GetUser(int id)
        {
            return Mapper.Map(await Database.UserManager.FindByIdAsync(id));
        }

        public async Task<User> GetUser(string email)
        {
            var user = await Database.UserManager.FindByEmailAsync(email);
            return (user != null) ? Mapper.Map(user) : null;
        }

        public User GetUserById(int id)
        {
            var user = Database.UserManager.FindById(id);
            return (user != null) ? Mapper.Map(user) : null;
        }

        public User GetUserByEmail(string email)
        {
            var user = Database.UserManager.FindByEmail(email);
            return (user != null) ? Mapper.Map(user) : null;
        }

        public async Task SendConfirmationMessageAsync(int userId, string confirmationLink)
        {
            await Database.UserManager.SendEmailAsync(userId, "Email address Confirmation",
                "<p><b>MVC Test Forum Email confirmation</b></p>To complete the registration, click on the following link: <br />" +
                "<a href=\"" + confirmationLink + "\">" + confirmationLink + "</a><br />If you didn't request for email confirm, just ignore this message.");
        }

        public async Task<OperationDetails> Update(User model)
        {
            var user = await Database.UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                user.Profile.EmailNotificationsEnabled = model.EmailNotificationsEnabled;
                user.Profile.ForumNotificationsEnabled = model.ForumNotificationsEnabled;
                user.Profile.SubscriptionEnabled = model.SubscriptionEnabled;
                user.Profile.UserName = model.UserName;
                user.Profile.Status = model.Status;
                user.IsBlocked = model.IsBlocked;
                user.Profile.Rating = model.Rating;

                var result = await Database.UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new OperationDetails(true, "User updated sucessfully", "");
                }
                else
                {
                    return new OperationDetails(false, GetErrorMessage(result), "", result.Errors);
                }
            }
            else
            {
                return new OperationDetails(false, "User was not found", "");
            }
        }

        public async Task<OperationDetails> ConfirmEmailAsync(int userId, string code)
        {
            if (code == null)
            {
                return new OperationDetails(false, "Code is incorrect", "code");
            }
            else if (await Database.UserManager.FindByIdAsync(userId) == null)
            {
                return new OperationDetails(false, "User was not found", "userId");
            }
            var result = await Database.UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return new OperationDetails(true, "Email was confirmed sucessfully", "");
            }
            else
            {
                return new OperationDetails(false, GetErrorMessage(result), "", result.Errors);
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(int userId) => await Database.UserManager.GeneratePasswordResetTokenAsync(userId);

        public string GeneratePasswordResetToken(int userId) => Database.UserManager.GeneratePasswordResetToken(userId);

        public async Task SendResetPasswordMessageAsync(int userId, string confirmationLink)
        {
            await Database.UserManager.SendEmailAsync(userId, "Forum password reset",
               "<p><b>MVC Test Forum password reset</b></p>To complete the password reset, click on the following link: <br />" +
               "<a href=\"" + confirmationLink + "\">" + confirmationLink + "</a><br />If you didn't request for password reset, just ignore this message.");
        }

        public async Task<OperationDetails> ResetPasswordAsync(int id, string code, string newPassword)
        {
            var result = await Database.UserManager.ResetPasswordAsync(id, code, newPassword);

            if (result.Succeeded)
            {
                return new OperationDetails(true, "Password was sucessfully reset", "");
            }
            else
            {
                return new OperationDetails(false, GetErrorMessage(result), "", result.Errors);
            }
        }

        public async Task<OperationDetails> Delete(int userId)
        {
            var user = await Database.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationDetails(false, "User was not found", "");
            }
            else
            {
                var result = await Database.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new OperationDetails(true, "User was deleted sucessfully", "");
                }
                else
                {
                    return new OperationDetails(false, GetErrorMessage(result), "", result.Errors);
                }
            }
        }

        public async Task<OperationDetails> Delete(string Email)
        {
            var user = await Database.UserManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return new OperationDetails(false, "User was not found", "");
            }
            else return await Delete(user.Id);
        }
        public async Task<bool> UserExists(string Email) => await Database.UserManager.FindByEmailAsync(Email) != null;

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

        #region Helpers

        string GetErrorMessage(IdentityResult result) => (result.Errors.Count() > 1) ?
                        "Multiple errors were encountered while processing your request"
                        : "An error encountered while processing your request";

        #endregion

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

        public ICollection<string> UserRoles(int userId)
        {
            var user = Database.UserManager.FindById(userId);
            var result = new List<string>();
            if (user != null)
            {
                foreach (var role in user.Roles)
                {
                    result.Add(Database.RoleManager.FindById(role.RoleId).Name);
                }
            }
            return result;

        }

        public async Task<OperationDetails> AddToRole(int userId, string role)
        {
            var user = Database.UserManager.FindById(userId);

            if (user != null)
            {
                if (Database.RoleManager.FindByName(role) != null)
                {
                    var result = await Database.UserManager.AddToRoleAsync(userId, role);
                    if (!result.Succeeded)
                    {
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                    }
                    else return new OperationDetails(true, "User was added sucessfully", "");
                }
                else return new OperationDetails(false, $"Role with name {role} does not exists!", "");
            }
            else return new OperationDetails(false, "User was not found", "");
        }
        ICollection<User> Users { get {
                var users = Database.UserManager.Users.ToList();
                var result = new List<User>();
                foreach (var user in users)
                {
                    result.Add(Mapper.Map(user));
                }
                return result;
            } }

        #endregion

    }
}

