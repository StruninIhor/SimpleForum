using DataAccessServices.Infrastructure;
using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using DataContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    public class ForumService : IForumService
    {
        IUnitOfWork Database;

        public ForumService(IUnitOfWork unit)
        {
            Database = unit;
        }

        public async Task<OperationDetails> Create(string name, string authorEmail)
        {
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);
            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            var forum = Database.Forums.Where(f => f.Name == name).FirstOrDefault();

            if (forum != null)
            {
                return new OperationDetails(false, "Forum with the same name exists", "");
            }

            Database.Forums.Create(new DataContract.Models.Forum { Name = name, Author = user, CreatedDate = DateTime.Now });
            await Database.SaveAsync();
            return new OperationDetails(true, "Forum was successfully created", "");
        }

        public OperationDetails Delete(int id)
        {
            if (Database.Forums.GetById(id) == null)
            {
                return new OperationDetails(false, "Forum was not found", "");
            }
            Database.Forums.Delete(id);
            Database.Save();
            return new OperationDetails(true, "Forum was deleted", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public ForumModel GetForum(int id)
        {
            var result = Mapper.Map(Database.Forums.GetById(id));
            return result;
        }

        public ICollection<ForumModel> GetForums()
        {
            ICollection<ForumModel> result = new List<ForumModel>();

            foreach (var forum in Database.Forums.GetAll())
            {
                result.Add(Mapper.Map(forum));
            }
            return result;
        }

        public OperationDetails Update(ForumModel item)
        {
            var forum = Database.Forums.GetById(item.Id);

            if (forum == null)
            {
                return new OperationDetails(false, "Forum to update was not found", "");
            }

            if (forum.AuthorId != item.AuthorId)
            {
                return new OperationDetails(false, "Wrong author", "");
            }
            forum.Name = item.Name;
            Database.Forums.Update(forum);
            Database.Save();
            return new OperationDetails(true, "Forum was updated successfully", "");
        }
    }
}
