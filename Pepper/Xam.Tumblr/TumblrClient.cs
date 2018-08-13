using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xam.Tumblr.Api;
using Xam.Tumblr.Models;

namespace Xam.Tumblr
{
    public class TumblrClient
    {

        public TumblrClient(String apikey,string blogname)
        {
            _tumblrDA = new TumblrApiAccess(apikey, blogname);
        }

        TumblrApiAccess _tumblrDA;
        private TumblrApiAccess TumblrDA
        {
            get{ return _tumblrDA;}
            set { _tumblrDA = value; }
        }

        /// <summary>
        /// Get Photos from blog tumblr
        /// </summary>
        /// <param name="name">blog name tumblr</param>
        /// <returns>True or False</returns>
        public async Task<List<Photo>> LoadPhotosTumblr(String name)
        {
            List<Photo> Photos = await TumblrDA.GetPhotosTumblr(name);
            if (Photos != null && Photos.Count > 0)
            {
                return Photos;
            }
            else
                return new List<Photo>();            
        }
                      
    }
}
