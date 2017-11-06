using System;
using System.Collections.Generic;
using SharpArch.Domain.DomainModel;

namespace SiteResponseChecker.Domain
{
    public class User : Entity
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual string Email { get; set; }

        public virtual int SnapshotInterval { get; set; }

        public virtual DateTime? LastSnapshotDate { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}
