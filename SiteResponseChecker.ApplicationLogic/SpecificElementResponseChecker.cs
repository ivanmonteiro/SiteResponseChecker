using System.Collections.Generic;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.ApplicationLogic
{
    public class SpecificElementResponseChecker
    {
        public string GetElementContents(string rawHtml, string selector, SpecificElementType type)
        {
            // Load the document using HTMLAgilityPack as normal
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(rawHtml);
            IEnumerable<HtmlNode> matches = null;
            //contents = document.QuerySelector(cssSelector).WriteContentTo();
            if (type == SpecificElementType.CssSelector)
            {
                // Fizzler for HtmlAgilityPack is implemented as the 
                // QuerySelectorAll extension method on HtmlNode
                matches = htmlDocument.DocumentNode.QuerySelectorAll(selector);
            }
            else
            {
                matches = htmlDocument.DocumentNode.SelectNodes(selector);
            }
            
            string contents = "";
            
            if (matches != null)
            {
                foreach (var htmlNode in matches)
                {
                    contents += htmlNode.WriteTo();
                }
            }
            
            return contents;
        }
    }
}
