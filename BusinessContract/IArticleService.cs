using BusinessContract.Infrastructure;
using BusinessContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract
{
    public interface IArticleService: IDisposable
    {
        Task<OperationDetails> Create(string name, string text, string authorEmail);
        OperationDetails Update(ArticleModel item);
        OperationDetails Delete(int id);

        ArticleModel GetArticle(int id);

        ICollection<ArticleModel> GetArticles();
    }
}
