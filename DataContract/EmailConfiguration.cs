using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public static class EmailConfiguration
    {
        public static string UserName { get => ConfigurationSettings.AppSettings["UserName"]; }

        public static string Password { get => ConfigurationSettings.AppSettings["Password"]; }

        public static string DisplayName { get => ConfigurationSettings.AppSettings["DisplayName"]; }

        public static string SmtpServer { get => ConfigurationSettings.AppSettings["SmtpServer"]; }

        public static int Port { get => int.Parse(ConfigurationSettings.AppSettings["Port"]); }
    }
}
