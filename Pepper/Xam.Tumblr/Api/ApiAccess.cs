using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xam.Tumblr.Models;

namespace Xam.Tumblr.Api
{

    public class TumblrApiAccess
    {


        public TumblrApiAccess(string keyapi,string blogname)
        {
            apikey = keyapi;
            nombreblog = blogname;
        }

        private string apikey;
        private string nombreblog;

        public async Task<string[]> GetRssTumblr(
            string feedUrl = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/posts/photo?api_key={1}&base-hostname={0}")
        {
            try
            {

                feedUrl = String.Format(feedUrl, nombreblog, apikey);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(feedUrl);
                HttpResponseMessage response = await client.GetAsync(feedUrl);
                String str = await response.Content.ReadAsStringAsync();

                //Obtenemos los datos            
                RootObject o = JsonConvert.DeserializeObject<RootObject>(str);

                List<string> cadenas = new List<string>();

                for (int i = 0; i <= o.response.posts.Count - 1; i++)
                {
                    Post p = o.response.posts[i];
                    foreach (var pic in p.photos)
                    {
                        cadenas.Add(pic.original_size.url);
                    }
                    i++;
                }

                return cadenas.ToArray();

            }
            catch (Exception ex) { return null; }
        }


        public async Task<String> GetAvatarTumblr(
            string size = "48",
            string urlavatar = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/avatar/{1}")
        {

            try
            {
                urlavatar = String.Format(urlavatar, nombreblog, size);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlavatar);
                HttpResponseMessage response = await client.GetAsync(urlavatar);
                return response.RequestMessage.RequestUri.OriginalString;

            }
            catch { return null; }
        }

        public async Task<Blog> GetBlogInfoTumblr(
           string infoUrl = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/info?api_key={1}")
        {
            try
            {

                infoUrl = String.Format(infoUrl, nombreblog, apikey);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(infoUrl);
                HttpResponseMessage response = await client.GetAsync(infoUrl);
                String str = await response.Content.ReadAsStringAsync();

                //Obtenemos los datos            
                RootObject o = JsonConvert.DeserializeObject<RootObject>(str);

                if (o.response.blog.title == "Untitled")
                    o.response.blog.title = nombreblog;

                return o.response.blog;

            }
            catch (Exception ex) { return null; }
        }

        public async Task<List<Photo>> GetPhotosTumblr(
            string name,
            string feedUrl = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/posts/photo?api_key={1}&base-hostname={0}")
        {
            try
            {
                feedUrl = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/posts/photo?api_key={1}&base-hostname={0}&tag={2}";
                feedUrl = String.Format(feedUrl, nombreblog, apikey,name);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(feedUrl);
                HttpResponseMessage response = await client.GetAsync(feedUrl);
                String str = await response.Content.ReadAsStringAsync();

                //Obtenemos los datos            
                RootObject o = JsonConvert.DeserializeObject<RootObject>(str);
                List<Photo> cadenas = new List<Photo>();

                o.response.posts = o.response.posts.Where(p => p.type == "photo").ToList<Post>();



                for (int i = 0; i <= o.response.posts.Count - 1; i++)
                {
                    Post p = o.response.posts[i];                    

                    foreach (var pic in p.photos)
                    {
                        try
                        {
                            pic.blogref = nombreblog;
                            pic.blogtype = "tumblr";
                            pic.caption = p.date;

                            String desc = "Tags: ";
                            foreach (String s in p.tags)
                                desc += s;

                            pic.title = String.Empty;

                            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();                            
                            pic.description = StripHtml(p.caption);                           
                            pic.mention = nombreblog + ".tumblr.com";
                            pic.url = p.post_url;
                           
                        }
                        catch { }

                        cadenas.Add(pic);
                    }
                    i++;
                }

                return cadenas;

            }
            catch (Exception ex) { return null; }
        }

        private string StripHtml(string value)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(value);            
            if (htmlDoc == null)
                return value;

            return htmlDoc.DocumentNode.InnerText;
        }

        public async Task<Photo> GetRandomPhoto(
            string feedUrl = "http://api.tumblr.com/v2/blog/{0}.tumblr.com/posts/photo?api_key={1}&base-hostname={0}")
        {
            try
            {

                feedUrl = String.Format(feedUrl, nombreblog, apikey);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(feedUrl);
                HttpResponseMessage response = await client.GetAsync(feedUrl);
                String str = await response.Content.ReadAsStringAsync();

                //Obtenemos los datos            
                RootObject o = JsonConvert.DeserializeObject<RootObject>(str);
                ObservableCollection<Photo> cadenas = new ObservableCollection<Photo>();

                Random rand = new Random();
                int indice = rand.Next(0, o.response.posts.Count - 1);
                Post p = o.response.posts[indice];
                indice = rand.Next(0, p.photos.Count - 1);
                Photo url = p.photos[indice];
                url.blogref = nombreblog;
                url.blogtype = "tumblr";
                return url;

            }
            catch (Exception ex) { return null; }
        }
     
    }
}
