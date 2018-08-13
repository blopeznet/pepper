using System.Collections.Generic;
using Xam.Marvelous.Model.Base.Summaries.Summaries;

namespace Xam.Marvelous.Model.Base.Summaries.Lists
{
    public class CreatorList
    {
        public string Available { get; set; }
        public string Returned { get; set; }
        public string CollectionURI { get; set; }
        public List<CreatorSummary> Items { get; set; }    
    }
}
