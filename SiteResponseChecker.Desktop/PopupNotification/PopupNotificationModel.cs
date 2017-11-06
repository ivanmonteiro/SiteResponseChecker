using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteResponseChecker.Desktop
{
    public class PopupNotificationModel
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string TextDiff { get; set; }
        public string SiteUri { get; set; }
    }
}
