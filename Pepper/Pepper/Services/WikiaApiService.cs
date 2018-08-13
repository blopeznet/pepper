using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xam.Wikia.Api;
using Xam.Wikia.Models.Search;
using Xam.Wikia.Models.SearchSuggestions;
using Xam.Wikia.Models.PhpQuery;

namespace Pepper
{
    /// <summary>
    /// Class for manage wikia API service
    /// Based on https://github.com/fablecode/wikia
    /// </summary>
    public class WikiaApiService
    {
        string domainUrl = Pepper.Resx.AppResources.Url_MarvelWikia;

        private static WikiaApiService _Instance;

        public static WikiaApiService Instance {
            get {
                if (_Instance == null)
                    _Instance = new WikiaApiService();
                return _Instance;
            }
            set => _Instance = value;
        }

        public async Task<CharacterPhpInfo> GetArticle(String name)
        {                        
            SearchListRequestParameter par = new SearchListRequestParameter(name);
            par.MinArticleQuality = 99;
            par.Limit = 10;
            WikiSearch search = new WikiSearch(domainUrl);
            LocalWikiSearchResultSet result = await search.SearchList(par);
            WikiArticle article = new WikiArticle(domainUrl);
            var content = await article.Details(System.Convert.ToInt32(result.Items[0].Id));
            CharacterPhpInfo detail = await Xam.Wikia.Helper.WikiReadingHelper.LoadMarvelWikiaPhpInfoById(result.Items[0].Id);
            return detail;
        }

        public async Task<CharacterPhpInfo> GetArticleById(String Id)
        {                                    
            CharacterPhpInfo detail = await Xam.Wikia.Helper.WikiReadingHelper.LoadMarvelWikiaPhpInfoById(Id);
            return detail;
        }

        public async Task<CharacterPhpInfo> GetArticleByName(String Name)
        {
            CharacterPhpInfo detail = await Xam.Wikia.Helper.WikiReadingHelper.LoadMarvelWikiaPhpInfoByName(Name);
            return detail;
        }

        

        public async Task<IList<LocalWikiSearchResult>> GetNames(String name)
        {            
            SearchListRequestParameter par = new SearchListRequestParameter(name);
            par.MinArticleQuality = 99;
            par.Limit = 10;
            WikiSearch search = new WikiSearch(domainUrl);
            LocalWikiSearchResultSet result = await search.SearchList(par);            
            return result.Items;
        }

        public async Task<List<SearchSuggestionsItems>> GetSuggestions(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return new List<SearchSuggestionsItems>();
            }                        

            SearchListRequestParameter par = new SearchListRequestParameter(name);
            par.MinArticleQuality = 99;
            par.Limit = 10;
            WikiSearchSuggestions sug = new WikiSearchSuggestions(domainUrl);
            var result = await sug.SuggestedPhrases(name);
            WikiSearch search = new WikiSearch(domainUrl);
            return new List<SearchSuggestionsItems>(result.Items);
        }

    }
}
