using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xam.Tumblr;
using Xam.Tumblr.Models;
using System.Linq;

namespace Pepper
{
    /// <summary>
    /// Class for manage tumblr API service
    /// </summary>
    public class TumblrApiService
    {
        private string blogname = Pepper.Resx.AppResources.blogname_Tumblr;
        private string apiKey = Pepper.Resx.AppResources.ApiKey_Tumblr;

        private static TumblrApiService _Instance;
        public static TumblrApiService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TumblrApiService();
                return _Instance;
            }
            set => _Instance = value;
        }

        public async Task<List<Photo>> GetPhotos(String name)
        {
            TumblrClient client = new TumblrClient(apiKey,blogname);
            List<Photo> photos = await client.LoadPhotosTumblr(name);
            Double width = Xamarin.Essentials.DeviceDisplay.ScreenMetrics.Width;
            foreach (Photo p in photos)
            {
                p.id = Guid.NewGuid();
                p.best_size = p.alt_sizes.Where(s => s.width <= width).FirstOrDefault(); 
                
                p.title = ConvertUrlToText(p.url);
            }
            return photos;
        }
        private String ConvertUrlToText(String url)
        {
            try
            {
                url = url.Substring(url.LastIndexOf("/") + 1).Replace("-", " ");
                url = url.First().ToString().ToUpper() + url.Substring(1);
                return url;
            }catch { return url; }
        }
    }
}
