using DataAccessServices.Interfaces;
using DataContract;
using DataContract.Identity;
using DataContract.Interfaces;
using DataContract.Models;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Modules
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        string emailAddress;
        string password;
        string smtpServer;
        IEmailConfiguration emailConfiguration;
        public ServiceModule(string connection, string smtpServer, string emailAddress, string password)
        {
            connectionString = connection;
            this.emailAddress = emailAddress;
            this.password = password;
            this.smtpServer = smtpServer;
            this.emailConfiguration = new EmailConfiguration(emailAddress, password, smtpServer);
        }
        public override void Load()
        {
            //Identity
            Bind<IEmailConfiguration>().To<EmailConfiguration>().WithConstructorArgument(emailAddress)
                .WithConstructorArgument(password)
                .WithConstructorArgument(smtpServer);
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString)
                .WithConstructorArgument(emailConfiguration);
            Bind<IUserService>().To<UserService>();
            Bind<IProfileManager>().To<ProfileManager>();

            //Forum
            Bind<IForumService>().To<ForumService>();
            Bind<ITopicService>().To<TopicService>();
            Bind<IArticleService>().To<ArticleService>();
        }
    }
}
