using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xam.Wikia
{
    public interface IWikiaHttpClient
    {
        Task<string> Get(string url);
        Task<string> Get(string url, IDictionary<string, string> parameters);
    }
}