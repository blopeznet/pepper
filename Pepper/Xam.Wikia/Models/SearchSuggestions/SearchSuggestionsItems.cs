namespace Xam.Wikia.Models.SearchSuggestions
{
    public class SearchSuggestionsItems
    {
        /// <summary>
        /// Searching article title
        /// </summary>
        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}