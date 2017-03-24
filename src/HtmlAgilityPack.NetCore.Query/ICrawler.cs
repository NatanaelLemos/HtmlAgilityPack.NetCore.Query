namespace HtmlAgilityPack.NetCore.Query
{
    public interface ICrawler
    {
        INode Load(string url);
    }
}
