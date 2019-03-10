using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public interface IEmailConfiguration
    {
        string UserName { get; }
        string Password { get; }
        string DisplayName { get; }
        string SmtpServer { get; }
        int Port { get; }
    }
}
