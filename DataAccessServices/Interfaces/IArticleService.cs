using DataAccessServices.Infrastructure;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface IArticleService: IDisposable
    {
        Task<OperationDetails> Create(string name, string text, string authorEmail, bool force = false);
        OperationDetails Update(ArticleModel item);
        OperationDetails Delete(int id);

        ArticleModel GetArticle(int id);

        ICollection<ArticleModel> GetArticles();
    }
}
