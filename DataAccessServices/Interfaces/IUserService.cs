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

        List<string> GetUsers();
    }
}
