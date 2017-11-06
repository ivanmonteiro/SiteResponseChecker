using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using SiteResponseChecker.Domain;
using SiteResponseChecker.Domain.Exceptions;
using System.Linq;
using System.Xml.Linq;


namespace SiteResponseChecker.ApplicationLogic.Utils
{
    public static class SiteHtmlUtil
    {
        //public static string StripHTML(string HTMLText)
        //{
        //    Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
        //    return reg.Replace(HTMLText, "");
        //}

        public static string StripHTMLAdvanced(string HTMLText)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(HTMLText);
            return new HtmlToTextConverter().ConvertToText(doc.DocumentNode.ChildNodes);
            //foreach(var script in doc.DocumentNode.Descendants("script").ToArray())
            //    script.Remove();
            //foreach(var style in doc.DocumentNode.Descendants("style").ToArray())
            //    style.Remove();

            //var sb = new StringBuilder();
            //foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            //{
            //    if (!node.HasChildNodes)
            //    {
            //        string text = node.InnerText;
            //        if (!string.IsNullOrEmpty(text))
            //            sb.AppendLine(text.Trim());
            //    }
            //}
            //return doc.DocumentNode.InnerText;
        }

        public static string GetSiteHtml(Site site)
        {
            string html = "";

            try
            {
                WebRequest webRequest = WebRequest.Create(site.SiteUrl);
                WebResponse webResponse = webRequest.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                if (webStream != null)
                {
                    StreamReader webStreamReader = new StreamReader(webStream);
                    html = webStreamReader.ReadToEnd();
                    webStream.Close();
                }
                webResponse.Close();

            }
            catch (Exception ex)
            {
                throw new RequestException(ex);
            }
            
            try
            {
                if (site.CheckSpecificElement && !String.IsNullOrEmpty(site.SpecificElement))
                {
                    html = new SpecificElementResponseChecker().GetElementContents(html, site.SpecificElement, site.SpecificElementType);
                }
            }
            catch (Exception ex)
            {
                throw new SpecificElementException(ex);
            }

            return html;
        }
    }
}
