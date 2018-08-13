using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xam.Wikia.Models.PhpQuery;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Xam.Wikia.Helper
{
    public static class WikiReadingHelper
    {
        public static async Task<CharacterPhpInfo> LoadMarvelWikiaPhpInfoById(          
          string pageid)
        {
            string feedUrl = "http://marvel.wikia.com/api.php?action=parse&pageid={0}&format=json";
            feedUrl = String.Format(feedUrl, pageid);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(feedUrl);
            HttpResponseMessage response = await client.GetAsync(feedUrl);

            String str = await response.Content.ReadAsStringAsync();
            
            Parse parse = JsonConvert.DeserializeObject<RootWikiSearch>(str).parse;
            

            CharacterPhpInfo info = new CharacterPhpInfo(parse);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(info.text);
            info.text = ParseHtmlCodeWithCss(FormattingMarvelWikia(doc));

            return info;
        }

        public static async Task<CharacterPhpInfo> LoadMarvelWikiaPhpInfoByName(
         string name)
        {
            string feedUrl = "http://marvel.wikia.com/api.php?action=parse&page={0}&format=json";
            feedUrl = String.Format(feedUrl, name);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(feedUrl);
            HttpResponseMessage response = await client.GetAsync(feedUrl);

            String str = await response.Content.ReadAsStringAsync();

            Parse parse = JsonConvert.DeserializeObject<RootWikiSearch>(str).parse;


            CharacterPhpInfo info = new CharacterPhpInfo(parse);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(info.text);
            info.text = ParseHtmlCodeWithCss(FormattingMarvelWikia(doc));

            return info;
        }

        private static String FormattingMarvelWikia(HtmlDocument doc)
        {
            
            List<HtmlNode> nodesToModify = doc.DocumentNode.Descendants().Where(n => n.Name == "img").ToList();
            foreach (HtmlNode node in nodesToModify)
            {
                try
                {
                    if (node.HasAttributes)
                    {
                        if (node.Attributes["src"].Value.Contains("data:image"))
                        {
                            node.Attributes["src"].Value = node.Attributes["data-src"].Value.ToString();
                        }
                    }
                }
                catch { }
            }

            nodesToModify = doc.DocumentNode.Descendants().ToList();
            foreach (HtmlNode node in nodesToModify)
            {
                try
                {
                    if (node.HasAttributes)
                    {
                        HtmlAttribute at = node.Attributes.Where(a => a.Name == "src").FirstOrDefault();
                        if (at != null && at.OwnerNode.Name != "img")
                            at.Remove();
                    }
                }
                catch { }
            }


            List<HtmlNode> nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name == "nav").ToList();
            foreach (var node in nodesToRemove)
                node.Remove();
            nodesToRemove.Clear();

            nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name == "sup").ToList();
            foreach (var node in nodesToRemove)
                node.Remove();
            nodesToRemove.Clear();


            nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name == "div" || n.Name == "a" || n.Name == "span").ToList();
            foreach (var node in nodesToRemove)
            {
                if (node.HasAttributes)
                {
                    HtmlAttribute at = node.Attributes.Where(a => a.Name == "class").FirstOrDefault();
                    if (at != null)
                    {
                        switch (at.Value)
                        {
                            case "infobox":
                                node.Remove();
                                break;
                            case "add-section-edit-link":
                                node.Remove();
                                break;
                            case "image image-thumbnail link-internal":
                                //node.Remove();
                                break;
                            case "g-ytsubscribe":
                                node.Remove();
                                break;
                            case "editsection":
                                node.Remove();
                                break;
                            default:
                                break;
                        }

                    }

                }
            }
            nodesToRemove.Clear();

            HtmlNode nodeNotes = null;
            nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name == "h2").ToList();
            foreach (var node in nodesToRemove)
            {
                if (node.HasAttributes)
                {
                    HtmlAttribute at = node.Attributes.Where(a => a.Name == "id").FirstOrDefault();
                    if (at != null)
                    {
                        if (at.Value == "Notes-Header")
                        {
                            nodeNotes = node;
                        }
                    }
                }


            }

            


            if (nodeNotes != null)
            {
                nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Line >= nodeNotes.Line).ToList();
                foreach (var node in nodesToRemove)
                    node.Remove();

            }

            //< a class="text" href="http://en.marveldatabase.com/index.php?title=Spider-Man&amp;action=edit">Edit this description</a>
            HtmlNode nodetoremove = doc.DocumentNode.Descendants().Where(n => n.InnerText == "Edit this description").FirstOrDefault();
            if (nodetoremove != null)
            {
                nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Line >= nodetoremove.Line).ToList();

                if (nodesToRemove != null)
                {
                    foreach (var node in nodesToRemove)
                        node.Remove();

                }
            }

            nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name == "div" || n.Name == "tr" || n.Name == "td").ToList();
            foreach (var node in nodesToRemove)
            {
                node.DescendantsAndSelf()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n => n.Remove());
            }
                

            nodesToRemove.Clear();
            nodesToModify.Clear();
            return doc.DocumentNode.InnerHtml.Replace("border-radius:20px;", "border-radius:20px;visibility:collapse;");
        }


        private static string MakeHtmlGood(string html)
        {
            var xmlDoc = XDocument.Parse(html);
            // Remove all inline styles
            xmlDoc.Descendants().Attributes("style").Remove();

            // Remove all classes inserted by 3rd party, without removing our own lovely classes
            foreach (var node in xmlDoc.Descendants())
            {
                var classAttribute = node.Attributes("class").SingleOrDefault();
                if (classAttribute == null)
                {
                    continue;
                }
                var classesThatShouldStay = classAttribute.Value.Split(' ').Where(className => !className.StartsWith("abc"));
                classAttribute.SetValue(string.Join(" ", classesThatShouldStay));

            }

            return xmlDoc.ToString();
        }

        /// <summary>
        /// Parsea e inyecta css
        /// </summary>
        /// <param name="xhtml"></param>
        /// <returns></returns>
        public static string ParseHtmlCodeWithCss(String xhtml)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();            
            sb.Append("<html><head>");            
            Assembly assembly = typeof(WikiReadingHelper).GetTypeInfo().Assembly;

            string js = "";
            using (Stream stream = assembly.GetManifestResourceStream("Xam.Wikia.Common.JavaScript.js"))
            using (StreamReader reader = new StreamReader(stream))
            {
                js = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(js))
            {
                sb.Append("<script type = \"text/javascript\">");
                sb.Append(js);
                sb.Append("</script>");
            }

            string style = "";
            using (Stream stream = assembly.GetManifestResourceStream("Xam.Wikia.Common.Rss.css"))
            using (StreamReader reader = new StreamReader(stream))
            {
                style = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(style))
            {
                sb.Append("<style type='text/css'>");
                sb.Append(style);
                sb.Append("</style>");
            }
            sb.Append("</head>");
            sb.Append("<body style='background: #000000;'>");
            sb.Append(xhtml);
            sb.Append("</body></html>");

            String str = sb.ToString();
            System.Diagnostics.Debug.WriteLine(str);
            //sb = sb.Replace("class=alignleft", " style=\"width: auto; margin: 0 20px 20px 0\"");
            return str;
        }
    }
}
