namespace HtmlAgilityPack.NetCore.Query
{
    public interface INodeAttribute
    {
        string Name { get; }
        INode OwnerNode { get; }
        string Value { get; }
        INodeAttribute Clone();
        void Remove();
    }
}
