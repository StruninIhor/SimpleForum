using DataAccessServices.Infrastructure;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(User userDto);
        Task<ClaimsIdentity> Authenticate(User userDto);
        Task SetInitialData(User adminDto, List<string> roles);
        Task<User> GetUser(int id);
        Task<User> GetUser(string Email);
        Task<string> GenerateConfirmationTokenAsync(int userId);
        string GenerateConfirmationToken(int userId);
        Task SendConfirmationMessageAsync(int userId, string confirmationLink);
        Task<OperationDetails> Update(User model);
        Task<OperationDetails> ConfirmEmailAsync(int userId, string code);

        Task<string> GeneratePasswordResetTokenAsync(int userId);
        string GeneratePasswordResetToken(int userId);
        Task SendResetPasswordMessageAsync(int userId, string confirmationLink);
        Task<OperationDetails> ResetPasswordAsync(int id, string code, string newPassword);

        Task<OperationDetails> Delete(int userId);
        Task<OperationDetails> Delete(string Email);

        Task<bool> UserExists(string Email);
        List<string> GetUsers();
    }
}
