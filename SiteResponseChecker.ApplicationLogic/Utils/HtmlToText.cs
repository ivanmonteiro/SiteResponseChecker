using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SiteResponseChecker.ApplicationLogic.Utils
{
    public class HtmlToTextConverter
    {
        private readonly HashSet<string> _elementsToSkip = new HashSet<string> { "script", "style", "label", "textarea", "button", "option", "select" };
        private readonly HashSet<string> _elementsWithNewLine = new HashSet<string> { "ul", "li", "table", "tr", "th", "div", "p", "h1", "h2", "h3", "h4" };

        public string ConvertToText(IEnumerable<HtmlNode> nodes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var node in nodes)
            {
                ConvertToText(node, sb);
            }

            return sb.ToString();
        }

        private bool ConvertToText(HtmlNode node, StringBuilder sb)
        {
            if (node.NodeType == HtmlNodeType.Comment)
                return false;

            if (node.NodeType == HtmlNodeType.Text)
            {
                var text = node.InnerText;
                text = HtmlEntity.DeEntitize(text);
                text = Regex.Replace(text.Trim(), @"\s+", " ");
                
                //if(node.NodeType.ToString().ToLower().Contains()
                //Changed to add line so it's more readable
                if (!string.IsNullOrEmpty(text))
                {
                    sb.Append(text);
                    sb.Append(" ");
                    return true;
                }
                return false;
            }

            if (_elementsToSkip.Contains(node.Name))
            {
                return false;
            }

            //Commented because it was preventing some links to show it's text
            /*
             if (node.Name == "a")
            {
                var siblings = node.ParentNode.ChildNodes;

                if (!siblings.Any(n => n.Name != "a" && !string.IsNullOrEmpty(node.InnerText)))
                {
                    return false;
                }
            }
            */

            var written = false;

            foreach (var child in node.ChildNodes)
            {
                if (ConvertToText(child, sb))
                {
                    written = true;
                }
            }

            if (written && _elementsWithNewLine.Contains(node.Name))
            {
                sb.AppendLine();
            }

            return written;
        }
    }
}