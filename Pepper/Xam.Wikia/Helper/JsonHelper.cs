using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xam.Wikia.Helper
{
    public class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}