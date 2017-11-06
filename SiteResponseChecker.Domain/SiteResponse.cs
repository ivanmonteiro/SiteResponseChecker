using System;
using SharpArch.Domain.DomainModel;

namespace SiteResponseChecker.Domain
{
    public class SiteResponse : Entity
    {
        public virtual int Id { get; set; }

        public virtual DateTime CheckDate { get; set; }

        public virtual string Contents { get; set; }

        public virtual string Diff { get; set; }

        public virtual string StatusCode { get; set; }

        public virtual string Hash { get; set; }

        public virtual Site Site { get; set; }
    }
}
