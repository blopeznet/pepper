using System.Collections.Generic;

namespace Xam.Wikia.Models.Article.Details
{
    public class ExpandedArticleResultSet
    {
        public Dictionary<string, ExpandedArticle> Items { get; set; }
        public string BasePath { get; set; }
    }
}