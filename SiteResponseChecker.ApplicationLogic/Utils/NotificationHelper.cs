using System;
using System.Text;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using SiteResponseChecker.Domain;
using System.Net.Mail;

namespace SiteResponseChecker.ApplicationLogic.Utils
{
    public class NotificationHelper
    {
        public Notification CreateNotification(Site site, SiteResponse currentResponse, SiteResponse lastResponse)
        {
            string subject = String.Format("Site Response Checker Notification: Site {0} changed", site.SiteName);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Site Response Checker Notification for <b><a href=\"{0}\">{1}</a></b><br />" +
                            "Changed at {2}", site.SiteUrl, site.SiteName, currentResponse.CheckDate.ToString());
            //sb.AppendFormat("<br /><br /><p><strong>New response:</strong></p><br />{0} <br /><br /><br /><p><strong>Old response:</strong></p><br />{1}", SiteHtmlUtil.StripHTMLAdvanced(currentResponse.Contents), SiteHtmlUtil.StripHTMLAdvanced(lastResponse.Contents));
            string diff = GetDiff(currentResponse, lastResponse);
            diff = diff.Replace("\r\n", "<br \\>");
            sb.AppendFormat("<br /><br /><p><strong>Differences from last version:</strong></p><br />{0} <br /><br />", diff);

            sb.AppendFormat("<br /><br /><p><strong>Current HTML Version:</strong></p><br />{0} <br /><br />", currentResponse.Contents);

            Notification notification = new Notification();
            notification.NotificationDate = DateTime.Now;
            notification.Site = site;
            notification.Subject = subject;
            notification.Contents = sb.ToString();
            return notification;
            //SendEmail(site.User.Email, subject, sb.ToString());
        }

        public string GetDiff(SiteResponse currentResponse, SiteResponse lastResponse)
        {
            string currentResponseText = SiteHtmlUtil.StripHTMLAdvanced(currentResponse.Contents);
            string lastResponseText = "";

            if (lastResponse != null)
            {
                lastResponseText = SiteHtmlUtil.StripHTMLAdvanced(lastResponse.Contents);    
            }
            
            StringBuilder stringBuilder = new StringBuilder();
            Differ differ = new Differ();
            InlineDiffBuilder inlineDiffBuilder = new InlineDiffBuilder(differ);
            var result = inlineDiffBuilder.BuildDiffModel(lastResponseText, currentResponseText);
            bool added_anchor_to_first_change = false;

            foreach (var line in result.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                {
                    if(!added_anchor_to_first_change)
                    {
                        stringBuilder.Append("<p class='added'><a name='anchor'>+</a> ");
                        added_anchor_to_first_change = true;
                    }
                    else
                    {
                        stringBuilder.Append("<p class='added'>+ ");
                    }
                    stringBuilder.AppendLine(line.Text);
                    stringBuilder.Append("</p>");
                }
                else if (line.Type == ChangeType.Deleted)
                {
                    if (!added_anchor_to_first_change)
                    {
                        stringBuilder.Append("<p class='removed'><a name='anchor'>-</a> ");
                        added_anchor_to_first_change = true;
                    }
                    else
                    {
                        stringBuilder.Append("<p class='removed'>- ");
                    }
                    stringBuilder.AppendLine(line.Text);
                    stringBuilder.Append("</p>");
                }
                else
                {
                    stringBuilder.Append("<p class='notchanged'>");
                    stringBuilder.AppendLine(line.Text);
                    stringBuilder.Append("</p>");
                }

                //stringBuilder.Append("<br/>");
            }
            return stringBuilder.ToString();
        }


        public static string GetBodyTopHtml()
        {
            string top_html = 
                  @"<!DOCTYPE html>
                    <html lang='pt-BR'>
                    <head>
                        <meta charset='UTF-8' />
                        <title>Notification</title>
                    </head>
                    <style>
	                    body {
		                    background-color: #eee;
	                    }
	                    p {
		                    margin-top: 0px;
		                    margin-bottom: 0px;		
	                    }
	                    .added {
		                    color: black; 
		                    background-color: #8CFF7A;
	                    }
	                    .removed {
		                    color: black; 
		                    background-color: #FA6B64;
	                    }
	                    .notchanged {
	                    }
                    </style>
                    <body>";
            return top_html;
        }

        public static string GetBottomHtml()
        {
            return "\r\n</body>\r\n</html>";
        }

    }
}