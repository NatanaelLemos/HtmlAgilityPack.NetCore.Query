using System.Collections.Generic;

namespace HtmlAgilityPack.NetCore.Query
{
    public interface INode
    {
        IEnumerable<INode> ChildNodes { get; }
        IEnumerable<INodeAttribute> Attributes { get; }
        IEnumerable<INode> Reach(string selector);

        INode ParentNode { get; }

        string InnerText { get; }
        string InnerHtml { get; }
        string OuterHtml { get; }
        string Name { get; }
    }
}
