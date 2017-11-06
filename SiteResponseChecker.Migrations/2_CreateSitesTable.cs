using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(2)]
    public class CreateSitesTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Sites")
                .WithColumn("SiteId").AsInt32().NotNullable().PrimaryKey().Identity().Indexed()
                .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "UserId").Indexed()
                .WithColumn("Enabled").AsBoolean().NotNullable()
                .WithColumn("NotificationEmail").AsString(100).NotNullable()
                .WithColumn("SiteName").AsString(100).NotNullable()
                .WithColumn("SiteUrl").AsString(500).NotNullable()
                .WithColumn("CheckInterval").AsInt32().NotNullable()
                .WithColumn("CheckSpecificElement").AsBoolean().NotNullable()
                .WithColumn("SpecificElement").AsString(100).Nullable();
        }
    }
}
