using HtmlAgilityPack;
using HtmlAgilityPack.NetCore.Query;

namespace HtmlAgilityPack.NetCore.Query.Implementation
{
    public class CrawlerNodeAttribute : INodeAttribute
    {
        private HtmlAttribute _attribute;

        public static CrawlerNodeAttribute Build(HtmlAttribute attribute) => new CrawlerNodeAttribute(attribute);
        private CrawlerNodeAttribute(HtmlAttribute attribute)
        {
            _attribute = attribute;
        }

        public string Name => _attribute.Name;

        public HtmlDocument OwnerDocument => _attribute.OwnerDocument;

        public INode OwnerNode => CrawlerNode.Build(_attribute.OwnerNode);

        public string Value => _attribute.Value;

        public INodeAttribute Clone() => Build(_attribute.Clone());

        public void Remove() => _attribute.Remove();

    }
}