using System.Collections.Generic;
using Xam.Marvelous.Model.Base.Summaries.Summaries;

namespace Xam.Marvelous.Model.Base.Summaries.Lists
{
    public class CharacterList
    {
        public string Available { get; set; }
        public string Returned { get; set; }
        public string CollectionURI { get; set; }
        public List<CharacterSummary> Items { get; set; }
    }
}
