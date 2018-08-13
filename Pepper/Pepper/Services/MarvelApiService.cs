using System;
using System.Collections.Generic;
using System.Text;
using Xam.Marvelous;
using Xam.Marvelous.Model.Base;
using Xam.Marvelous.Model.Filters;

namespace Pepper
{
    /// <summary>
    /// Class for manage marvel API service
    /// Xam.Marvelous based on https://github.com/TylerBrinks/Marvelous
    /// </summary>
    public class MarvelApiService
    {
        private string publicKey = Pepper.Resx.AppResources.ApiKey_Marvel_Public;
        private string privateKey = Pepper.Resx.AppResources.ApiKey_Marvel_Private;

        private static MarvelApiService _Instance;

        public static MarvelApiService Instance
        {
            get {
                if (_Instance == null)                
                    _Instance = new MarvelApiService();
                return _Instance;
            }
            set => _Instance = value;
        }

        public List<Character> FindCharByNameStart(String value= "Spi")
        {
            // Client definition
            var client = new MarvelRestClient(publicKey, privateKey);

            // Filter definition
            var filter = new CharacterRequestFilter();
            filter.NameStartsWith = value;
            filter.OrderBy(OrderResult.NameAscending);
            filter.ModifiedSince = new DateTime(1975, 12, 12);
            // Perform Search
            var response = client.GetCharacters(filter);

            if (response.Data.Results.Count > 0)
                return response.Data.Results;
            else
                return new List<Character>();
        }

        public List<Character> FindCharByName(String value = "Spiderman")
        {
            // Client definition
            var client = new MarvelRestClient(publicKey, privateKey);

            // Filter definition
            var filter = new CharacterRequestFilter();
            filter.Name = value;
            filter.OrderBy(OrderResult.NameAscending);
            filter.ModifiedSince = new DateTime(1975, 12, 12);
            // Perform Search
            var response = client.GetCharacters(filter);

            if (response.Data.Results.Count > 0)
                return response.Data.Results;
            else
                return new List<Character>();
        }

        public List<Story> FindStoriesByCharacterId(int value)
        {            
            // Client definition
            var client = new MarvelRestClient(publicKey, privateKey);
            Story s = new Story();
            
            // Filter definition
            var filter = new StoryRequestFilter();
            filter.AddCharacter(value);                        
            filter.OrderBy(OrderResult.NameAscending);
            filter.ModifiedSince = new DateTime(1975, 12, 12);
            // Perform Search
            var response = client.GetStories(filter);

            if (response.Data.Results.Count > 0)
                return response.Data.Results;
            else
                return new List<Story>();
        }

        public List<Comic> FindComicByCharacterId(String characterId)
        {
            // Client definition
            var client = new MarvelRestClient(publicKey, privateKey);            
            // Filter definition
            var filter = new ComicRequestFilter();            
            filter.OrderBy(OrderResult.NameAscending);
            filter.ModifiedSince = new DateTime(1975, 12, 12);
            // Perform Search
            var response = client.GetCharacterComics(characterId, filter);

            if (response.Data.Results.Count > 0)
                return response.Data.Results;
            else
                return new List<Comic>();
        }

    }
}
