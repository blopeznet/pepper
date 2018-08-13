using System.Threading.Tasks;
using Xam.Wikia.Models;

namespace Xam.Wikia.Api
{
    public interface IWikiNavigation
    {
        Task<NavigationResultSet> NavigationLinks();
    }
}