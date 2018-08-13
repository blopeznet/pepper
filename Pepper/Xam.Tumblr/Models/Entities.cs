using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Xam.Tumblr.Models
{

        public class Meta
        {
            public int status { get; set; }
            public string msg { get; set; }
        }

        public class Blog
        {

            
            public string type { get; set; }
            
            public string urlavatar { get; set; }
            public string title { get; set; }
            public string name { get; set; }
            public int posts { get; set; }
            public string url { get; set; }
            public int updated { get; set; }
            public string description { get; set; }
            public bool ask { get; set; }
            public string ask_page_title { get; set; }
            public bool ask_anon { get; set; }
            public bool is_nsfw { get; set; }
            public bool share_likes { get; set; }
            public int likes { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class AltSize
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class OriginalSize
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class Photo
        {
            [JsonIgnore]
            public Guid id { get; set; }
            public string blogref { get; set; }
            public string blogtype { get; set; }
            public string title { get; set; }
            public string caption { get; set; }
            public string description { get; set; }
            public string mention { get; set; }
            public string url { get; set; }
            public List<AltSize> alt_sizes { get; set; }

            [JsonIgnore]
            public AltSize best_size { get; set; }

            public OriginalSize original_size { get; set; }

            public override string ToString()
            {
                return this.original_size.url;
            }
        }

        public class Post
        {
            public string blog_name { get; set; }
            public object id { get; set; }
            public string post_url { get; set; }
            public string slug { get; set; }
            public string type { get; set; }
            public string date { get; set; }
            public int timestamp { get; set; }
            public string state { get; set; }
            public string format { get; set; }
            public string reblog_key { get; set; }
            public List<string> tags { get; set; }
            public string short_url { get; set; }
            public List<object> highlighted { get; set; }
            public int note_count { get; set; }
            public string caption { get; set; }
            public string image_permalink { get; set; }
            public List<Photo> photos { get; set; }
            public string photoset_layout { get; set; }
        }

        public class Response
        {
            public Blog blog { get; set; }
            public List<Post> posts { get; set; }
            public int total_posts { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
            public Response response { get; set; }
        }
     
}
