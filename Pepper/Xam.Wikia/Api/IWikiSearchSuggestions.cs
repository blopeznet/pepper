using System.Threading.Tasks;
using Xam.Wikia.Models.SearchSuggestions;

namespace Xam.Wikia.Api
{
    public interface IWikiSearchSuggestions
    {
        Task<SearchSuggestionsPhrases> SuggestedPhrases(string query);
    }
}