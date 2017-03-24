using HtmlAgilityPack;
using HtmlAgilityPack.NetCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlAgilityPack.NetCore.Query.Implementation
{
    /// <summary>
    /// Proxy class that wraps HtmlNode, implements the INode interface (to mock and implements the Reach method)
    /// </summary>
    public class CrawlerNode : HtmlNode, INode
    {
        /// <summary>
        /// Real HtmlNode (hidden for outside)
        /// </summary>
        private HtmlNode _node;

        /// <summary>
        /// Proxy Attributes.
        /// This attributes will have the same behavior of HtmlAttributes but implements the INodeAttribute interface
        /// </summary>
        public new IEnumerable<INodeAttribute> Attributes
        {
            get
            {
                foreach (var attr in _node.Attributes)
                {
                    //yield return will return a pseudo-method that will run only if necessary
                    //ex. if you run an Attributes.First(), this collection will build only the first element,
                    //instead of build all items before return
                    yield return CrawlerNodeAttribute.Build(attr);
                }
            }
        }

        /// <summary>
        /// Proxy ChildNodes.
        /// This nodes will have the same behavior of HtmlNodes but implements the INode interface
        /// </summary>
        public new IEnumerable<INode> ChildNodes
        {
            get
            {
                foreach (var child in _node.ChildNodes)
                {
                    yield return CrawlerNode.Build(child);
                }
            }
        }

        public new INode ParentNode => CrawlerNode.Build(_node.ParentNode);

        public new string InnerText => _node.InnerText;
        public new string InnerHtml => _node.InnerHtml;
        public new string OuterHtml => _node.OuterHtml;
        public new string Name => _node.Name;

        /// <summary>
        /// Do not use this constructor. To build a CrawlerNode, use "Build" static method
        /// </summary>
        [Obsolete("Do not use this constructor. To build a CrawlerNode, use \"Build\" static method")]
        public CrawlerNode(HtmlNodeType type, HtmlDocument ownerdocument, int index) :
            base(type, ownerdocument, index)
        {
            throw new NotImplementedException("Do not use this constructor. To build a CrawlerNode, use \"Build\" static method");
        }

        /// <summary>
        /// Returns an instance of CrawlerNode
        /// </summary>
        /// <param name="node">The HtmlNode that will be Wrapped inside the CrawlerNode</param>
        /// <returns>The CrawlerNode instance</returns>
        public static CrawlerNode Build(HtmlNode node) => new CrawlerNode(node);

        private CrawlerNode(HtmlNode node)
            : base(HtmlNodeType.Comment, node.OwnerDocument, 0)
        {
            _node = node;
        }

        /// <summary>
        /// Returns an IEnumerable of ALL items found using the selector.
        /// This method will run recursively until get all nodes, like JQuery. And accept a CSS selector (class, id, tag)
        /// </summary>
        /// <param name="selector">A CSS selector</param>
        /// <returns>All nodes that match the selector</returns>
        public IEnumerable<INode> Reach(string selector)
        {
            if (selector.StartsWith("."))
            {
                if (Attributes.Any(a => a.Name.Equals("class") && a.Value.Contains(selector.Substring(1))))
                    yield return this;
            }
            else if (selector.StartsWith("#"))
            {
                if (Attributes.Any(a => a.Name.Equals("id") && a.Value.Contains(selector.Substring(1))))
                    yield return this;
            }
            else if (selector.StartsWith("'") && selector.EndsWith("'"))
            {
                //BUG: IF SEARCHS FOR STRING THAT IS CONTAINED IN AN ELEMENT AND ITS CHILDREN, WILL NOT WORK
                //EX: <div> test <div> test </div> </div>
                var strSelector = selector.Substring(1, selector.Length - 2);
                var strUTF8Selector = strSelector.Replace("á", "&aacute;"); //UNDONE: NO IDEA HOW TO CORRECT THIS

                var thisContainsSelector = InnerHtml.Contains(strSelector) || InnerHtml.Contains(strUTF8Selector);
                var childrenContainsSelector = ChildNodes.Any() && ChildNodes.Any(n => n.InnerHtml.Contains(strSelector) || n.InnerHtml.Contains(strUTF8Selector));

                if (thisContainsSelector && (!childrenContainsSelector))
                {
                    yield return this;
                }
            }
            else
            {
                if (Name.Equals(selector))
                    yield return this;
            }

            foreach (var child in ChildNodes)
            {
                foreach (var reached in child.Reach(selector))
                {
                    if (reached != null) yield return reached;
                }
            }
        }
    }
}
