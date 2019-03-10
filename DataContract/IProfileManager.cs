using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public interface IProfileManager : IDisposable
    {
        void Create(UserProfile item);
    }
}
