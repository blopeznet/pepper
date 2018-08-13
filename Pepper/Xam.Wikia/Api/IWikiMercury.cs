using System.Threading.Tasks;
using Xam.Wikia.Models.Mercury;

namespace Xam.Wikia.Api
{
    public interface IWikiMercury
    {
        Task<WikiDataContainer> WikiVariables();
    }
}