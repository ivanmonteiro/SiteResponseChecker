using System;
using SharpArch.Domain.DomainModel;

namespace SiteResponseChecker.Domain
{
    public class SiteError : Entity
    {
        public virtual int Id { get; set; }
        
        public virtual string ErrorType { get; set; }

        public virtual string ErrorDetails { get; set; }

        public virtual bool IsRecurring { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual Site Site { get; set; }
    }
}
