using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SharpArch.Domain.DomainModel;

namespace SiteResponseChecker.Domain
{
    public class Site : Entity
    {
        public virtual int Id { get; set; }

        public virtual string NotificationEmail { get; set; }

        public virtual string SiteName { get; set; }

        public virtual string SiteUrl { get; set; }

        public virtual int CheckInterval { get; set; }

        public virtual bool CheckSpecificElement { get; set; }

        public virtual string SpecificElement { get; set; }

        public virtual SpecificElementType SpecificElementType { get; set; }

        public virtual User User { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual ICollection<SiteResponse> SiteResponses { get; set; }

        public virtual ICollection<SiteError> SiteErrors { get; set; }
    }

    public enum SpecificElementType
    {
        CssSelector,
        XPathExpression
    }
}
