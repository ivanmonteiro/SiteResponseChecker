using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(3)]
    public class CreateSiteResponsesTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("SiteResponses")
                .WithColumn("SiteResponseId").AsInt32().NotNullable().PrimaryKey().Identity().Indexed()
                .WithColumn("SiteId").AsInt32().NotNullable().ForeignKey("Sites", "SiteId").Indexed()
                .WithColumn("Contents").AsString(4096).NotNullable()
                .WithColumn("Hash").AsString(100).NotNullable()
                .WithColumn("StatusCode").AsString(50).NotNullable()
                .WithColumn("CheckDate").AsDateTime().NotNullable();
        }
    }
}
