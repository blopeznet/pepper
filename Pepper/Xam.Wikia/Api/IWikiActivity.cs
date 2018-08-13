using System.Threading.Tasks;
using Xam.Wikia.Enums;
using Xam.Wikia.Models.Activity;

namespace Xam.Wikia.Api
{
    public interface IWikiActivity
    {
        Task<ActivityResponseResult> Latest();
        Task<ActivityResponseResult> Latest(ActivityRequestParameters requestParameters);
        Task<ActivityResponseResult> RecentlyChangedArticles();
        Task<ActivityResponseResult> RecentlyChangedArticles(ActivityRequestParameters requestParameters);
        Task<ActivityResponseResult> Activity(ActivityRequestParameters requestParameters, ActivityEndpoint endpoint);
    }
}