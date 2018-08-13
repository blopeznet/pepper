using System.Threading.Tasks;
using Xam.Wikia.Models.Search;

namespace Xam.Wikia.Api
{
    public interface IWikiSearch
    {
        Task<LocalWikiSearchResultSet> SearchList(SearchListRequestParameter requestParameters);
    }
}