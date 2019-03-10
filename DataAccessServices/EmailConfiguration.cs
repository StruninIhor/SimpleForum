using DataContract;

namespace DataAccessServices
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public EmailConfiguration(string emailAddress, string password, string smtpServer, int port = 587, string displayName = "MVC Test Forum")
        {
            UserName = emailAddress;
            Password = password;
            SmtpServer = smtpServer;
            Port = port;
            DisplayName = displayName;
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string DisplayName { get; private set; }

        public string SmtpServer { get; private set; }

        public int Port { get; private set; }
    }
}
