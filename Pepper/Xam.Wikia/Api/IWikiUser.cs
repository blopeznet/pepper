using System.Threading.Tasks;
using Xam.Wikia.Models.User;

namespace Xam.Wikia.Api
{
    public interface IWikiUser
    {
        Task<UserResultSet> Details(UserRequestParameters requestParameters);
    }
}