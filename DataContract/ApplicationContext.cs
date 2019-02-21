using DataContract.Identity.Models;
using DataContract.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{ 
    public class ApplicationContext : IdentityDbContext<AppUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationContext(string connectionString) : base(connectionString) { }
        public ApplicationContext() : this("DefaultConnection") { }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasRequired(u => u.Profile)
                .WithRequiredPrincipal(p => p.AppUser);

            modelBuilder.Entity<Comment>().HasOptional(c => c.ReplyTo)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ReplyToCommentId)
                .WillCascadeOnDelete(false);

            ConfigureAuthor(modelBuilder, u => u.Comments);
            ConfigureAuthor(modelBuilder, u => u.Topics);
            ConfigureAuthor(modelBuilder, u => u.Forums);
            ConfigureAuthor(modelBuilder, u => u.Articles);

            modelBuilder.Entity<Comment>().HasRequired(c => c.Topic)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TopicId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Topic>().HasRequired(t => t.Forum)
                .WithMany(f => f.Topics)
                .HasForeignKey(t => t.ForumId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Forum>().ToTable("Forums"); //Why EF is creating "Fora" instead of "Forums"?

            base.OnModelCreating(modelBuilder);
        }



        void ConfigureAuthor<T>(DbModelBuilder modelBuilder, Expression<Func<AppUser, ICollection<T>>> expression) where T:BaseEntity
        {
            modelBuilder.Entity<T>().HasRequired(c => c.Author)
                .WithMany(expression)
                .HasForeignKey(c => c.AuthorId)
                .WillCascadeOnDelete(false);
        }
    }
}
