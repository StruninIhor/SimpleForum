using DataContract;
using DataContract.Identity;
using DataContract.Identity.Models;
using DataContract.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;

namespace DataAccessServices
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationContext Database { get; set; }

        AppUserManager userManager;
        AppRoleManager roleManager;
        IProfileManager clientManager;
        IRepository<Forum> forumRepository;
        IRepository<Article> articleRepository;
        IRepository<Topic> topicRepository;
        IRepository<Comment> commentRepository;

        public UnitOfWork(string connectionString, IEmailConfiguration emailConfiguration)
        {
            Database = new ApplicationContext(connectionString);
            roleManager = new AppRoleManager(new CustomRoleStore(Database));
            clientManager = new ClientManager(Database);

            userManager = new AppUserManager(new CustomUserStore(Database));

            userManager.UserValidator = new UserValidator<AppUser, int>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            userManager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<AppUser, int>
                {
                    MessageFormat = "MVC forum security code is: {0}"
                });
            userManager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<AppUser, int>
                {
                    Subject = "MVC Forum security code",
                    BodyFormat = "MVC forum security code is: {0}"
                });
            userManager.EmailService = new IdentityEmailService(emailConfiguration);
            Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider dataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("MVC Forum");

            userManager.UserTokenProvider = new DataProtectorTokenProvider<AppUser, int>(
                dataProtectionProvider.Create("ASP.NET Identity"));
        }

        public AppUserManager UserManager => userManager;

        public AppRoleManager RoleManager => roleManager;

        public IProfileManager ProfileManager => clientManager;

        public IRepository<Forum> Forums
        {
            get
            {
                if (forumRepository == null)
                {
                    forumRepository = new ForumRepository(Database);
                }
                return forumRepository;
            }
        }

        public IRepository<Article> Articles
        {
            get
            {
                if (articleRepository == null)
                {
                    articleRepository = new ArticleRepository(Database);
                }
                return articleRepository;
            }
        }

        public IRepository<Topic> Topics
        {
            get
            {
                if (topicRepository == null)
                {
                    topicRepository = new TopicRepository(Database);
                }
                return topicRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (commentRepository == null)
                {
                    commentRepository = new CommentRepository(Database);
                }
                return commentRepository;
            }
        }

        public void Save()
        {
            Database.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Database.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    //clientManager
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
