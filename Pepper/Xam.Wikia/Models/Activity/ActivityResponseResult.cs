using System.Collections.Generic;

namespace Xam.Wikia.Models.Activity
{
    public class ActivityResponseResult
    {
        public IList<ActivityResponseItem> Items { get; set; }
        public string Basepath { get; set; }
    }
}