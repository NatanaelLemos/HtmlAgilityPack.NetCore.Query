using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;
using HtmlAgilityPack.NetCore.Query;
using System.Threading.Tasks;

namespace HtmlAgilityPack.NetCore.Query.Implementation
{
    public class Crawler : ICrawler
    {
        /// <summary>Load the site</summary>
        /// <param name="url">Url to the site</param>
        /// <returns>Document node of the loaded site</returns>
        public INode Load(string url)
        {
            try
            {
                if (!TestValidUrl(url))
                {
                    return null;
                }

                using (var client = new HttpClient())
                using (var pageStream = client.GetAsync(url).Result.Content.ReadAsStreamAsync().Result)
                using (var streamReader = new StreamReader(pageStream, Encoding.UTF8))
                {
                    var pageString = streamReader.ReadToEnd();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(pageString);
                    return CrawlerNode.Build(doc.DocumentNode);
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<INode> LoadAsync(string url)
        {
            try
            {
                if (!TestValidUrl(url))
                {
                    return null;
                }

                HttpClient client = null;
                HttpResponseMessage response = null;
                Stream resultStream = null;
                StreamReader reader = null;

                try
                {
                    client = new HttpClient();
                    response = await client.GetAsync(url);
                    resultStream = await response.Content.ReadAsStreamAsync();

                    reader = new StreamReader(resultStream, Encoding.UTF8);

                    var pageString = reader.ReadToEnd();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(pageString);
                    return CrawlerNode.Build(doc.DocumentNode);
                }
                finally
                {
                    if (client != null) client.Dispose();
                    if (response != null) response.Dispose();
                    if (resultStream != null) resultStream.Dispose();
                    if (reader != null) reader.Dispose();
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private bool TestValidUrl(string url)
        {
            Uri uriResult;
            var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult);

            if (!result)
            {
                return false;
            }

            return uriResult.Scheme.ToUpper().Equals("HTTP") || uriResult.Scheme.ToUpper().Equals("HTTPS");
        }
    }
}