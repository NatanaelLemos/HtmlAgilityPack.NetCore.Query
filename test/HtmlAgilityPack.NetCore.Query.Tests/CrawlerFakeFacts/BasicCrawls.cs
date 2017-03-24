using System;
using Xunit;
using HtmlAgilityPack.NetCore.Query;
using HtmlAgilityPack.NetCore.Query.Implementation;
using HtmlAgilityPack;
using System.Linq;

namespace HtmlAgilityPack.NetCore.Query.Tests.CrawlerFakeFacts
{
    public class BasicCrawls
    {
        private string _fakeSite = @"<!doctype html>
                                    <html>
                                        <head>
                                            <title>Fake site</title>
                                        </head>
                                        <body>
                                            <header>
                                                <div class='row'>
                                                    <div class='col-md-10'>
                                                        big column
                                                    </div>
                                                    <div class='col-md-2'>
                                                        little column
                                                    </div>
                                                </div>
                                                <table>
                                                    <tbody id='table-body'>
                                                        <tr>
                                                            <td>cell</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </header>
                                            <main>
                                                this is main
                                                <div id='myDiv'></div>
                                            </main>
                                            <footer>
                                                <div class='dv1'>
                                                    test string search
                                                </div>
                                                <div class='dv2'>
                                                    <div class='dv21'>
                                                        <div class='dv211'>
                                                            test string search
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='dv3'>
                                                    <div class='dv31'>
                                                    </div>
                                                    test string search
                                                </div>
                                            </footer>
                                        </body>
                                    </html>";

        [Fact]
        public void Test_if_tag_selector_works()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_fakeSite);

            var node = CrawlerNode.Build(doc.DocumentNode).Reach("main");
            Assert.True(node.First().InnerText.Trim().Equals("this is main"));
        }

        [Fact]
        public void Test_if_class_selector_works()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_fakeSite);

            var node = CrawlerNode.Build(doc.DocumentNode).Reach(".col-md-2");
            Assert.True(node.First().InnerText.Trim().Equals("little column"));
        }
        
        [Fact]
        public void Test_if_id_selector_works()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_fakeSite);

            var node = CrawlerNode.Build(doc.DocumentNode).Reach("#table-body");
            Assert.True(node.First().InnerText.Trim().Equals("cell"));
        }

        [Fact]
        public void Test_if_string_selector_works()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_fakeSite);

            var nodes = CrawlerNode.Build(doc.DocumentNode).Reach("'test string search'");
            Assert.True(nodes.Count() == 3);
        }
    }
}
