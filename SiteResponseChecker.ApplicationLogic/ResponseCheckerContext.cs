using SiteResponseChecker.Domain;
using SiteResponseChecker.NhRepository;

namespace SiteResponseChecker.ApplicationLogic
{
    public class ResponseCheckerContext
    {
        public Site Site { get; set; }
        public SiteResponse LastResponse { get; set; }
        public SiteRepository SiteRepository { get; set; }
        public SiteResponseRepository SiteResponseRepository { get; set; }
        public SiteErrorRepository SiteErrorRepository { get; set; }
        public NotificationsRepository NotificationsRepository { get; set; }
    }
}
