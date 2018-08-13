using System.Threading.Tasks;
using Xam.Wikia.Models.RelatedPages;

namespace Xam.Wikia.Api
{
    public interface IWikiRelatedPages
    {
        Task<RelatedPages> Articles(params int[] ids);
        Task<RelatedPages> Articles(RelatedArticlesRequestParameters requestParameters);
    }
}