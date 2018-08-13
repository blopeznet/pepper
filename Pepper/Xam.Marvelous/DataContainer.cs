using System.Collections.Generic;

namespace Xam.Marvelous
{
    public class DataContainer<T> where T : class, new()
    {
        public string Offset { get; set; }
        public string Limit { get; set; }
        public string Total { get; set; }
        public string Count { get; set; }
        public List<T> Results { get; set; }
    }
}
