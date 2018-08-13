using Xam.Rss.Feeds;

namespace Xam.Rss.Parser
{
    internal interface IFeedParser
    {
        BaseFeed Parse(string feedXml);
    }
}
