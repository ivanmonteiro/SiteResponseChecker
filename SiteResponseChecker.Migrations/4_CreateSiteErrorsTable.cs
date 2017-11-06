using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(4)]
    public class CreateSiteErrorsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("SiteErrors")
                .WithColumn("SiteErrorId").AsInt32().NotNullable().PrimaryKey().Identity().Indexed()
                .WithColumn("SiteId").AsInt32().NotNullable().ForeignKey("Sites", "SiteId").Indexed()
                .WithColumn("ErrorType").AsString(50).NotNullable()
                .WithColumn("ErrorDetails").AsString(4096).NotNullable()
                .WithColumn("IsRecurring").AsBoolean().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();
        }
    }
}
