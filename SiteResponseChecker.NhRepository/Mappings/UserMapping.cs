using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("Users");

            Id(x => x.Id, "UserId").UnsavedValue(0);
            Map(x => x.UserName).Length(20).Not.Nullable();
            Map(x => x.Password).Length(20).Not.Nullable();
            Map(x => x.Email).Length(50).Not.Nullable();
            Map(x => x.CreateDate).Not.Nullable();
            Map(x => x.SnapshotInterval).Not.Nullable();
            Map(x => x.LastSnapshotDate).Nullable();

            HasMany(x => x.Sites)
                .KeyColumn("UserId")
                .Cascade.All();
        }
    }
}
