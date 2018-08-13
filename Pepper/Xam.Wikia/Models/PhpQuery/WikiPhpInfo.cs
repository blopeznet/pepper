using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xam.Wikia.Models.PhpQuery
{
    #region Marvel Wikia

    public class WikiResultCharacters
    {
        public string pageid { get; set; }
        public string name { get; set; }
    }
    public class Parse
    {
        public string title { get; set; }
        public int revid { get; set; }
        public Text text { get; set; }
        public List<Langlink> langlinks { get; set; }
        public List<Category> categories { get; set; }
        public List<Link> links { get; set; }
        public List<Template> templates { get; set; }
        public List<string> images { get; set; }
        public List<string> externallinks { get; set; }
        public List<Section> sections { get; set; }
        public string displaytitle { get; set; }
    }

    public class Template
    {
        public int ns { get; set; }
        [JsonProperty(PropertyName = "*")]
        public string name { get; set; }
        public string exists { get; set; }
    }

    public class Text
    {
        [JsonProperty(PropertyName = "*")]
        public string name { get; set; }
    }

    public class Langlink
    {
        public string lang { get; set; }
        public string url { get; set; }
        [JsonProperty(PropertyName = "*")]
        public string name { get; set; }
    }

    public class Link
    {
        public int ns { get; set; }
        [JsonProperty(PropertyName = "*")]
        public string name { get; set; }
        public string exists { get; set; }
    }

    public class Category
    {
        public string sortkey { get; set; }
    }

    public class Section
    {
        public int toclevel { get; set; }
        public string level { get; set; }
        public string line { get; set; }
        public string number { get; set; }
        public string index { get; set; }
        public object fromtitle { get; set; }
        public int? byteoffset { get; set; }
        public string anchor { get; set; }
    }

    public class RootWikiSearch
    {
        public Parse parse { get; set; }
    }

    #endregion

    public  class CharacterPhpInfo
    {
        public CharacterPhpInfo(Parse ps)
        {
            displaytitle = ps.displaytitle;
            images = ps.images;
            text = ps.text.name;
        }

        public string displaytitle { get; set; }
        public List<string> images { get; set; }
        public String text { get; set; }

    }

}
