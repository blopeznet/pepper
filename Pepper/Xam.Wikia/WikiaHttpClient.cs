﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xam.Wikia.Helper;

namespace Xam.Wikia
{
    public class WikiaHttpClient : IWikiaHttpClient
    {
        public Task<string> Get(string url)
        {
            return Get(url, null);
        }

        public async Task<string> Get(string url, IDictionary<string, string> parameters)
        {
            url = UrlHelper.GenerateUrl(url, parameters);

            using (var client = new HttpClient())
                return await client.GetStringAsync(url);
        }
    }
}