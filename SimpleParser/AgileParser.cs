using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleParser
{
    class AgileParser
    {
        public void Parse()
        {
            Dictionary<string, List<List<string>>> dict = Parse("https://www.readfootball.com/tables.html");
            Dictionary<string, List<List<string>>> dict2 = Parse_Beta("https://www.google.com/");
        }

        public Dictionary<string, List<List<string>>> Parse(string url)
        {
            try
            {
                Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();

                using (HttpClientHandler handler = new HttpClientHandler() { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
                {
                    using (var client = new HttpClient(handler))
                    {
                        using (HttpResponseMessage response = client.GetAsync(url).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var html = response.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                    doc.LoadHtml(html);
                                    
                                    var tables = doc.DocumentNode.SelectNodes(".//div[@class='block_content']//div[@class='two-table-row']//div[@class]");
                                    if (tables != null && tables.Count > 0)
                                    {
                                        foreach (var table in tables)
                                        {
                                            var titleNode = table.SelectSingleNode(".//div[@class='head_tb']");
                                            if (titleNode != null)
                                            {
                                                var table_b = table.SelectSingleNode(".//div[@class='tab_champ']//table");
                                                if (table_b != null)
                                                {
                                                    var rows = table_b.SelectNodes(".//tr");
                                                    if (rows != null && rows.Count > 0)
                                                    {
                                                        var res = new List<List<string>>();

                                                        foreach (var row in rows) 
                                                        {
                                                            var cells = row.SelectNodes(".//td");
                                                            if (cells != null && cells.Count > 0)
                                                            {
                                                                res.Add(new List<string>(cells.Select(c => c.InnerText)));
                                                            }
                                                        }

                                                        result[titleNode.InnerText] = res;
                                                    }
                                                }
                                            }
                                        }

                                        return result;
                                    }
                                    else { Console.WriteLine("Oooooopssss"); }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public Dictionary<string, List<List<string>>> Parse_Beta(string url)
        {
            try
            {
                Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();

                using (HttpClientHandler handler = new HttpClientHandler() { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
                {
                    using (var client = new HttpClient(handler))
                    {
                        using (HttpResponseMessage response = client.GetAsync(url).Result)
                        {
                            if (response.IsSuccessStatusCode)//
                            {
                                var html = response.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                    doc.LoadHtml(html);

                                    //var el = doc.GetElementbyId("skip-link");
                                    var tab = doc.DocumentNode.SelectNodes(".//div[@class]");
                                    foreach (HtmlAgilityPack.HtmlNode node in tab)
                                    {
                                        string s = "";
                                        foreach (HtmlAgilityPack.HtmlAttribute att in node.Attributes)
                                        {
                                            if (att.Name == "class") Console.WriteLine("Class: " + att.Value); 
                                            
                                        }
                                        //HtmlAgilityPack.HtmlNode n = GetPointedNode("o3j99 n1xJcf Ne6nSd", node);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        private HtmlAgilityPack.HtmlNode GetPointedNode(string className, HtmlAgilityPack.HtmlNode node)
        {
            if (node.ChildNodes.Count == 0) return null;
            foreach (HtmlAgilityPack.HtmlNode htmlNode in node.ChildNodes)
            {
                string name = htmlNode.Name;
                if (htmlNode.Name == className)
                {
                    return htmlNode;
                }

                HtmlAgilityPack.HtmlNode nextNode = GetPointedNode(className, htmlNode);
                if (nextNode != null)
                {
                    return nextNode;
                }
            }

            return null;
        }
    }
}
