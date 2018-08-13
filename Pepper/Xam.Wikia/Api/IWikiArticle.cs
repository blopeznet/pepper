using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xam.Wikia.Enums;
using Xam.Wikia.Models.Article;
using Xam.Wikia.Models.Article.AlphabeticalList;
using Xam.Wikia.Models.Article.Details;
using Xam.Wikia.Models.Article.NewArticles;
using Xam.Wikia.Models.Article.PageList;
using Xam.Wikia.Models.Article.Popular;
using Xam.Wikia.Models.Article.Simple;

namespace Xam.Wikia.Api
{
    public interface IWikiArticle
    {
        Task<ContentResult> Simple(int id);
        Task<ExpandedArticleResultSet> Details(ArticleDetailsRequestParameters requestParameters);
        Task<string> ArticleRequest(ArticleEndpoint endpoint, Func<IDictionary<string, string>> getParameters);
        Task<ExpandedArticleResultSet> Details(params int[] ids);
        Task<UnexpandedListArticleResultSet> AlphabeticalList(string category);
        Task<UnexpandedListArticleResultSet> AlphabeticalList(ArticleListRequestParameters requestParameters);
        Task<ExpandedListArticleResultSet> PageList(ArticleListRequestParameters requestParameters);
        Task<ExpandedListArticleResultSet> PageList(string category);
        Task<NewArticleResultSet> NewArticles();
        Task<NewArticleResultSet> NewArticles(NewArticleRequestParameters requestParameters);
        Task<PopularListArticleResultSet> PopularArticleSimple(PopularArticleRequestParameters requestParameters);
        Task<PopularExpandedArticleResultSet> PopularArticleDetail(PopularArticleRequestParameters requestParameters);
        Task<T> PopularArticle<T>(PopularArticleRequestParameters requestParameters, bool expand);
    }
}