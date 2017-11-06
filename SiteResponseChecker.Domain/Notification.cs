using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace SiteResponseChecker.Domain
{
    public class Notification : Entity
    {
        public virtual int Id { get; set; }
        
        public virtual DateTime NotificationDate { get; set; }
        
        public virtual DateTime? SendDate { get; set; }

        public virtual string SendError { get; set; }

        public virtual bool IsSent { get; set; }

        public virtual string Subject { get; set; }
        
        public virtual string Contents { get; set; }

        public virtual Site Site { get; set; }
    }
}
