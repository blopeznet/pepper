using System.Threading.Tasks;
using Xam.Wikia.Configuration;
using Xam.Wikia.Helper;
using Xam.Wikia.Models.Mercury;

namespace Xam.Wikia.Api
{
    public class WikiMercury : IWikiMercury
    {
        private readonly IWikiaHttpClient _wikiaHttpClient;
        private readonly string _wikiApiUrl;
        private readonly string _wikiVariablesUrl = "Mercury/WikiVariables";

        public WikiMercury(string domainUrl)
            : this(domainUrl, WikiaSettings.RelativeApiUrl)
        {

        }

        public WikiMercury(string domainUrl, string relativeApiUrl)
            : this(domainUrl, relativeApiUrl, new WikiaHttpClient())
        {

        }

        public WikiMercury(string domainUrl, string relativeApiUrl, IWikiaHttpClient wikiaHttpClient)
        {
            _wikiApiUrl = UrlHelper.GenerateApiUrl(domainUrl, relativeApiUrl);
            _wikiaHttpClient = wikiaHttpClient;
        }

        public async Task<WikiDataContainer> WikiVariables()
        {
            var requestUrl = UrlHelper.GenerateApiUrl(_wikiApiUrl, _wikiVariablesUrl);
            var json = await _wikiaHttpClient.Get(requestUrl);

            return JsonHelper.Deserialize<WikiDataContainer>(json);
        }
    }
}