namespace Xam.Wikia.Models.Search
{
    public class LocalWikiSearchResult
    {
        public string Quality { get; set; }
        public string Url { get; set; }
        public string Ns { get; set; }
        public string Id { get; set; }

        public long NumericId { get { return System.Convert.ToInt64(Id); } }
        public string Title { get; set; }
        public string Snippet { get; set; }
    }
}